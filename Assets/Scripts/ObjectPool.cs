using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class ObjectPool
    {
        private readonly List<GameObject> _pool = new List<GameObject>();

        private readonly  GameObject _prefab;

        public ObjectPool(int qty, GameObject prefab)
        {
            _prefab = prefab;
            AddToPool(qty);
        }

        private void AddToPool(int qty)
        {
            for (var count = 0; count < qty; count++)
            {
                var obj = Object.Instantiate(_prefab);
                obj.SetActive(false);
                _pool.Add(obj);
            }
        }

        public GameObject GetInstance()
        {
            var result = _pool.FirstOrDefault(o => !o.activeInHierarchy);

            if (result != null)
                return result;

            result = Object.Instantiate(_prefab);
            _pool.Add(result);

            Debug.LogWarning("Pool was exhusted. Recommend increasing size.");

            return result;
        }
    }
}
