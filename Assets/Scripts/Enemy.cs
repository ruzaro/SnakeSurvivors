using UnityEngine;

namespace SnakeSurvivors
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 0.95f;
        [SerializeField] private Transform target;
        
        [SerializeField] private float collisionRadius = 0.2f;
        
        private Transform _target;
        private Transform _transform;
        private SpatialPartitioner _spatialPartitioner;

        private SpatialPartition _spatialPartition;

        public float CollisionRadius => collisionRadius;

        private void Awake()
        {
            _transform = transform;
            _target = target;
        }

        public Enemy SetTarget(Transform target)
        {
            _target = target;
            return this;
        }

        public Enemy AddSpatialPartitioner(SpatialPartitioner spatialPartitioner)
        {
            _spatialPartitioner = spatialPartitioner;
            if (_spatialPartition != null)
            {
                _spatialPartition.Remove(gameObject);
            }

            _spatialPartition = _spatialPartitioner.GetPartition(_transform.position);
            _spatialPartition.Add(gameObject);
            return this;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            var dest = _target.transform.position;

            var curr = _transform.position;

            var delta = dest - curr;

            var direction = delta.normalized;

            var movement = direction * speed * Time.deltaTime;

            _transform.position = curr + movement;
            
            _spatialPartition = _spatialPartition.UpdatePartition(gameObject);
            // TODO push other enemies
        }
    }
}