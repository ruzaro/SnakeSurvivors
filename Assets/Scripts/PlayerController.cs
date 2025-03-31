using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SnakeSurvivors
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float cameraSpeed = 1.0f;
        [SerializeField] [Min(0.0f)] private float speed = 1f;
        
        [SerializeField] private float collisionRadius = 0.2f;

        [SerializeField] private SnakeBait snakeBait;

        [SerializeField] private Transform center;
        
        [SerializeField] private SnakePart prefab;
        [SerializeField] private float partsDistance = 0.1f;

        private readonly SpatialPartitioner<Enemy> _spatialPartitioner = Singletons.EnemySpatialPartitioner;
        private readonly List<SnakePart> _snakeParts = new();
        private readonly CenterPosition _centerPosition = new();

        private void Awake()
        {
            
        }
        
        private void Start()
        {
            // StartCoroutine(SpawnCo());
        }

        private void Update()
        {
            center.position = Vector3.MoveTowards(center.position, _centerPosition.CentralPosition, speed * Time.deltaTime);
        }

        private IEnumerator SpawnCo()
        {
            yield return new WaitForSeconds(2.0f);
            
            if (_snakeParts.Count > 5) yield break;

            var lastPart = _snakeParts.LastOrDefault();

            var isEmpty = lastPart == null;

            var target = isEmpty ? snakeBait.transform : lastPart.transform;

            var backward = -target.up * partsDistance;

            var pos = target.position + backward;

            var snakePart = Instantiate(prefab, transform);
        
            snakePart.transform.position = pos;
            snakePart.transform.rotation = target.rotation;
            
            snakePart.Attach(target, _spatialPartitioner, collisionRadius, partsDistance);
            
            _snakeParts.Add(snakePart);

            var weight = 5.0f / _snakeParts.Count; // TODO better weight?
            
            _centerPosition.AddTarget(snakePart.transform, weight);

            if (isEmpty)
            {
                snakeBait.Attach(snakePart.transform, partsDistance);
            }

            StartCoroutine(SpawnCo());
        }
    }
}