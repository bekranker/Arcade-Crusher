using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private Transform _poolContainer;

    void Awake()
    {
        _poolContainer = new GameObject("PoolContainer").transform;
        _poolContainer.SetParent(transform);
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var config in poolConfigs)
        {

            var pool = new Pool(config.Prefab, _poolContainer);
            _pools.Add(config.PoolKey, pool);
            if (config.Prefab.TryGetComponent<IPoolObject>(out var pooledObject))
            {
                pooledObject.OnInit(this);
            }
            pool.Prewarm(config.InitialSize, config.Active);
        }
    }

    public GameObject Get(string poolKey)
    {
        if (!_pools.TryGetValue(poolKey, out Pool pool))
        {
            return null;
        }
        return pool.Get();
    }
    public List<GameObject> GetAll(string poolKey)
    {
        if (!_pools.TryGetValue(poolKey, out Pool pool))
        {
            return null;
        }

        return pool.AllPoolObjects();
    }
    public void Return(GameObject obj)
    {
        string poolName = "";
        if (obj.TryGetComponent<IPoolObject>(out var pooledObject))
        {
            poolName = pooledObject.PoolKey;
        }
        else
        {
            poolConfigs.ForEach((pooledObject) =>
            {
                if (pooledObject.Prefab == obj)
                {
                    poolName = pooledObject.PoolKey;
                }
            });
        }
        if (string.IsNullOrEmpty(poolName))
        {
            return;
        }
        if (!_pools.TryGetValue(poolName, out Pool pool))
        {
            return;
        }
        pool.Return(obj);
    }

    [System.Serializable]
    public class PoolConfig
    {
        public string PoolKey;
        public GameObject Prefab;
        public int InitialSize = 10;
        public bool Active;
    }

    private class Pool
    {
        private readonly GameObject _prefab;
        private readonly Transform _container;
        private readonly Queue<GameObject> _objects = new Queue<GameObject>();

        public Pool(GameObject prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }
        public int PoolSize() => _objects.Count;
        public List<GameObject> AllPoolObjects() => new List<GameObject>(_objects);
        public void Prewarm(int count, bool active)
        {
            for (int i = 0; i < count; i++)
            {
                _objects.Enqueue(CreateNewObject(active));
            }
        }

        public GameObject Get()
        {
            if (_objects.Count == 0)
            {
                return CreateNewObject(true);
            }

            var obj = _objects.Dequeue();
            if (obj.TryGetComponent<IPoolObject>(out var pooledObject))
            {
                pooledObject.OnGet();
                obj.SetActive(true);
                return obj;
            }
            obj.SetActive(true);
            _objects.Enqueue(obj);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.transform.SetParent(_container);
            obj.SetActive(false);
            _objects.Enqueue(obj);
        }

        private GameObject CreateNewObject(bool activate = false)
        {
            var obj = Instantiate(_prefab, _container);
            obj.SetActive(activate);

            if (!activate)
            {
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
            }
            return obj;
        }
    }


}
