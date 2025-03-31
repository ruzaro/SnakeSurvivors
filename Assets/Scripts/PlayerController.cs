using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SnakeSurvivors
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float speed = 1f;

        [SerializeField] private UnityEvent<Vector2> onMove;
        
        [SerializeField] private SpatialPartitioner spatialPartitioner;
        
        [SerializeField] private float collisionRadius = 0.2f;
        
        private InputAction _moveAction;
        private InputAction _sprintAction;
        private Transform _transform;
        private SpatialPartition _spatialPartition;

        private void Awake()
        {
            _transform = transform;
            _moveAction = InputSystem.actions.FindAction(Input.Player.Move);
            _sprintAction = InputSystem.actions.FindAction(Input.Player.Sprint);
            _spatialPartition = spatialPartitioner.GetPartition(_transform.position);
        }

        private void FixedUpdate()
        {
            var speed = this.speed;

            if (_sprintAction.IsPressed())
            {
                speed *= 2.0f;
            }
            
            var moveValue = _moveAction.ReadValue<Vector2>() * speed * Time.deltaTime;

            onMove?.Invoke(moveValue);
            
            var pos = _transform.position;
            pos += moveValue.ToVec3();
            _transform.position = pos;
            
            _spatialPartition = spatialPartitioner.GetPartition(_transform.position);
            
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

                    if (Vector3.Distance(enemyPos, _transform.position) < collisionRadius + enemy.CollisionRadius)
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
    }
}