using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pickups
{
    [Serializable]
    public class InterfaceField<T> where T : class
    {
        [SerializeField] private Object obj;
        public T Value => obj as T;
    }
} 