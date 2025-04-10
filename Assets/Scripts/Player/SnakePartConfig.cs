using UnityEngine;

namespace SnakeSurvivors
{
    [CreateAssetMenu(fileName = "SnakePart", menuName = "ScriptableObjects/Snake/Part", order = 0)]
    public class SnakePartConfig : ScriptableObject
    {
        [SerializeField] private Weapon weaponPrefab;
    }
}