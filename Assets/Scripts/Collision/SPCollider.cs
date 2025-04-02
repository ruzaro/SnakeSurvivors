using System;
using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class SPCollider : MonoBehaviour
    {
        [SerializeField] private SPPartitioner partitioner;
        
        [SerializeField] private float radius = 1.0f;
        public float Radius => radius;

        [SerializeField] private SPColliderTag colliderTag;
        public SPColliderTag Tag => colliderTag;

        [SerializeField] private UnityEvent<SPCollider> onCollision;
        public void OnCollision(SPCollider other) => onCollision?.Invoke(other);
        
        private SpatialPartitioner _spatialPartitioner;
        private SpatialPartition _spatialPartition;

        private Vector2 _position;
        public Vector2 Position => _position;

        private void Awake()
        {
            AddSpatialPartitioner();
            UpdatePosition();
        }

        private void AddSpatialPartitioner()
        {
            _spatialPartitioner = partitioner.SpatialPartitioner;
            
            _spatialPartition?.Remove(this);
            _spatialPartition = _spatialPartitioner.GetPartition(_position);
            _spatialPartition.Add(this);
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _position = transform.position;
            UpdatePartition();
        }

        private void UpdatePartition()
        {
            var pos = _position;

            var partition = _spatialPartitioner.GetPartition(pos);

            if (partition == _spatialPartition) return;
            
            _spatialPartition.Remove(this);
            _spatialPartition = partition;
            _spatialPartition.Add(this);
        }

        private void OnEnable()
        {
            UpdatePosition();
            
            _spatialPartition?.Remove(this);
            _spatialPartition = _spatialPartitioner.GetPartition(_position);
            _spatialPartition.Add(this);
        }

        private void OnDisable()
        {
            _spatialPartition?.Remove(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}