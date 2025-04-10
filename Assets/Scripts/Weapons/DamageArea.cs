using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(DamageSource))]
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private float damageTimer = 0.1f;
        [SerializeField] private float radius = 2.0f;
        
        [SerializeField] private UnityEvent<DamageArea> onHit;
        public UnityEvent<DamageArea> OnHit => onHit;

        private DamageSource _damageSource;
        private Coroutine _damageCoroutine;

        private void Awake()
        {
            _damageSource = GetComponent<DamageSource>();
        }

        private void OnEnable()
        {
            _damageCoroutine ??= StartCoroutine(DamageCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }

        private IEnumerator DamageCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(damageTimer);
                
                Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, radius);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.gameObject != gameObject && neighbor.IsEnemy() &&
                        neighbor.TryGetComponent(out DamageDestination damageDestination) &&
                        _damageSource.CanAttack(damageDestination))
                    {
                        _damageSource.Attack(damageDestination);
                        onHit?.Invoke(this);
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}