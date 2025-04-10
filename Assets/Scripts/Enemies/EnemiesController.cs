using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int batchesCount = 20;
        
        private EnemiesPool _pool;

        private readonly SortedSet<EnemiesBatch> _batches = new();
        private int _currentBatch;

        private class EnemiesBatch : IComparable<EnemiesBatch>, IEnumerable<StraightTargetFollower>
        {
            private readonly int _id;
            private readonly HashSet<StraightTargetFollower> _followers = new();
            
            public int Id => _id;

            public EnemiesBatch(int id)
            {
                _id = id;
            }
            
            public void Add(StraightTargetFollower follower) => _followers.Add(follower);

            public void Remove(StraightTargetFollower follower) => _followers.Remove(follower);

            public bool Contains(StraightTargetFollower follower) => _followers.Contains(follower);

            public int CompareTo(EnemiesBatch other)
            {
                var ret = _followers.Count.CompareTo(other._followers.Count);
                if (ret == 0)
                {
                    ret = _id.CompareTo(other._id);
                }
                return ret;
            }

            public IEnumerator<StraightTargetFollower> GetEnumerator() => _followers.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
            
        private class EnemiesPool : ObjectPool<Enemy>
        {
            private readonly EnemiesController _controller;
            private readonly Enemy _prefab;
            private readonly Transform _parent;
            
            public EnemiesPool(EnemiesController controller, Enemy prefab, Transform parent, int initialCapacity, int maxSize, bool collectionChecks = true) : 
                base(initialCapacity, maxSize, collectionChecks)
            {
                _controller = controller;
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
                
                // if (enemy.TryGetComponent(out StraightTargetFollower straightTargetFollower))
                // {
                //     var batch = _controller._batches.Min;
                //
                //     _controller._batches.Remove(batch);
                //
                //     batch.Add(straightTargetFollower);
                //
                //     _controller._batches.Add(batch);
                // }
            }

            public override void OnRelease(Enemy enemy)
            {
                enemy.gameObject.SetActive(false);
                enemy.transform.SetParent(_parent);
                
                if (enemy.TryGetComponent(out ActionIfTooFar actionIfTooFar))
                {
                    actionIfTooFar.SetTarget(null);
                }
                
                if (enemy.TryGetComponent(out EnemyMovement enemyMovement))
                {
                    enemyMovement.SetTarget(null);
                }
                
                // if (enemy.TryGetComponent(out StraightTargetFollower straightTargetFollower))
                // {
                //     straightTargetFollower.SetTarget(null);
                //     
                //     var batch = _controller._batches.First(x => x.Contains(straightTargetFollower));
                //
                //     _controller._batches.Remove(batch);
                //
                //     batch.Remove(straightTargetFollower);
                //
                //     _controller._batches.Add(batch);
                // }
            }

            public override void OnDestroy(Enemy enemy)
            {
                
            }
        }

        private void Awake()
        {
            _pool = new(this, enemyPrefab, poolParent, maxEnemies, maxEnemies);

            for (var i = 0; i < batchesCount; ++i)
            {
                _batches.Add(new EnemiesBatch(i));
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnCo());
        }

        private void Update()
        {
            var currentBatch = _batches.First(x => x.Id == _currentBatch);
            
            foreach (var targetFollower in currentBatch)
            {
                targetFollower.OnUpdate(Time.deltaTime * batchesCount);
            }

            _currentBatch = (_currentBatch + 1)%batchesCount;
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

                    // if (enemy.TryGetComponent(out StraightTargetFollower straightTargetFollower))
                    // {
                    //     straightTargetFollower.SetTarget(target);
                    // }
                    
                    if (enemy.TryGetComponent(out EnemyMovement enemyMovement))
                    {
                        enemyMovement.SetTarget(target);
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