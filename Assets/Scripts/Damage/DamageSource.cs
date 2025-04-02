using UnityEngine;

namespace SnakeSurvivors
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] private float damage;

        private Unit _unit;
        public Unit Unit => _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public bool CanAttack(DamageDestination damageDestination)
        {
            if (_unit == null) return true;
            if (damageDestination.Unit == null) return true;
            return _unit.CanAttack(damageDestination.Unit);
        }

        public void Attack(DamageDestination damageDestination)
        {
            damageDestination.ReceiveDamage(damage, damageType);
        }
    }
}