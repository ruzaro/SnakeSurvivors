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
            _spatialPartition.Add(gameObject);
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
            
            _spatialPartition = _spatialPartition.UpdatePartition(gameObject);
            
            // TODO check collision with enemy
            
            // TODO check collision with exp
            
            
        }
    }
}