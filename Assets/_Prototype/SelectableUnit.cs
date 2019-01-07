using System.Numerics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Vector3 = UnityEngine.Vector3;

namespace DefaultNamespace
{
    [RequireComponent(typeof(UnitMovement), typeof(UnitAttack))]
    public class SelectableUnit : NetworkBehaviour
    {
        public float Hitpoints = 1;
        public short TeamNumber;

        private UnitMovement _unitMovement;
        private UnitAttack _unitAttack;

        private void Start()
        {
            _unitMovement = GetComponent<UnitMovement>();
            _unitAttack = GetComponent<UnitAttack>();
            TeamManager.Instance.RegisterUnit(this);
        }

        private void OnDestroy()
        {
            TeamManager.Instance.UnregisterUnit(this);
        }

        [Server]
        public void MoveTo(Vector3 location)
        {
            _unitAttack.CeaseFire();
            _unitMovement.MoveTo(location);
        }

        [Server]
        public void Halt()
        {
            _unitMovement.Stop();
            _unitAttack.CeaseFire();
        }

        [Server]
        public void AttackMove(Vector3 position)
        {
            _unitMovement.MoveTo(position);
            _unitAttack.FireAtWill();
        }

        public NetworkInstanceId GetNetworkId()
        {
            return GetComponent<NetworkIdentity>().netId;
        }

        [Server]
        public void AttackTarget(SelectableUnit target)
        {
            _unitMovement.MoveToWithin(target.transform.position, _unitAttack.Range);
            _unitAttack.SetTarget(target);
        }
    }
}