using System.Collections;
using UnityEngine;

namespace SnakeSurvivors
{
    public class SpawningController : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        [SerializeField] private Transform target;
        
        private readonly SpatialPartitioner<Enemy> _spatialPartitioner = Singletons.EnemySpatialPartitioner;

        private void Start()
        {
            StartCoroutine(SpawnCo());
        }

        private IEnumerator SpawnCo()
        {
            yield return new WaitForSeconds(2.0f);

            const float val = 5.0f;
            const float b = 10.0f;
            const int c = 20;

            for (var i = 0; i < c; ++i)
            {
                var toMove = Random.insideUnitCircle * 1.1f;
                var range = b + Random.Range(-val, val);
                var angle = Random.Range(0, 2*Mathf.PI);

                toMove.x = range * Mathf.Cos(angle);
                toMove.y = range * Mathf.Sin(angle);
                
                
                var pos = target.position;
                pos.x += toMove.x;
                pos.y += toMove.y;

                var enemy = Instantiate(prefab, transform);
            
                enemy.transform.position = pos;
            
                enemy.GetComponent<Enemy>().SetTarget(target).AddSpatialPartitioner(_spatialPartitioner);
            }

            StartCoroutine(SpawnCo());
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var spatialPartition in _spatialPartitioner)
            {
                var pos = spatialPartition.Position;
                var leftTop = pos.ToVec3();
                var leftBottom = (pos + new Vector2(0, _spatialPartitioner.PartitionHeight)).ToVec3();
                var rightTop = (pos + new Vector2(_spatialPartitioner.PartitionWidth, 0)).ToVec3();
                var rightBottom = (pos + new Vector2(_spatialPartitioner.PartitionWidth, _spatialPartitioner.PartitionHeight)).ToVec3();

                if (spatialPartition.Count > 0)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                
                Gizmos.DrawLine(leftTop, rightTop);
                Gizmos.DrawLine(leftTop, leftBottom);
                Gizmos.DrawLine(rightBottom, rightTop);
                Gizmos.DrawLine(rightBottom, leftBottom);
            }
        }
#endif
    }
}