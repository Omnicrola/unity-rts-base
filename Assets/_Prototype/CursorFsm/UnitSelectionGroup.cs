using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class UnitSelectionGroup
    {
        private readonly PlayerController _localPlayer;
        public static readonly UnitSelectionGroup EMPTY = new UnitSelectionGroup(null, new List<SelectableUnit>());
        public List<SelectableUnit> units { get; private set; }

        public UnitSelectionGroup(SelectableUnit oneUnit)
        {
            this.units = new List<SelectableUnit>() {oneUnit};
        }

        public UnitSelectionGroup(PlayerController localPlayer, List<SelectableUnit> units)
        {
            _localPlayer = localPlayer;
            this.units = units;
        }

        public void MoveTo(Vector3 location)
        {
            ForEachUnitByNetId((netId) => _localPlayer.CmdMoveUnit(netId, location));
        }
        public void AttackLocation(Vector3 position)
        {
            ForEachUnitByNetId((netId) => _localPlayer.CmdAttackMove(netId, position));
        }
        public void AttackTarget(SelectableUnit selectableUnit)
        {
            var targetNetId = selectableUnit.GetNetworkId();
            ForEachUnitByNetId(netId=>_localPlayer.CmdAttackTarget(netId, targetNetId));
        }

        private void ForEachUnitByNetId(Action<NetworkInstanceId> action)
        {
            foreach (var selectableUnit in units)
            {
                action(selectableUnit.GetNetworkId());
            }
        }

        public void Halt()
        {
            ForEachUnitByNetId((netId)=>_localPlayer.CmdHalt(netId));
        }
    }
}