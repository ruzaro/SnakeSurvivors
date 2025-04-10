using System.Collections.Generic;
using UnityEngine;

namespace Pickups
{
    [CreateAssetMenu(fileName = "ExperienceCaps", menuName = "ScriptableObjects/Exp/Caps", order = 0)]
    public class ExpCaps : ScriptableObject
    {
        [SerializeField] private List<ExpCap> caps = new();
    }
}