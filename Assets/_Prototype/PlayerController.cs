using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject InitialSpawn;

    public Team Team
    {
        get { return TeamManager.Instance.GetTeamForPlayer(playerControllerId); }
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            var cursorManager = FindObjectOfType<CursorManager>();
            cursorManager.RegisterLocalPlayer(this);
            cursorManager.PickSpawnPosition();
        }
        else
        {
            enabled = false;
        }
    }

    [Command]
    public void CmdSpawnStartingUnit(Vector3 position)
    {
        var newUnit = Instantiate(InitialSpawn, position, Quaternion.identity, transform);
        TeamManager.Instance.SpawnUnitForTeam(playerControllerId, newUnit);
    }

    [Command]
    public void CmdMoveUnit(NetworkInstanceId targetInstanceId, Vector3 location)
    {
        WithNetId(targetInstanceId, unit => unit.MoveTo(location));
    }

    [Command]
    public void CmdAttackMove(NetworkInstanceId netId, Vector3 position)
    {
        WithNetId(netId, unit => unit.AttackMove(position));
    }

    [Command]
    public void CmdSpawnOneUnit(Vector3 position)
    {
        var newUnit = Instantiate(InitialSpawn, transform);
        TeamManager.Instance.SpawnUnitForTeam(playerControllerId, newUnit);
    }

    [Command]
    public void CmdAttackTarget(NetworkInstanceId netId, NetworkInstanceId targetNetId)
    {
        var targetUnit = TeamManager.Instance.FindByNetId(targetNetId);
        WithNetId(netId, unit =>
            {
                if (TeamManager.UnitsAreEnemies(unit, targetUnit))
                {
                    unit.AttackTarget(targetUnit);
                }
                else
                {
                    Debug.Log("Invalid target, unit is not an enemy!");
                }
            }
        );
    }

    private void WithNetId(NetworkInstanceId netId, Action<SelectableUnit> action)
    {
        var targetUnit = TeamManager.Instance.FindByNetId(netId);
        action(targetUnit);
    }

    [Command]
    public void CmdHalt(NetworkInstanceId netId)
    {
        WithNetId(netId, unit => unit.Halt());
    }

    public List<SelectableUnit> GetAllPlayerUnits()
    {
        var team = TeamManager.Instance.GetTeamForPlayer(playerControllerId);
        return TeamManager.Instance.AllUnitsForPlayer(team.TeamNumber);
    }
}