using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(DamageSource))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SPColliderTag enemyTag;

        [SerializeField] private UnityEvent<Bullet> onHit;
        public UnityEvent<Bullet> OnHit => onHit;

        private DamageSource _damageSource;

        private void Awake()
        {
            _damageSource = GetComponent<DamageSource>();
        }

        public void OnSPCollision(SPCollider other)
        {
            if (other.Tag == enemyTag && 
                other.TryGetComponent(out DamageDestination damageDestination) &&
                _damageSource.CanAttack(damageDestination))
            {
                _damageSource.Attack(damageDestination);
                onHit?.Invoke(this);
            }
        }
    }
}