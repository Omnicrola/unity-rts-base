using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class ProjectileController : NetworkBehaviour, IPoolableObject
    {
        public LayerMask ImpactMask;
        public GameObject ExplosionPrefab;
        public float ProjectileSize = 1f;

        private NetworkSpawnPool _owningPool;

        [SyncVar] public Vector3 _startPosition;
        [SyncVar] public Vector3 _startRotation;
        [SyncVar] public float _speed;
        [SyncVar] public float _range;
        [SyncVar] public short _teamId;

        public void ResetProjectile(float speed, float range, short teamId)
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation.eulerAngles;
            _speed = speed;
            _range = range;
            _teamId = teamId;
        }

        public override void OnStartClient()
        {
            transform.rotation = Quaternion.Euler(_startRotation);
        }

        private void Update()
        {
            var currentPosition = transform.position;
            var distance = (currentPosition - _startPosition).magnitude;
            if (distance > _range)
            {
                ReturnToPool();
            }

            var newPosition = currentPosition + transform.forward * _speed * Time.deltaTime;
            var ray = new Ray(currentPosition, transform.forward);
            var offset = newPosition - currentPosition;
            transform.position = newPosition;

            var collisions = Physics.SphereCastAll(ray, ProjectileSize, offset.magnitude, ImpactMask)
                .Where(r => !r.collider.isTrigger)
                .OrderBy(r => r.distance)
                .ToList();
            if (collisions.Any())
            {
                var hit = collisions.First();
                if (ExplosionPrefab)
                {
                    var impact = Instantiate(ExplosionPrefab, hit.point, Quaternion.identity);
                    Destroy(impact, 5);
                }

                Debug.Log("HIT : " + hit.collider.name);
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            _owningPool.UnSpawnObject(gameObject);
        }

        public void SetOwningPool(NetworkSpawnPool pool)
        {
            _owningPool = pool;
        }
    }
}