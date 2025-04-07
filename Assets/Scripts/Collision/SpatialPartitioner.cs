using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

// TODO size bigger than partition size
// TODO removing elements on iterations

namespace SnakeSurvivors
{
    public class SpatialPartition : IPoolableObject
    {
        public Vector2Int Index { get; private set; }

        public SpatialPartitioner SpatialPartitioner { get; }
        
        private readonly HashSet<SPCollider> _objects = new();
        public IReadOnlyCollection<SPCollider> Objects => _objects;
        
        private readonly HashSet<SPCollider> _toRemove = new();

        public int Count => _objects.Count;
        
        public SpatialPartition(Vector2Int index, SpatialPartitioner spatialPartitioner)
        {
            Init(index);
            SpatialPartitioner = spatialPartitioner;
        }

        public void Init(Vector2Int index)
        {
            Index = index;
        }

        public void Add(SPCollider toAdd)
        {
            _objects.Add(toAdd);
        }
        
        public void Remove(SPCollider toRemove)
        {
            _toRemove.Add(toRemove);
        }

        public void OnPoolReturn()
        {
            _objects.Clear();
            Index = default;
        }
        
        public Vector2 Position => new(Index.x * SpatialPartitioner.PartitionWidth, Index.y *  SpatialPartitioner.PartitionHeight);

        public IEnumerable<SpatialPartition> GetNeighbours(int distance = 1) => 
            SpatialPartitioner.GetPartitionNeighbourhood(Index, distance);

        public void Clear()
        {
            foreach (var collider in _toRemove)
            {
                _objects.Remove(collider);
            }
            
            _toRemove.Clear();
        }
    }
    
    public class SpatialPartitioner
    {
        private readonly SPPartitioner _config;
        public float PartitionWidth => _config.PartitionWidth;
        public float PartitionHeight => _config.PartitionHeight;

        private readonly Dictionary<Vector2Int, SpatialPartition> _partitions = new();
        public IReadOnlyDictionary<Vector2Int, SpatialPartition> Partitions => _partitions;

        private readonly BaseObjectsPool<SpatialPartition> _partitionsPool = new();

        public SpatialPartitioner(SPPartitioner config)
        {
            _config = config;
        }

        public Vector2Int GetPartitionIndex(Vector2 position)
        {
            var xPos = position.x;
            var yPos = position.y;
            
            var x = Mathf.FloorToInt(xPos / PartitionWidth);
            var y = Mathf.FloorToInt(yPos / PartitionHeight);
            
            return new Vector2Int(x, y);
        }

        public SpatialPartition GetPartition(Vector2 pos) => GetPartition(GetPartitionIndex(pos));

        public SpatialPartition GetPartition(Vector2Int index)
        {
            if (_partitions.TryGetValue(index, out var partition)) return partition;
            if (_partitionsPool.TryRetrieve(out partition))
            {
                partition.Init(index);
                return partition;
            }
            
            partition = new SpatialPartition(index, this);
            _partitions.Add(index, partition);

            return partition;
        }
        
        public void Empty(SpatialPartition partition)
        {
            _partitions.Remove(partition.Index);
            _partitionsPool.Return(partition);
        }

        public void Update()
        {
            foreach (var (_, partition) in _partitions)
            {
                foreach (var collider in partition.Objects)
                {
                    var thisRadius = collider.Radius;
                    var thisPosition = collider.Position;

                    var index = partition.Index;

                    const int distance = 1;
                    
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
                            if (_partitions.TryGetValue(new Vector2Int(x, y), out var neighbour))
                            {
                                foreach (var other in neighbour.Objects)
                                {
                                    if (other == collider) continue;
                                    if (other == null) continue;
                            
                                    var otherRadius = other.Radius;
                                    var otherPosition = other.Position;

                                    if (Vector2.Distance(thisPosition, otherPosition) < thisRadius + otherRadius)
                                    {
                                        collider.OnCollision(other);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var toRemove = new List<SpatialPartition>();
            
            foreach (var (_, partition) in _partitions)
            {
                partition.Clear();

                if (partition.Count == 0)
                {
                    toRemove.Add(partition);
                }
            }
            
            foreach (var spatialPartition in toRemove)
            {
                Empty(spatialPartition);
            }
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
    }
}