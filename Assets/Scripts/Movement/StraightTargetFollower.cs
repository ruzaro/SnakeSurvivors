using UnityEngine;

namespace SnakeSurvivors
{
    public class StraightTargetFollower : MonoBehaviour
    {
        [SerializeField] private float speed = 0.95f;
        
        private Transform _target;
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        private void Update()
        {
            if (_target == null) return;

            var dest = _target.transform.position;

            var curr = transform.position;

            var delta = dest - curr;

            var direction = delta.normalized;

            var movement = direction * speed * Time.deltaTime;
            
            transform.position = curr + movement;
        }
    }
}