using System;
using System.Collections;
using UnityEngine;

namespace SnakeSurvivors
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float size = 0.2f;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float destructionTimer = 5.0f;

        [SerializeField] private SPColliderTag enemyTag;

        private void Start()
        {
            StartCoroutine(DestroyAfterTime());
        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(destructionTimer);
            
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            var dir = transform.up;
            
            var pos = transform.position;
            pos += dir * speed * Time.fixedDeltaTime;
            transform.position = pos;
        }

        private void OnSPCollision(SPCollider other)
        {
            if (other.Tag == enemyTag)
            {
                // TODO destruction while colliding
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, size);
        }
#endif
    }
}