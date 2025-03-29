using System;
using UnityEngine;

namespace SnakeSurvivors
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 0.95f;
        [SerializeField] private Transform target;
        
        private Transform _target;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _target = target;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target == null) return;

            var dest = _target.transform.position;

            var curr = _transform.position;

            var delta = dest - curr;

            var direction = delta.normalized;

            var movement = direction * speed * Time.deltaTime;

            _transform.position = curr + movement;
        }
    }
}