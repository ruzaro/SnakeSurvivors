using System;
using UnityEngine;

namespace SnakeSurvivors
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 0.95f;
        [SerializeField] private Transform target;
        [SerializeField] private float pushForce = 0.4f;

        [SerializeField] private SPColliderTag enemyTag;
        
        private Transform _target;
        private Transform _transform;
        
        public event Action OnDeath;

        private void Awake()
        {
            _transform = transform;
            _target = target;
        }

        public Enemy SetTarget(Transform target)
        {
            _target = target;
            return this;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            var dest = _target.transform.position;

            var curr = _transform.position;

            var delta = dest - curr;

            var direction = delta.normalized;

            var movement = direction * speed * Time.fixedDeltaTime;
            
            _transform.position = curr + movement;
        }

        private void OnSPCollision(SPCollider other)
        {
            if (other.Tag == enemyTag)
            {
                // Push other enemies away
                Vector2 otherPos = other.transform.position;
                Vector2 thisPos = _transform.position;
            
                var dir = (otherPos - thisPos).normalized;
                other.transform.position += (dir * Time.fixedDeltaTime * pushForce).ToVec3();
            }
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
        }
    }
}