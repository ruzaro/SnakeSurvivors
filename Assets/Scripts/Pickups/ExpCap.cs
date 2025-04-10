using UnityEngine;

namespace Pickups
{
    [CreateAssetMenu(fileName = "ExpCap", menuName = "ScriptableObjects/Exp/Cap", order = 0)]
    public class ExpCap : ScriptableObject
    {
        [SerializeField] private uint start;
        [SerializeField] private uint end;
        [SerializeField] private uint cap;
    }
}