using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SnakeSurvivors
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float speed = 1f;

        [SerializeField] private UnityEvent<Vector2> onMove;
        
        private InputAction _moveAction;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _moveAction = InputSystem.actions.FindAction(Input.Player.Move);
        }

        private void Update()
        {
            var moveValue = _moveAction.ReadValue<Vector2>() * speed * Time.deltaTime;

            onMove?.Invoke(moveValue);
            
            var pos = _transform.position;
            pos += moveValue.ToVec3();
            _transform.position = pos;
        }
    }
}