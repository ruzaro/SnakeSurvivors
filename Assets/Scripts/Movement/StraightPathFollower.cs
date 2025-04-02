using UnityEngine;

namespace SnakeSurvivors
{
    public class StraightPathFollower : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        
        private void Update()
        {
            var dir = transform.up;
            
            var pos = transform.position;
            pos += dir * speed * Time.deltaTime;
            transform.position = pos;
        }
    }
}