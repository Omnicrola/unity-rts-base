using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CursorAdapter
    {
        private readonly LayerMask _unitMask;
        private readonly LayerMask _terrainMask;
        private readonly PlayerController _localPlayer;
        private UnitSelectionGroup _currentSelection;
 
        public CursorAdapter(PlayerController localPlayer, LayerMask terrainMask, LayerMask unitMask)
        {
            _localPlayer = localPlayer;
            _terrainMask = terrainMask;
            _unitMask = unitMask;
            _currentSelection = new UnitSelectionGroup(localPlayer, new List<SelectableUnit>());
        }


        public void AttackMoveToCursorPosition()
        {
            var playerTeamId = _localPlayer.Team.TeamNumber;
            var selectedEnemy = GetUnitsUnderCursor()
                .FirstOrDefault(u => u.TeamNumber != playerTeamId);

            if (selectedEnemy != null)
            {
                _currentSelection.AttackTarget(selectedEnemy);
            }
            else
            {
                MoveToTerrainPosition();
            }
        }

        public void Halt()
        {
            _currentSelection.Halt();
        }

        public bool MoveToTerrainPosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, _terrainMask))
            {
                _currentSelection.AttackLocation(hitInfo.point);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetSelectedUnits(List<SelectableUnit> selected)
        {
            _currentSelection = new UnitSelectionGroup(_localPlayer, selected);
        }

        public void SpawnPlayerAt(Vector3 location)
        {
            _localPlayer.CmdSpawnStartingUnit(location);
        }

        private IEnumerable<SelectableUnit> GetUnitsUnderCursor()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.RaycastAll(ray, 1000, _unitMask, QueryTriggerInteraction.Ignore)
                .OrderBy(c => c.distance)
                .Select(c => c.collider.GetComponent<SelectableUnit>())
                .Where(u => u != null);
        }

        public void PointSelect()
        {
            var playerTeam = _localPlayer.Team.TeamNumber;
            var units = GetUnitsUnderCursor()
                .Where(u => u.TeamNumber == playerTeam)
                .ToList();
            _currentSelection = new UnitSelectionGroup(_localPlayer, units);
        }

        public void BoxSelect(Rect selectionBox)
        {
            var playerTeam = _localPlayer.Team.TeamNumber;
            var selectedUnits = TeamManager.Instance.AllUnitsForPlayer(playerTeam)
                .Where(IsInBounds(selectionBox))
                .ToList();
            _currentSelection = new UnitSelectionGroup(_localPlayer, selectedUnits);
        }

        private Func<SelectableUnit, bool> IsInBounds(Rect selectionBox)
        {
            return (unit) =>
            {
                var worldPosition = unit.transform.position;
                var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                return selectionBox.Contains(screenPosition);
            };
        }
    }
}