using System.Collections;
using UnityEngine;

namespace SnakeSurvivors
{
    public class AreaWeapon : Weapon
    {
        [SerializeField] private DamageArea areaPrefab;
        [SerializeField] private float shootingCooldown;

        private DamageArea _area;

        private void Awake()
        {
            _area = Instantiate(areaPrefab, transform);
            _area.gameObject.SetActive(false);
        }

        private void Start()
        {
            StartCoroutine(AreaSpawnCo());
        }

        private IEnumerator AreaSpawnCo()
        {
            while (gameObject != null)
            {
                yield return new WaitForSeconds(shootingCooldown);

                _area.gameObject.SetActive(true);
            }
        }
    }
}