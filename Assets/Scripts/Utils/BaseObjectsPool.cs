using System.Collections.Generic;
using System.Linq;

namespace SnakeSurvivors
{
    public interface IPoolableObject
    {
        void OnPoolReturn();
    }
    
    public class BaseObjectsPool<T> where T : IPoolableObject
    {
        private readonly List<T> _objects = new();
        private int _lastIndex;
        
        public bool TryRetrieve(out T obj)
        {
            if (_lastIndex == 0)
            {
                obj = default;
                return false;
            }

            if (_lastIndex == _objects.Count)
            {
                obj = _objects.Last();
            }
            else
            {
                obj = _objects[_lastIndex];
            }
            
            if (obj == null) return false;
            
            --_lastIndex;
            return true;
        }

        public void Return(T obj)
        {
            if (obj == null) return;
            
            if (_lastIndex < _objects.Count)
            {
                _objects[_lastIndex] = obj;
            }
            else
            {
                _objects.Add(obj);
            }
            ++_lastIndex;
            obj.OnPoolReturn();
        }
    }
}