using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeSurvivors
{
    public class SpatialPartition : IEnumerable<GameObject>
    {
        public Vector2Int Index { get; private set; }
        
        public SpatialPartitioner SpatialPartitioner { get; private set; }
        
        private readonly HashSet<GameObject> _objects = new();

        public int Count => _objects.Count;
        
        public SpatialPartition(Vector2Int index, SpatialPartitioner spatialPartitioner)
        {
            Index = index;
            SpatialPartitioner = spatialPartitioner;
        }

        public void Add(GameObject gameObject)
        {
            _objects.Add(gameObject);
        }
        
        public void Remove(GameObject go)
        {
            _objects.Remove(go);
        }

        public SpatialPartition UpdatePartition(GameObject gameObject)
        {
            var pos = gameObject.transform.position;

            var partition = SpatialPartitioner.GetPartition(pos);

            if (partition != this)
            {
                partition._objects.Add(gameObject);
                _objects.Remove(gameObject);
            }

            return partition;
        }
        
        public Vector2 Position => new Vector2(Index.x * SpatialPartitioner.PartitionWidth, Index.y *  SpatialPartitioner.PartitionHeight);

        public IEnumerable<SpatialPartition> GetNeighbours(int distance = 1) => 
            SpatialPartitioner.GetPartitionNeighbourhood(Index, distance);

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class SpatialPartitioner : MonoBehaviour
    {
        [SerializeField] private float partitionWidth = 1.0f;
        public float PartitionWidth => partitionWidth;
        
        [SerializeField] private float partitionHeight = 1.0f;
        public float PartitionHeight => partitionHeight;

        private readonly Dictionary<Vector2Int, SpatialPartition> _partitions = new();


        public Vector2Int GetPartitionIndex(Vector2 position)
        {
            var xPos = position.x;
            var yPos = position.y;
            
            var x = Mathf.FloorToInt(xPos / partitionWidth);
            var y = Mathf.FloorToInt(yPos / partitionHeight);
            
            return new Vector2Int(x, y);
        }

        public SpatialPartition GetPartition(Vector2 pos) => GetPartition(GetPartitionIndex(pos));

        public SpatialPartition GetPartition(Vector2Int index)
        {
            if (_partitions.TryGetValue(index, out var partition)) return partition;
            
            partition = new SpatialPartition(index, this);
            _partitions.Add(index, partition);

            return partition;
        }

        public IEnumerable<SpatialPartition> GetPartitionNeighbourhood(Vector2Int index, int distance = 1)
        {
            var centerX = index.x;
            var centerY = index.y;

            var left = centerX - distance;
            var right = centerX + distance;
            var top = centerY - distance;
            var bottom = centerY + distance;

            for (var x = left; x <= right; ++x)
            {
                for (var y = top; y <= bottom; ++y)
                {
                    if (_partitions.TryGetValue(new Vector2Int(x, y), out var partition))
                    {
                        yield return partition;
                    }
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var (index, partition) in _partitions)
            {
                var pos = partition.Position;
                var leftTop = pos.ToVec3();
                var leftBottom = (pos + new Vector2(0, partitionHeight)).ToVec3();
                var rightTop = (pos + new Vector2(partitionWidth, 0)).ToVec3();
                var rightBottom = (pos + new Vector2(partitionWidth, partitionHeight)).ToVec3();

                if (partition.Count > 0)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                
                Gizmos.DrawLine(leftTop, rightTop);
                Gizmos.DrawLine(leftTop, leftBottom);
                Gizmos.DrawLine(rightBottom, rightTop);
                Gizmos.DrawLine(rightBottom, leftBottom);
            }
        }
#endif
    }
}