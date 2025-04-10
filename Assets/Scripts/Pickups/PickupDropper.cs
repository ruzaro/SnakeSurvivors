using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups
{
    public class PickupDropper : MonoBehaviour
    {
        [SerializeField] private PickupsConfig pickupsConfig;

        private void OnDestroy()
        {
            // TODO temp solution
            DropPickup();
        }

        public void DropPickup()
        {
            var rand = Random.Range(0, 1.0f);

            var pickups = new List<int>();

            for (var i = 0; i < pickupsConfig.Pickups.Count; ++i)
            {
                var pickup = pickupsConfig.Pickups[i];
                if (pickup.DropRate >= rand)
                {
                    pickups.Add(i);
                }
            }
            
            if (pickups.Count <= 0) return;
            
            var randPickup = Random.Range(0, pickups.Count);
            
            var pickupIndex = pickups[randPickup];
            
            var prefab = (MonoBehaviour)pickupsConfig.Pickups[pickupIndex].Prefab;
            
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}