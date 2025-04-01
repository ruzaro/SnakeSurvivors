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
        
        private readonly SpatialPartitioner<Enemy> _spatialPartitioner = Singletons.EnemySpatialPartitioner;
        private SpatialPartition<Enemy> _spatialPartition;

        private void Awake()
        {
            _spatialPartition = _spatialPartitioner.GetPartition(transform.position);
        }

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

            _spatialPartition = _spatialPartitioner.GetPartition(pos);
            
            foreach (var enemy in _spatialPartition)
            {
                var enemyPos = enemy.Position;

                if (Vector2.Distance(pos, enemyPos) < enemy.Size + size)
                {
                    Destroy(enemy.gameObject);
                    Destroy(gameObject);
                    break;
                }
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