using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class ActionIfTooFar : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField][Min(0.0f)] private float distance = 20.0f;
        [SerializeField] private UnityEvent<GameObject> action;
        public UnityEvent<GameObject> Action => action;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void Update()
        {
            if (target == null) return;

            if (Vector2.Distance(target.position, transform.position) > distance)
            {
                action?.Invoke(gameObject);
            }
        }
    }
}