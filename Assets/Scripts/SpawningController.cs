using System.Collections;
using UnityEngine;

namespace SnakeSurvivors
{
    public class SpawningController : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        [SerializeField] private Transform target;

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
            
                enemy.GetComponent<Enemy>().SetTarget(target);
            }

            StartCoroutine(SpawnCo());
        }
    }
}