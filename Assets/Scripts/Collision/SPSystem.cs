using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeSurvivors
{
    public class SPSystem : MonoBehaviour
    {
        [SerializeField] private List<SPPartitioner> partitioners = new();
        
        private readonly List<SpatialPartitioner> _spatialPartitioners = new();

        private void Awake()
        {
            foreach (var partitioner in partitioners)
            {
                var spatialPartitioner = new SpatialPartitioner(partitioner);
                _spatialPartitioners.Add(spatialPartitioner);
                partitioner.Attach(spatialPartitioner);
            }
        }

        private void FixedUpdate()
        {
            foreach (var spatialPartitioner in _spatialPartitioners)
            {
                spatialPartitioner.Update();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var spatialPartitioner in _spatialPartitioners)
            {
                foreach (var (_, spatialPartition) in spatialPartitioner.Partitions)
                {
                    var pos = spatialPartition.Position;
                    var leftTop = pos.ToVec3();
                    var leftBottom = (pos + new Vector2(0, spatialPartitioner.PartitionHeight)).ToVec3();
                    var rightTop = (pos + new Vector2(spatialPartitioner.PartitionWidth, 0)).ToVec3();
                    var rightBottom = (pos + new Vector2(spatialPartitioner.PartitionWidth, spatialPartitioner.PartitionHeight)).ToVec3();

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
        }
#endif
    }
}