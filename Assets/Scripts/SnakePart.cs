using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeSurvivors
{
    public class SnakePart : MonoBehaviour
    {
        private Transform _transform;
        
        [SerializeField] [Min(0.0f)] private float speed = 1f;

        [SerializeField] [Min(0.0f)] private float rotationSpeed = 1.0f;

        private Transform _followTarget;

        private SpatialPartitioner _spatialPartitioner;
        private SpatialPartition _spatialPartition;

        private float _collisionRadius;
        private float _distance;
        
        private void Awake()
        {
            _transform = transform;
        }

        public void Attach(Transform followTarget, SpatialPartitioner spatialPartitioner, float collisionRadius, float distance)
        {
            _followTarget = followTarget;
            _spatialPartitioner = spatialPartitioner;
            _spatialPartition = spatialPartitioner.GetPartition(_transform.position);
            _collisionRadius = collisionRadius;
            _distance = distance;
        }

        private void FixedUpdate()
        {
            var dir = _transform.up;

            var targetPos = _followTarget.position;
            targetPos.z = _transform.position.z; // TODO fix this

            var targetRot = (targetPos - _transform.position).normalized;
            
            var rotated = Vector3.RotateTowards(dir, targetRot, rotationSpeed * Time.fixedDeltaTime, 0.0f);

            _transform.rotation = Quaternion.LookRotation(Vector3.forward, rotated);

            dir = _transform.up;
            
            var pos = _transform.position;
            if (Vector2.Distance(targetPos, pos) < _distance) return;
            
            var dif = dir * speed * Time.fixedDeltaTime;
            
            pos += dif;
            _transform.position = pos;
            
            _spatialPartition = _spatialPartitioner.GetPartition(_transform.position);
            
            CollideWithEnemies();
            
            // TODO check collision with exp
        }

        private void CollideWithEnemies()
        {
            // TODO move this logic to enemies?
            foreach (var partition in _spatialPartition.GetNeighbours())
            {
                var destroyed = new HashSet<GameObject>();
                
                foreach (var go in partition)
                {
                    if (!go.TryGetComponent(out Enemy enemy)) continue;

                    var enemyPos = go.transform.position;

                    if (Vector3.Distance(enemyPos, _transform.position) < _collisionRadius + enemy.CollisionRadius)
                    {
                        destroyed.Add(go);
                    }
                }
                
                // TODO temporary destruction of enemies
                foreach (var go in destroyed)
                {
                    partition.Remove(go);
                    Destroy(go);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_followTarget == null) return;
            
            var pos = transform.position;
            
            var dir = transform.up;
        
            var dirRight = transform.right;
            
            var targetPos = _followTarget.position;
            targetPos.z = transform.position.z;
        
            var targetRot = (targetPos - transform.position).normalized;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + dir);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + dirRight);
            
            var rotated = Vector3.RotateTowards(dir, targetRot, rotationSpeed, 0.0f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, pos + rotated);
        }
    }
}