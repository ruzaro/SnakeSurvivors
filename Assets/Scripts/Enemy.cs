using System;
using UnityEngine;

namespace SnakeSurvivors
{
    public class Enemy : MonoBehaviour, IPartitionable
    {
        [SerializeField] private float size = 0.2f;
        public float Size => size;
        
        [SerializeField] private float speed = 0.95f;
        [SerializeField] private Transform target;
        [SerializeField] private float pushForce = 0.4f;
        
        public Vector3 Position => _transform.position;
        
        private Transform _target;
        private Transform _transform;
        private SpatialPartitioner<Enemy> _spatialPartitioner;

        private SpatialPartition<Enemy> _spatialPartition;

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

        public Enemy AddSpatialPartitioner(SpatialPartitioner<Enemy> spatialPartitioner)
        {
            _spatialPartitioner = spatialPartitioner;
            if (_spatialPartition != null)
            {
                _spatialPartition.Remove(this);
            }

            _spatialPartition = _spatialPartitioner.GetPartition(_transform.position);
            _spatialPartition.Add(this);
            return this;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            var dest = _target.transform.position;

            var curr = _transform.position;

            var delta = dest - curr;

            var direction = delta.normalized;

            var movement = direction * speed * Time.fixedDeltaTime;

            _transform.position = curr + movement;
            
            _spatialPartition = _spatialPartition.UpdatePartition(this);
            
            PushOtherEnemiesAway();
        }

        private void PushOtherEnemiesAway()
        {
            foreach (var other in _spatialPartition)
            {
                if (other == this) continue;
                
                var otherPos = other.transform.position;
                var thisPos = _transform.position;
                
                var dir = (otherPos - thisPos).normalized;

                if (Vector2.Distance(otherPos, thisPos) < size + other.size)
                {
                    other.transform.position += dir * Time.fixedDeltaTime * pushForce;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, size);
        }
#endif
    }
}