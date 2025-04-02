using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeSurvivors
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;

        [SerializeField] private Transform poolParent;
        [SerializeField] private Transform enemiesParent;

        [SerializeField] private Transform target;

        [SerializeField] private int maxEnemies = 80;
        [SerializeField] private int enemiesBatch = 20;
        
        private EnemiesPool _pool;

        private class EnemiesPool : ObjectPool<Enemy>
        {
            private readonly Enemy _prefab;
            private readonly Transform _parent;
            
            public EnemiesPool(Enemy prefab, Transform parent, int initialCapacity, int maxSize, bool collectionChecks = true) : 
                base(initialCapacity, maxSize, collectionChecks)
            {
                _prefab = prefab;
                _parent = parent;
            }

            public override Enemy CreateItem()
            {
                var enemy = Instantiate(_prefab, _parent);
                enemy.gameObject.SetActive(false);

                if (enemy.TryGetComponent(out ActionIfTooFar actionIfTooFar))
                {
                    actionIfTooFar.Action.AddListener(go =>
                    {
                        //Release(enemy);
                    });
                }
                
                enemy.Health.OnDeath.AddListener(() =>
                {
                    Release(enemy);
                });
                
                return enemy;
            }

            public override void OnGet(Enemy enemy)
            {
                enemy.gameObject.SetActive(true);
                enemy.Health.ResetHealth();
            }

            public override void OnRelease(Enemy enemy)
            {
                enemy.gameObject.SetActive(false);
                enemy.transform.SetParent(_parent);
                
                if (enemy.TryGetComponent(out ActionIfTooFar actionIfTooFar))
                {
                    actionIfTooFar.SetTarget(null);
                }
                
                if (enemy.TryGetComponent(out StraightTargetFollower straightTargetFollower))
                {
                    straightTargetFollower.SetTarget(null);
                }
            }

            public override void OnDestroy(Enemy enemy)
            {
                
            }
        }

        private void Awake()
        {
            _pool = new(enemyPrefab, poolParent, maxEnemies, maxEnemies);
        }

        private void Start()
        {
            StartCoroutine(SpawnCo());
        }

        private const float SpawnRadiusDiff = 5.0f;
        private const float SpawnRadius = 20.0f;

        private IEnumerator SpawnCo()
        {
            yield return new WaitForSeconds(2.0f); // TODO timer instead of coroutine

            if (_pool.CountAll < maxEnemies || _pool.CountInactive > enemiesBatch)
            {
                for (var i = 0; i < enemiesBatch; ++i)
                {
                    var toMove = Random.insideUnitCircle * 1.1f;
                    var range = SpawnRadius + Random.Range(-SpawnRadiusDiff, SpawnRadiusDiff);
                    var angle = Random.Range(0, 2*Mathf.PI);

                    toMove.x = range * Mathf.Cos(angle);
                    toMove.y = range * Mathf.Sin(angle);
            
            
                    var pos = target.position;
                    pos.x += toMove.x;
                    pos.y += toMove.y;

                    var enemy = _pool.Get();
        
                    enemy.transform.SetParent(enemiesParent);
                    
                    enemy.transform.position = pos;

                    if (enemy.TryGetComponent(out StraightTargetFollower straightTargetFollower))
                    {
                        straightTargetFollower.SetTarget(target);
                    }

                    if (enemy.TryGetComponent(out ActionIfTooFar destroyIfTooFar))
                    {
                        destroyIfTooFar.SetTarget(target);
                    }
                    
                    enemy.gameObject.SetActive(true);
                }
            }
            

            StartCoroutine(SpawnCo());
            yield break;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (target == null) return;
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(target.position, SpawnRadius - SpawnRadiusDiff);
            Gizmos.DrawWireSphere(target.position, SpawnRadius + SpawnRadiusDiff);
        }
#endif
    }
}