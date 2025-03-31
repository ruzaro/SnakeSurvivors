using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeSurvivors
{
    public class SnakePart : MonoBehaviour
    {
        private Transform _transform;
        
        [SerializeField] [Min(0.0f)] private float speed = 1f;

        [SerializeField] [Min(0.0f)] private float rotationSpeed = 1.0f;

        [SerializeField] private float distance = 0.1f;

        [SerializeField] private Transform target;
        
        private void Awake()
        {
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var dir = _transform.up;

            var targetPos = target.position;
            targetPos.z = _transform.position.z; // TODO fix this

            var targetRot = (targetPos - _transform.position).normalized;
            
            var rotated = Vector3.RotateTowards(dir, targetRot, rotationSpeed * Time.deltaTime, 0.0f);

            _transform.rotation = Quaternion.LookRotation(Vector3.forward, rotated);

            dir = _transform.up;
            
            var pos = _transform.position;
            if (Vector2.Distance(targetPos, pos) < distance) return;
            
            var dif = dir * speed * Time.deltaTime;
            
            pos += dif;
            _transform.position = pos;
        }

        private void OnDrawGizmos()
        {
            var pos = transform.position;
            
            var dir = transform.up;

            var dirRight = transform.right;
            
            var targetPos = target.position;
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