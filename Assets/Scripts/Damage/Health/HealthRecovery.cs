using UnityEngine;

namespace SnakeSurvivors
{
    public class HealthRecovery : MonoBehaviour
    {
        [SerializeField] private float amount;
        [SerializeField] private float cooldown;
        [SerializeField] private HealthPool healthPool;

        private float _timer;

        private void Awake()
        {
            _timer = cooldown;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Recover();
                _timer = cooldown;
            }
        }

        private void Recover()
        {
            healthPool.RestoreHealth(amount);
        }
    }
}