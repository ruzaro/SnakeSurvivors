using UnityEngine;

namespace SnakeSurvivors
{
    public class SmoothTargetFollower : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float speed = 1f;

        [SerializeField] [Min(0.0f)] private float rotationSpeed = 1.0f;
        
        [SerializeField] private float distance = 1.0f;
        public float Distance => distance;

        private Transform _followTarget;
        private Transform _transform;
        
        public void Attach(Transform followTarget)
        {
            _followTarget = followTarget;
        }
        
        private void Awake()
        {
            _transform = transform;
        }

        public void OnUpdate(float deltaTime)
        {
            var dir = _transform.up;

            var targetPos = _followTarget.position;
            targetPos.z = _transform.position.z; // TODO fix this

            var targetRot = (targetPos - _transform.position).normalized;
            
            var rotated = Vector3.RotateTowards(dir, targetRot, rotationSpeed * deltaTime, 0.0f);

            _transform.rotation = Quaternion.LookRotation(Vector3.forward, rotated);

            dir = _transform.up;
            
            var pos = _transform.position;
            if (Vector2.Distance(targetPos, pos) < distance) return;
            
            var dif = dir * speed * deltaTime;
            
            pos += dif;
            _transform.position = pos;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_followTarget == null) return;
            
            var pos = transform.position;
            
            var dir = transform.up;
        
            var dirRight = transform.right;
            
            var targetPos = _followTarget.position;
            targetPos.z = transform.position.z;
        
            var targetRot = (targetPos - transform.position).normalized;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + dir);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + dirRight);
            
            var rotated = Vector3.RotateTowards(dir, targetRot, rotationSpeed, 0.0f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, pos + rotated);
        }
#endif
    }
}