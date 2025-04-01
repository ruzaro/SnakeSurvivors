using System;
using System.Linq;
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
        
        public Vector2 Position => _transform.position;
        
        private Transform _target;
        private Transform _transform;
        private SpatialPartitioner<Enemy> _spatialPartitioner;

        private SpatialPartition<Enemy> _spatialPartition;
        
        public event Action OnDeath;

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

            if (CanMoveThere(curr + movement))
            {
                _transform.position = curr + movement;
                _spatialPartition = _spatialPartition.UpdatePartition(this);
            }
            
            PushOtherEnemiesAway();
        }

        private bool CanMoveThere(Vector2 position)
        {
            foreach (var partition in _spatialPartition.GetNeighbours())
            {
                foreach (var other in partition)
                {
                    if (other == this) continue;
                
                    Vector2 otherPos = other.transform.position;
                    var thisPos = position;

                    if (Vector2.Distance(otherPos, thisPos) < size + other.size)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        private void PushOtherEnemiesAway()
        {
            foreach (var partition in _spatialPartition.GetNeighbours())
            {
                foreach (var other in partition)
                {
                    if (other == this) continue;
                
                    Vector2 otherPos = other.transform.position;
                    Vector2 thisPos = _transform.position;

                    if (Vector2.Distance(otherPos, thisPos) < size + other.size)
                    {
                        var dir = (otherPos - thisPos).normalized;
                        other.transform.position += (dir * Time.fixedDeltaTime * pushForce).ToVec3();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
            _spatialPartition.Remove(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, size);
        }
#endif
    }
}