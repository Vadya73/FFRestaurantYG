using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private Queue<T> _poolObjects;
        private List<T> _allPoolObjects;
        
        private T _prefab;
        private int _initialSize;
        private Transform _parent = null;

        public Queue<T> PoolObjects => _poolObjects;
        public List<T> AllPoolObjects => _allPoolObjects;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _initialSize = initialSize;
            _parent = parent;
        }

        public void Initialize()
        {
            _poolObjects = new Queue<T>(_initialSize);
            _allPoolObjects = new List<T>(_initialSize);
            
            for (int i = 0; i < _initialSize; i++)
            {
                var instObject = CreateObject();
                instObject.gameObject.SetActive(false);
                _poolObjects.Enqueue(instObject);
                _allPoolObjects.Add(instObject);
            }
        }
        
        public T Spawn()
        {
            if (_poolObjects.Count > 0)
            {
                T poolObject = _poolObjects.Dequeue();
                poolObject.gameObject.SetActive(true);
                poolObject.OnSpawn();
                return poolObject;
            }
            
            var instObject = CreateObject();
            instObject.gameObject.SetActive(true);
            instObject.OnSpawn();
            _allPoolObjects.Add(instObject);
            return instObject;
        }

        public void Despawn(T poolObject)
        {
            poolObject.gameObject.SetActive(false);
            poolObject.OnDespawn();
            _poolObjects.Enqueue(poolObject);
        }

        private T CreateObject()
        {
            T newObj = Object.Instantiate(_prefab, _parent);
            newObj.gameObject.name = $"{_prefab.name}_pooled";
            return newObj;
        }
    }

    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}