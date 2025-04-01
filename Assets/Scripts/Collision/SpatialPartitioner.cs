using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO size bigger than partition size

namespace SnakeSurvivors
{
    public class SpatialPartition : IEnumerable<SPCollider>
    {
        public Vector2Int Index { get; }
        
        public SpatialPartitioner SpatialPartitioner { get; }
        
        private readonly HashSet<SPCollider> _objects = new();

        public int Count => _objects.Count;
        
        public SpatialPartition(Vector2Int index, SpatialPartitioner spatialPartitioner)
        {
            Index = index;
            SpatialPartitioner = spatialPartitioner;
        }

        public void Add(SPCollider toAdd)
        {
            _objects.Add(toAdd);
        }
        
        public void Remove(SPCollider toRemove)
        {
            _objects.Remove(toRemove);
        }
        
        public Vector2 Position => new(Index.x * SpatialPartitioner.PartitionWidth, Index.y *  SpatialPartitioner.PartitionHeight);

        public IEnumerable<SpatialPartition> GetNeighbours(int distance = 1) => 
            SpatialPartitioner.GetPartitionNeighbourhood(Index, distance);

        public IEnumerator<SPCollider> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class SpatialPartitioner  : MonoBehaviour, IEnumerable<SpatialPartition>
    {
        private readonly float _partitionWidth;
        public float PartitionWidth => _partitionWidth;
        
        private readonly float _partitionHeight;
        public float PartitionHeight => _partitionHeight;

        private readonly Dictionary<Vector2Int, SpatialPartition> _partitions = new();

        public SpatialPartitioner(float partitionWidth, float partitionHeight)
        {
            _partitionWidth = partitionWidth;
            _partitionHeight = partitionHeight;
        }
        
        public Vector2Int GetPartitionIndex(Vector2 position)
        {
            var xPos = position.x;
            var yPos = position.y;
            
            var x = Mathf.FloorToInt(xPos / _partitionWidth);
            var y = Mathf.FloorToInt(yPos / _partitionHeight);
            
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

        public IEnumerator<SpatialPartition> GetEnumerator()
        {
            foreach (var partition in _partitions.Values)
            {
                yield return partition;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var (_, spatialPartition) in _partitions)
            {
                var pos = spatialPartition.Position;
                var leftTop = pos.ToVec3();
                var leftBottom = (pos + new Vector2(0, PartitionHeight)).ToVec3();
                var rightTop = (pos + new Vector2(PartitionWidth, 0)).ToVec3();
                var rightBottom = (pos + new Vector2(PartitionWidth, PartitionHeight)).ToVec3();

                if (spatialPartition.Count > 0)
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