using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeSurvivors
{
    public class SnakeBait : MonoBehaviour
    {
        [SerializeField] private float debugVal;

        private Transform _target;
        private InputAction _moveAction;
        private Transform _transform;

        private float _distance;
        
        private void Awake()
        {
            _transform = transform;
            _moveAction = InputSystem.actions.FindAction(Const.Input.Player.Move);
        }

        public void Attach(Transform target, float distance)
        {
            _target = target;
            _distance = distance;
        }
        
        private void FixedUpdate()
        {
            if (_target == null) return;
            
            var moveValue = _moveAction.ReadValue<Vector2>();
            var moveValueX = moveValue.x;
            
            var targetUp = _target.up;
            var targetPos = _target.position;

            var angle = -90.0f * moveValueX;

            var direction = Quaternion.AngleAxis(angle, Vector3.forward) * targetUp;

            _transform.position = targetPos + direction * _distance * 1.1f;
        }

        private void OnDrawGizmos()
        {
            if (_target == null) return;
            
            var targetPos = _target.position;
            var targetUp = _target.up;

            var moveValueX = debugVal;

            var angle = -90.0f * moveValueX;

            var test = Quaternion.AngleAxis(angle, Vector3.forward) * targetUp;
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(targetPos, targetPos + test);
        }
    }
}