using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


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
            if (_pools.ContainsKey(config.PoolKey))
            {
                Debug.LogError($"Duplicate pool key: {config.PoolKey}");
                continue;
            }
            var pool = new Pool(config.Prefab, _poolContainer);
            _pools.Add(config.PoolKey, pool);
            pool.Prewarm(config.InitialSize);
        }
    }

    public GameObject Get(string poolKey)
    {
        if (!_pools.TryGetValue(poolKey, out Pool pool))
        {
            Debug.LogError($"Pool not found: {poolKey}");
            return null;
        }

        return pool.Get();
    }

    public void Return(GameObject obj)
    {
        if (!obj.TryGetComponent<IPoolObject>(out var pooledObject))
        {
            Debug.LogError("Object is not a pooled object: " + obj.name);
            Destroy(obj);
            return;
        }

        if (!_pools.TryGetValue(pooledObject.PoolKey, out Pool pool))
        {
            Debug.LogError($"Pool not found for object: {obj.name}");
            Destroy(obj);
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
    }

    private class Pool
    {
        private readonly GameObject _prefab;
        private readonly Transform _container;
        private readonly Queue<GameObject> _objects = new Queue<GameObject>();
        private readonly List<GameObject> _activeObjects = new List<GameObject>();

        public Pool(GameObject prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _objects.Enqueue(CreateNewObject());
            }
        }

        public GameObject Get()
        {
            if (_objects.Count == 0)
            {
                Debug.LogWarning($"Pool empty, creating new object: {_prefab.name}");
                return CreateNewObject(true);
            }

            var obj = _objects.Dequeue();
            _activeObjects.Add(obj);
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.transform.SetParent(_container);
            _activeObjects.Remove(obj);
            _objects.Enqueue(obj);
            obj.SetActive(false);
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
