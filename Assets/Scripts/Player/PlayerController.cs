using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeSurvivors
{
    [RequireComponent(typeof(HealthPool))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float cameraSpeed = 1.0f;
        [SerializeField] [Min(0.0f)] private float speed = 1f;
        [SerializeField] private float damageCooldown = 0.5f;

        [SerializeField] private SnakeBait snakeBait;

        [SerializeField] private Transform center;
        
        [SerializeField] private SnakePart prefab;
        
        private readonly List<SnakePart> _snakeParts = new();
        private readonly HashSet<SmoothTargetFollower> _snakePartsMovement = new();
        private readonly CenterPosition _centerPosition = new();
        private HealthPool _health;
        private float _damageCooldownTimer;

        private void Awake()
        {
            _health = GetComponent<HealthPool>();
            
            _health.OnDeath.AddListener(() =>
            {
                Debug.LogError("Player Death");
            });
        }

        private void Start()
        {
            StartCoroutine(SpawnCo());
        }

        private void Update()
        {
            Vector3 targetPosition = _centerPosition.CentralPosition;
            targetPosition.z = center.position.z;
            center.position = Vector3.MoveTowards(center.position, targetPosition, speed * Time.deltaTime);
            
            _damageCooldownTimer -= Time.deltaTime;
            if (_damageCooldownTimer <= 0.0f)
            {
                _damageCooldownTimer = 0.0f;
            }
            
            foreach (var targetFollower in _snakePartsMovement)
            {
                targetFollower.OnUpdate(Time.deltaTime);
            }
        }

        private IEnumerator SpawnCo()
        {
            yield return new WaitForSeconds(2.0f);
            
            if (_snakeParts.Count > 1) yield break;

            var lastPart = _snakeParts.LastOrDefault();

            var isEmpty = lastPart == null;

            var target = isEmpty ? snakeBait.transform : lastPart.transform;

            var snakePart = Instantiate(prefab, transform);

            var distance = 0.0f;

            if (snakePart.TryGetComponent(out SmoothTargetFollower targetFollower))
            {
                distance = targetFollower.Distance;
                targetFollower.Attach(target);
                _snakePartsMovement.Add(targetFollower);
            }

            var backward = -target.up * distance;
            var pos = target.position + backward;
        
            snakePart.transform.position = pos;
            snakePart.transform.rotation = target.rotation;
            
            snakePart.DamageDestination.OnDamageReceived.AddListener(damage =>
            {
                if (_damageCooldownTimer > 0.0f) return;
                _health.ReduceHealth(damage);
                _damageCooldownTimer = damageCooldown;
            });
            
            _snakeParts.Add(snakePart);

            var weight = 5.0f / _snakeParts.Count; // TODO better weight?
            
            _centerPosition.AddTarget(snakePart.transform, weight);

            if (isEmpty)
            {
                snakeBait.Attach(snakePart.transform, distance);
            }

            StartCoroutine(SpawnCo());
        }
    }
}