using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class HealthPool : MonoBehaviour
    {
        [SerializeField] private float maxHealth;
        
        [SerializeField] private UnityEvent<float, float> onHealthChanged;
        public UnityEvent<float, float> OnHealthChanged => onHealthChanged;
        
        [SerializeField] private UnityEvent<float> onHealthChangedPercent;
        public UnityEvent<float> OnHealthChangedPercent => onHealthChangedPercent;
        
        [SerializeField] private UnityEvent onDeath;
        public UnityEvent OnDeath => onDeath;

        private float _health;

        private void Awake()
        {
            _health = maxHealth;
        }

        public void ReduceHealth(float amount)
        {
            if (_health <= 0.0f) return;
            
            _health -= amount;

            var died = false;

            if (_health <= 0)
            {
                _health = 0.0f;
                died = true;
            }
            
            NotifyHealthChanged();

            if (died)
            {
                onDeath?.Invoke();
            }
        }

        public void ResetHealth() => RestoreHealth(maxHealth);

        public void RestoreHealth(float amount)
        {
            _health += amount;
            
            if (_health > maxHealth)
            {
                _health = maxHealth;
            }
            
            NotifyHealthChanged();
        }

        private void NotifyHealthChanged()
        {
            onHealthChanged?.Invoke(_health, maxHealth);
            onHealthChangedPercent?.Invoke(_health / maxHealth);
        }
    }
}