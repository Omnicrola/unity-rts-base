using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class SpawnManager : NetworkBehaviour
    {
        private Dictionary<NetworkHash128, NetworkSpawnPool> _pools;

        private void Start()
        {
            _pools = GetComponents<NetworkSpawnPool>()
                .ToDictionary(p => p.AssetId, p => p);
        } 

        [Server]
        public void Spawn(NetworkHash128 assetId, Action<GameObject> prespawnAction)
        {
            var pooledObj = _pools[assetId].GetFromPool();
            prespawnAction.Invoke(pooledObj);
            NetworkServer.Spawn(pooledObj);
        }

    }
}