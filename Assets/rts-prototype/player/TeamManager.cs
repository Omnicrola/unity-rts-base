using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class TeamManager
    {
        #region Singleton

        private static TeamManager _instance;
        private static readonly object _instanceMutex = new Object();

        internal static TeamManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceMutex)
                    {
                        if (_instance == null)
                            _instance = new TeamManager();
                    }
                }

                return _instance;
            }
        }

        #endregion

        private Dictionary<short, Team> _playerMap;
        private Dictionary<short, List<SelectableUnit>> _unitMap;
        private List<SelectableUnit> _allUnits;
        private short _teamCount;

        private TeamManager()
        {
            Reset();
        }

        private void Reset()
        {
            _playerMap = new Dictionary<short, Team>();
            _unitMap = new Dictionary<short, List<SelectableUnit>>();
            _allUnits = new List<SelectableUnit>();
            _teamCount = 0;
        }

        public void AddTeam(short playerId)
        {
            var teamId = ++_teamCount;
            _playerMap[playerId] = new Team(teamId, playerId, "Team " + teamId);
            _unitMap[teamId] = new List<SelectableUnit>();
            Debug.Log("Added team " + _teamCount + " for player " + playerId);
        }

        public void SpawnUnitForTeam(short playerId, GameObject newUnit)
        {
            var teamId = _playerMap[playerId].TeamNumber;
            Debug.Log("Spawn unit for team " + teamId + " player " + playerId);
            var selectableUnit = newUnit.GetComponent<SelectableUnit>();
            selectableUnit.TeamNumber = teamId;
            NetworkServer.Spawn(newUnit);
        }

        public void RegisterUnit(SelectableUnit selectableUnit)
        {
            var teamId = selectableUnit.TeamNumber;
            Debug.Log("Register unit for team " + teamId);
            _unitMap[teamId].Add(selectableUnit);
            _allUnits.Add(selectableUnit);
        }

        public void UnregisterUnit(SelectableUnit selectableUnit)
        {
            var teamId = selectableUnit.TeamNumber;
            Debug.Log("Unregister unit for team " + teamId);
            _unitMap[teamId].Remove(selectableUnit);
            _allUnits.Remove(selectableUnit);
        }

        public SelectableUnit FindByNetId(NetworkInstanceId networkInstanceId)
        {
            return _allUnits.First(u => u.GetNetworkId() == networkInstanceId);
        }

        public static bool UnitsAreEnemies(SelectableUnit unit1, SelectableUnit targetUnit)
        {
            return unit1.TeamNumber != targetUnit.TeamNumber;
        }

        public List<SelectableUnit> AllUnitsForPlayer(short teamId)
        {
            var allPlayerUnits = _unitMap[teamId];
            return new List<SelectableUnit>(allPlayerUnits);
        }

        public List<SelectableUnit> GetEnemiesOf(short teamId)
        {
            return _allUnits
                .Where(u => u.TeamNumber != teamId)
                .ToList();
        }

        public Team GetTeamForPlayer(short playerControllerId)
        {
            return _playerMap[playerControllerId];
        }
    }
}