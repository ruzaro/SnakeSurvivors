using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeSurvivors
{
    [Serializable]
    [CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/Team/Team", order = 0)]
    public class Team : ScriptableObject
    {
        [SerializeField] private string id;

        [SerializeField] private List<Team> enemies = new();
        
        public bool CanAttack(Team other) => enemies.Contains(other);
    }
}