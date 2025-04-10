using UnityEngine;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(HealthPool))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float pushForce = 0.4f;
        
        private Transform _transform;
        private HealthPool _health;
        public HealthPool Health => _health;

        private void Awake()
        {
            _transform = transform;
            _health = GetComponent<HealthPool>();
        }

        public void Die()
        {
            // TODO temp solution
            Destroy(gameObject);
        }
    }
}