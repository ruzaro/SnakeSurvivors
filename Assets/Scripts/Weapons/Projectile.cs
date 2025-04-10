using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(DamageSource))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Projectile> onHit;
        public UnityEvent<Projectile> OnHit => onHit;

        private DamageSource _damageSource;

        private void Awake()
        {
            _damageSource = GetComponent<DamageSource>();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy) &&
                enemy.TryGetComponent(out DamageDestination damageDestination) &&
                _damageSource.CanAttack(damageDestination))
            {
                _damageSource.Attack(damageDestination);
                onHit?.Invoke(this);
            }
        }
    }
}