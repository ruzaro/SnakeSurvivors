using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeSurvivors
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet bullet;
        [SerializeField] private float shootingCooldown;

        private void Start()
        {
            StartCoroutine(BulletSpawnCo());
        }

        private IEnumerator BulletSpawnCo()
        {
            while (gameObject != null)
            {
                yield return new WaitForSeconds(shootingCooldown);

                var dir = Random.Range(0.0f, 360.0f);
                
                var toShoot = Instantiate(bullet);
                toShoot.transform.Rotate(Vector3.forward, dir);
                toShoot.transform.position = transform.position;
            }
        }
    }
}