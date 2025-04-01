using UnityEngine;

namespace SnakeSurvivors
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] private float damage;
    }
}