using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeSurvivors
{
    public interface IPartitionable
    {
        Vector2 Position { get; }
    }
    
    public class SpatialPartition<TObject> : IEnumerable<TObject> where TObject : IPartitionable
    {
        public Vector2Int Index { get; private set; }
        
        public SpatialPartitioner<TObject> SpatialPartitioner { get; private set; }
        
        private readonly HashSet<TObject> _objects = new();

        public int Count => _objects.Count;
        
        public SpatialPartition(Vector2Int index, SpatialPartitioner<TObject> spatialPartitioner)
        {
            Index = index;
            SpatialPartitioner = spatialPartitioner;
        }

        public void Add(TObject toAdd)
        {
            _objects.Add(toAdd);
        }
        
        public void Remove(TObject toRemove)
        {
            _objects.Remove(toRemove);
        }

        public SpatialPartition<TObject> UpdatePartition(TObject toUpdate)
        {
            var pos = toUpdate.Position;

            var partition = SpatialPartitioner.GetPartition(pos);

            if (partition != this)
            {
                partition._objects.Add(toUpdate);
                _objects.Remove(toUpdate);
            }

            return partition;
        }
        
        public Vector2 Position => new Vector2(Index.x * SpatialPartitioner.PartitionWidth, Index.y *  SpatialPartitioner.PartitionHeight);

        public IEnumerable<SpatialPartition<TObject>> GetNeighbours(int distance = 1) => 
            SpatialPartitioner.GetPartitionNeighbourhood(Index, distance);

        public IEnumerator<TObject> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class SpatialPartitioner<TObject>  : IEnumerable<SpatialPartition<TObject>> where TObject : IPartitionable
    {
        private readonly float _partitionWidth;
        public float PartitionWidth => _partitionWidth;
        
        private readonly float _partitionHeight;
        public float PartitionHeight => _partitionHeight;

        private readonly Dictionary<Vector2Int, SpatialPartition<TObject>> _partitions = new();

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

        public SpatialPartition<TObject> GetPartition(Vector2 pos) => GetPartition(GetPartitionIndex(pos));

        public SpatialPartition<TObject> GetPartition(Vector2Int index)
        {
            if (_partitions.TryGetValue(index, out var partition)) return partition;
            
            partition = new SpatialPartition<TObject>(index, this);
            _partitions.Add(index, partition);

            return partition;
        }

        public IEnumerable<SpatialPartition<TObject>> GetPartitionNeighbourhood(Vector2Int index, int distance = 1)
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

        public IEnumerator<SpatialPartition<TObject>> GetEnumerator()
        {
            foreach (var partition in _partitions.Values)
            {
                yield return partition;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}