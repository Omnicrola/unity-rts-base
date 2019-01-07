using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class UnitAttack : NetworkBehaviour
    {
        public GameObject ProjectilePrefab;
        public Transform MuzzlePosition;
        public float EnemyScanInterval = 1;
        public float Range = 5;
        public float Damage = 1;
        public float Speed = 100f;
        public float RoundsPerMinute = 60;
        [Range(0.1f, 1.0f)] public float Accuracy = 0.9f;


        private SelectableUnit _currentTarget;
        private bool _isAttacking;
        private float _lastShotTime;
        private bool _fireAtWill;
        private float _nextScanTime;
        private short _teamId;

        private void Start()
        {
            _teamId = GetComponent<SelectableUnit>().TeamNumber;
        }

        public void SetTarget(SelectableUnit targetUnit)
        {
            _currentTarget = targetUnit;
            _isAttacking = true;
            _fireAtWill = false;
        }

        public void FireAtWill()
        {
            _fireAtWill = true;
            _isAttacking = true;
        }

        public void CeaseFire()
        {
            _isAttacking = false;
            _fireAtWill = false;
        }

        [Server]
        private void Update()
        {
            if (isServer && _isAttacking)
            {
                if (_fireAtWill && _currentTarget == null)
                {
                    AcquireTarget();
                }

                if (HasTarget() && CanFireNextShot() && IsInRange())
                {
                    _lastShotTime = Time.time;
                    FireShot();
                }
            }
        }

        private void AcquireTarget()
        {
            if (_nextScanTime <= Time.time)
            {
                _nextScanTime = Time.time + EnemyScanInterval;
                _currentTarget = TeamManager.Instance.GetEnemiesOf(_teamId)
                    .OrderBy(Distance)
                    .Where(InRange)
                    .FirstOrDefault();
            }
        }

        private bool InRange(SelectableUnit unit)
        {
            var direction = unit.transform.position - transform.position;
            return direction.magnitude < Range;
        }

        private float Distance(SelectableUnit unit)
        {
            var direction = unit.transform.position - transform.position;
            return direction.magnitude;
        }

        private void FireShot()
        {
            var origin = MuzzlePosition == null ? transform.position : MuzzlePosition.position;
            var targetPosition = _currentTarget.transform.position;
            var projectile = Instantiate(ProjectilePrefab, origin, Quaternion.identity);
            projectile.transform.LookAt(targetPosition);
            var projectileController = projectile.GetComponent<ProjectileController>();
            projectileController.ResetProjectile(Speed, Range, _teamId);
        }

        private bool HasTarget()
        {
            return _currentTarget != null;
        }


        private bool CanFireNextShot()
        {
            var elapsed = Time.time - _lastShotTime;
            var timeHasElapsed = elapsed >= RoundsPerMinute / 60f;
            return timeHasElapsed;
        }

        private bool IsInRange()
        {
            var delta = transform.position - _currentTarget.transform.position;
            var distance = delta.magnitude;
            return distance <= Range;
        }
    }
}