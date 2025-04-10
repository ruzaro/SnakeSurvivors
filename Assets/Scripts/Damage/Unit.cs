using UnityEngine;

namespace SnakeSurvivors
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Team/Unit", order = 0)]
    public class Unit : ScriptableObject
    {
        [SerializeField] private Team team;
        public Team Team => team;

        public bool CanAttack(Unit target) => team.CanAttack(target.team);
    }
}