using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeSurvivors
{
    [CreateAssetMenu(fileName = "ArmorType", menuName = "ScriptableObjects/Damage/ArmorType", order = 0)]
    public class ArmorType : ScriptableObject
    {
        [Serializable]
        private class Modifier
        {
            [SerializeField] private DamageType damageType;
            public DamageType DamageType => damageType;
            
            [SerializeField] private float value;
            public float Value => value;
        }

        [SerializeField] private List<Modifier> modifiers = new();
        
        public float Against(DamageType damageType)
        {
            var modifier = modifiers.FirstOrDefault(x => x.DamageType == damageType);
            return modifier?.Value ?? 1.0f;
        }
    }
}