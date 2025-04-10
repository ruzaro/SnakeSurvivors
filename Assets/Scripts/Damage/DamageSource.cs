using UnityEngine;

namespace SnakeSurvivors
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] private float damage;
        [SerializeField] private Unit unit;
        public Unit Unit => unit;

        public bool CanAttack(DamageDestination damageDestination)
        {
            if (unit == null) return true;
            if (damageDestination.Unit == null) return true;
            return unit.CanAttack(damageDestination.Unit);
        }

        public void Attack(DamageDestination damageDestination)
        {
            damageDestination.ReceiveDamage(damage, damageType);
        }
    }
}