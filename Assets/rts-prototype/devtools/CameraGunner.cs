using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class CameraGunner : NetworkBehaviour
    {
        public SpawnManager SpawnManager;
        public GameObject ProjectilePrefab;
        private NetworkHash128 _assetId;

        private void Start()
        {
            _assetId = ProjectilePrefab.GetComponent<NetworkIdentity>().assetId;
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                CmdFire();
            }
        }

        [Command]
        void CmdFire()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            SpawnManager.Spawn(_assetId, obj =>
            {
                obj.transform.position = ray.origin;
                obj.transform.LookAt(ray.GetPoint(50));
                var projectileController = obj.GetComponent<ProjectileController>();
                projectileController.ResetProjectile(10, 50, 1);
            });
        }
    }
}