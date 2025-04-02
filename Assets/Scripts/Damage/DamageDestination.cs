using System;
using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class DamageDestination : MonoBehaviour
    {
        [SerializeField] private ArmorType armorType;
        [SerializeField] private float armor;
        [SerializeField] private UnityEvent<float> onDamageReceived;
        public UnityEvent<float> OnDamageReceived => onDamageReceived;
        
        private Unit _unit;
        public Unit Unit => _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public void ReceiveDamage(float damage, DamageType damageType)
        {
            var typeModifier = armorType != null ? armorType.Against(damageType) : 1.0f;
            var armorModifier = ArmorModifier;

            damage *= typeModifier;
            damage *= armorModifier;
            
            onDamageReceived?.Invoke(damage);
        }

        private float ArmorModifier
        {
            get
            {
                if (armor >= 0.0f)
                {
                    const float positiveMod = 0.06f;
                    return 1.0f / (1.0f + armor * positiveMod);
                }

                const float negativeBase = 0.94f;
                return 2 - Mathf.Pow(negativeBase, -armor);
            }
        }
    }
}