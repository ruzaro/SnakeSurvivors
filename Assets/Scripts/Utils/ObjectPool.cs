using UnityEngine;

namespace SnakeSurvivors
{
    public abstract class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly UnityEngine.Pool.ObjectPool<T> _pool;
        
        public int CountActive => _pool.CountActive;
        public int CountInactive => _pool.CountInactive;
        public int CountAll => _pool.CountAll;

        public ObjectPool(int initialCapacity, int maxSize, bool collectionChecks = true)
        {
            _pool = new UnityEngine.Pool.ObjectPool<T>(
                CreateItem, OnGet, OnRelease, OnDestroy, 
                collectionChecks, initialCapacity, maxSize
                );
        }

        public void Clear() => _pool.Clear();
        
        public T Get() => _pool.Get();
        
        public void Release(T obj) => _pool.Release(obj);

        public abstract T CreateItem();
        public abstract void OnGet(T item);
        public abstract void OnRelease(T item);
        public abstract void OnDestroy(T item);
    }
}