using System.Collections.Generic;
using UnityEngine;

namespace Pickups
{
    [CreateAssetMenu(fileName = "PickupsConfig", menuName = "ScriptableObjects/Pickups/Collection", order = 0)]
    public class PickupsConfig : ScriptableObject
    {
        [SerializeField] private List<PickupConfig> pickups = new();
        public List<PickupConfig> Pickups => pickups;
    }
}