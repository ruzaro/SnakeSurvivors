using UnityEngine;

namespace Pickups
{
    public class PickupCollector : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IPickup pickup))
            {
                pickup.Collect(this);
            }
        }
    }
}