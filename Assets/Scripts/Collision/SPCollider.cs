using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class SPCollider : MonoBehaviour
    {
        [SerializeField] private float radius = 1.0f;
        public float Radius => radius;

        [SerializeField] private SPColliderTag colliderTag;
        public SPColliderTag Tag => colliderTag;

        [SerializeField] private UnityEvent<SPCollider> onCollision;
        public void OnCollision(SPCollider other) => onCollision?.Invoke(other);
        
        private SpatialPartitioner _spatialPartitioner;
        private SpatialPartition _spatialPartition;

        private Vector2 _position;

        private void Awake()
        {
            // TODO add partitioner
            UpdatePosition();
        }

        public void AddSpatialPartitioner(SpatialPartitioner spatialPartitioner)
        {
            _spatialPartitioner = spatialPartitioner;
            _spatialPartition?.Remove(this);

            _spatialPartition = _spatialPartitioner.GetPartition(_position);
            _spatialPartition.Add(this);
        }
        
        public void UpdatePosition()
        {
            _position = transform.position;
            UpdatePartition();
        }
        
        public void UpdatePartition()
        {
            var pos = _position;

            var partition = _spatialPartitioner.GetPartition(pos);

            if (partition == _spatialPartition) return;
            
            _spatialPartition.Remove(this);
            _spatialPartition = partition;
            _spatialPartition.Add(this);
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