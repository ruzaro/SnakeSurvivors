using System;
using Pickups;
using UnityEngine;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(DamageDestination))]
    public class SnakePart : MonoBehaviour
    {
        private DamageDestination _damageDestination;
        public DamageDestination DamageDestination => _damageDestination;

        private void Awake()
        {
            _damageDestination = GetComponent<DamageDestination>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy) &&
                enemy.TryGetComponent(out DamageSource damageSource) &&
                damageSource.CanAttack(_damageDestination))
            {
                damageSource.Attack(_damageDestination);
            }
        }
    }
}