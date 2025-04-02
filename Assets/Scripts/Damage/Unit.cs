using UnityEngine;

namespace SnakeSurvivors
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Team team;
        public Team Team => team;

        public bool CanAttack(Unit target) => team.CanAttack(target.team);
    }
}