using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class NetworkSpawnPool : NetworkBehaviour
    {
        public int InitialPoolSize = 5;
        public GameObject PooledPrefab;
        private readonly Queue<GameObject> _inactivePool = new Queue<GameObject>();
        private readonly List<GameObject> _activePool = new List<GameObject>();

        public NetworkHash128 AssetId { get; set; }

        private int instanceCount;
        private Transform _container;

        private void Awake()
        {
            AssetId = PooledPrefab.GetComponent<NetworkIdentity>().assetId;
        }

        void Start()
        {
            var container  = new GameObject(PooledPrefab.name +" pool container");
            container.transform.parent = transform;
            _container = container.transform;
            
            for (int i = 0; i < InitialPoolSize; ++i)
            {
                var pooledObject = CreateFreshPoolObject();
                _inactivePool.Enqueue(pooledObject);
            }

            ClientScene.RegisterSpawnHandler(AssetId, SpawnObject, UnSpawnObject);
        }

        private GameObject CreateFreshPoolObject()
        {
            instanceCount++;
            var pooledObject = Instantiate(PooledPrefab, Vector3.zero, Quaternion.identity);
            pooledObject.SendMessage("SetOwningPool", this);
            pooledObject.name = PooledPrefab.name + " pool(" + instanceCount + ")";
            pooledObject.SetActive(false);
            pooledObject.transform.parent = _container;
            return pooledObject;
        }

        public GameObject GetFromPool()
        {
            GameObject pooledObject = null;
            if (_inactivePool.Count > 0)
            {
                pooledObject = _inactivePool.Dequeue();
                _activePool.Add(pooledObject);
            }
            else
            {
                pooledObject = CreateFreshPoolObject();
            }

            pooledObject.SetActive(true);
            return pooledObject;
        }

        public GameObject SpawnObject(Vector3 position, NetworkHash128 assetId)
        {
            return GetFromPool();
        }

        public void UnSpawnObject(GameObject spawned)
        {
            _activePool.Remove(spawned);
            _inactivePool.Enqueue(spawned);
            spawned.SetActive(false);
            NetworkServer.UnSpawn(spawned);
        }
    }
}