using System;
using UnityEngine;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(DamageDestination))]
    public class SnakePart : MonoBehaviour
    {
        [SerializeField] private SPColliderTag enemyTag;
        
        private DamageDestination _damageDestination;
        public DamageDestination DamageDestination => _damageDestination;

        private void Awake()
        {
            _damageDestination = GetComponent<DamageDestination>();
        }

        public void OnSPCollision(SPCollider other)
        {
            if (other.Tag == enemyTag && 
                other.TryGetComponent(out DamageSource damageSource) &&
                damageSource.CanAttack(_damageDestination))
            {
                damageSource.Attack(_damageDestination);
            }
            
            // TODO check collision with exp
        }
    }
}