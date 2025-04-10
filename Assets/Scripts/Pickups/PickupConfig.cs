using UnityEngine;

namespace Pickups
{
    [CreateAssetMenu(fileName = "PickupConfig", menuName = "ScriptableObjects/Pickups/Single", order = 0)]
    public class PickupConfig : ScriptableObject
    {
        [SerializeField] private InterfaceField<IPickup> prefab;
        public IPickup Prefab => prefab.Value;
        
        [SerializeField][Range(0.0f, 1.0f)] private float dropRate;
        public float DropRate => dropRate;
    }
}