using System;
using UnityEngine;

namespace SnakeSurvivors
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 0.95f;
        [SerializeField] private float avoidForce = 0.4f;
        [SerializeField] private Transform target;
        
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void FixedUpdate()
        {
            Vector2 toTarget = (target.position - transform.position).normalized;
            Vector2 avoid = AvoidOthers();
            Vector2 finalDir = (toTarget + avoid).normalized;
            
            _rb.MovePosition(_rb.position + finalDir * speed * Time.fixedDeltaTime);
            
            // Vector2 direction = (target.position - transform.position).normalized;
            // Vector2 newPosition = _rb.position + direction * speed * Time.fixedDeltaTime;
            //
            // _rb.MovePosition(newPosition);
        }

        private Vector2 AvoidOthers()
        {
            Vector2 avoidDir = Vector2.zero;
            float avoidRadius = avoidForce;

            Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, avoidRadius);
            foreach (var neighbor in neighbors)
            {
                if (neighbor.gameObject != gameObject && neighbor.IsEnemy())
                {
                    Vector2 diff = (Vector2)(transform.position - neighbor.transform.position);
                    float dist = diff.magnitude;
                    if (dist > 0)
                        avoidDir += diff.normalized / dist;
                }
            }

            return avoidDir;
        }
    }
}