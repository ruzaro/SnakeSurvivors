using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeSurvivors
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float shootingCooldown;

        [SerializeField] private int maxBullets;
        [SerializeField] private Transform bulletsPoolParent;
        [SerializeField] private Transform bulletsActiveParent;

        private BulletPool _pool;
        
        private readonly HashSet<StraightPathFollower> _followers = new();

        private void Awake()
        {
            _pool = new(this, bulletPrefab, bulletsPoolParent, maxBullets, maxBullets);
        }

        private class BulletPool : ObjectPool<Bullet>
        {
            private readonly BulletSpawner _spawner;
            private readonly Bullet _prefab;
            private readonly Transform _parent;
            
            public BulletPool(BulletSpawner spawner, Bullet prefab, Transform parent, int initialCapacity, int maxSize, bool collectionChecks = true) : base(initialCapacity, maxSize, collectionChecks)
            {
                _spawner = spawner;
                _prefab = prefab;
                _parent = parent;
            }

            public override Bullet CreateItem()
            {
                // TODO bullets parent
                var bullet = Instantiate(_prefab/*, _parent, true*/);
                bullet.gameObject.SetActive(false);

                if (bullet.TryGetComponent(out ActionAfterSeconds actionAfterSeconds))
                {
                    actionAfterSeconds.Action.AddListener(go =>
                    {
                        Release(bullet);
                    });
                }
                
                bullet.OnHit.AddListener(_ =>
                {
                    Release(bullet);
                });

                return bullet;
            }

            public override void OnGet(Bullet bullet)
            {
                bullet.gameObject.SetActive(true);
                if (bullet.TryGetComponent(out StraightPathFollower pathFollower))
                {
                    _spawner._followers.Add(pathFollower);   
                }
            }

            public override void OnRelease(Bullet bullet)
            {
                bullet.gameObject.SetActive(false);
                bullet.transform.SetParent(_parent);
                if (bullet.TryGetComponent(out StraightPathFollower pathFollower))
                {
                    _spawner._followers.Remove(pathFollower);   
                }
            }

            public override void OnDestroy(Bullet bullet)
            {
                
            }
        }

        private void Start()
        {
            StartCoroutine(BulletSpawnCo());
        }

        private void Update()
        {
            foreach (var pathFollower in _followers)
            {
                pathFollower.OnUpdate(Time.deltaTime);
            }
        }

        private IEnumerator BulletSpawnCo()
        {
            while (gameObject != null)
            {
                yield return new WaitForSeconds(shootingCooldown);

                var dir = Random.Range(0.0f, 360.0f);
                
                var bullet = _pool.Get();
                
                // TODO bullets parent
                //bullet.transform.SetParent(bulletsActiveParent);
                
                bullet.transform.Rotate(Vector3.forward, dir);
                bullet.transform.position = transform.position;
            }
        }
    }
}