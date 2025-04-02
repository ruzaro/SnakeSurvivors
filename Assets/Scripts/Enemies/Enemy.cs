using UnityEngine;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(HealthPool))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float pushForce = 0.4f;

        [SerializeField] private SPColliderTag enemyTag;
        
        private Transform _transform;
        private HealthPool _health;
        public HealthPool Health => _health;

        private void Awake()
        {
            _transform = transform;
            _health = GetComponent<HealthPool>();
        }

        public void OnSPCollision(SPCollider other)
        {
            if (other.Tag == enemyTag)
            {
                // Push other enemies away
                Vector2 otherPos = other.transform.position;
                Vector2 thisPos = _transform.position;
            
                var dir = (otherPos - thisPos).normalized;
                other.transform.position += (dir * pushForce * Time.fixedDeltaTime).ToVec3();
            }
        }
    }
}