using UnityEngine;

namespace Pickups
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PickupMagnet : MonoBehaviour
    {
        [SerializeField] private float strength = 1.0f;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IPickup pickup) &&
                other.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                var otherPos = other.transform.position;
                var pos = transform.position;
                
                var dir = (pos - otherPos).normalized;
                
                rb.AddForce(dir * strength);
            }
        }
    }
}