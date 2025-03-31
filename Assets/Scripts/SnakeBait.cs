using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeSurvivors
{
    public class SnakeBait : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private InputAction _moveAction;
        private Transform _transform;
        
        private void Awake()
        {
            _transform = transform;
            _moveAction = InputSystem.actions.FindAction(Input.Player.Move);
        }
        
        private void FixedUpdate()
        {
            var moveValue = _moveAction.ReadValue<Vector2>();
            var moveValueX = moveValue.x;
            
            var targetUp = target.up;
            var targetRight = target.right;

            var targetPos = target.position + targetUp;

            if (moveValueX > 0)
            {
                targetPos = target.position + targetRight;
            }
            else if (moveValueX < 0)
            {
                targetPos = target.position - targetRight;
                moveValueX = -moveValueX;
            }
            
            var rotated = Vector3.RotateTowards(targetUp, targetPos, moveValueX * Mathf.PI, 0.0f);

            _transform.position = targetPos + rotated;
        }
    }
}