using System.Collections.Generic;
using UnityEngine;

namespace TechBear.Pooling
{
    public class CustomObjectPool<T> where T : MonoBehaviour
    {
        #region Private Variables

        private readonly Transform _poolParent;
        private readonly Queue<T> _objects;
        private readonly T _prefab;

        #endregion

        #region Constructor

        public CustomObjectPool(T prefab, int initialCapacity, Transform poolParent = null)
        {
            _poolParent = poolParent;
            _prefab = prefab;
            _objects = new Queue<T>(initialCapacity);
        }

        #endregion

        #region Public Methods

        public int Count()
        {
            return _objects.Count;
        }

        public T Get()
        {
            if (_objects.Count == 0)
            {
                AddObjects(1);
            }

            T obj = _objects.Dequeue();
            obj.gameObject.SetActive(true);

            return obj;
        }

        public void Release(T obj)
        {
            if (_objects.Count >= 1000)
            {
                Object.Destroy(obj.gameObject);
            }
            
            obj.gameObject.SetActive(false);
            obj.transform.parent = _poolParent;
            _objects.Enqueue(obj);
        }

        #endregion

        #region Private Variables

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = Object.Instantiate(_prefab, _poolParent);
                obj.gameObject.SetActive(false);
                _objects.Enqueue(obj);
            }
        }

        #endregion
    }
}