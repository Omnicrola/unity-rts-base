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
        private Camera _mainCamera;
        public UnitSelectionGroup CurrentSelection { get; private set; }
        public event EventHandler<UnitSelectionChangedEvent> SelectionChanged;

        public CursorAdapter(Camera mainCamera, PlayerController localPlayer, LayerMask terrainMask, LayerMask unitMask)
        {
            _mainCamera = mainCamera;
            _localPlayer = localPlayer;
            _terrainMask = terrainMask;
            _unitMask = unitMask;
            CurrentSelection = new UnitSelectionGroup(localPlayer, new List<SelectableUnit>());
        }


        public void AttackMoveToCursorPosition()
        {
            var playerTeamId = _localPlayer.Team.TeamNumber;
            var selectedEnemy = GetUnitsUnderCursor()
                .FirstOrDefault(u => u.TeamNumber != playerTeamId);

            if (selectedEnemy != null)
            {
                CurrentSelection.AttackTarget(selectedEnemy);
            }
            else
            {
                MoveToTerrainPosition();
            }
        }

        public void Halt()
        {
            CurrentSelection.Halt();
        }

        public bool MoveToTerrainPosition()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, _terrainMask))
            {
                CurrentSelection.AttackLocation(hitInfo.point);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SpawnPlayerAt(Vector3 location)
        {
            _localPlayer.CmdSpawnStartingUnit(location);
        }

        private IEnumerable<SelectableUnit> GetUnitsUnderCursor()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
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
            CurrentSelection = new UnitSelectionGroup(_localPlayer, units);
            NotifySelectionChanged();
        }

        public void BoxSelect(Rect selectionBox)
        {
            var playerTeam = _localPlayer.Team.TeamNumber;
            var selectedUnits = TeamManager.Instance.AllUnitsForPlayer(playerTeam)
                .Where(IsInBounds(selectionBox))
                .ToList();
            CurrentSelection = new UnitSelectionGroup(_localPlayer, selectedUnits);
            NotifySelectionChanged();
        }

        private void NotifySelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged.Invoke(this, new UnitSelectionChangedEvent(CurrentSelection));
            }
        }

        private Func<SelectableUnit, bool> IsInBounds(Rect selectionBox)
        {
            return (unit) =>
            {
                var worldPosition = unit.transform.position;
                var screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
                return selectionBox.Contains(screenPosition);
            };
        }
    }

    public class UnitSelectionChangedEvent
    {
        public UnitSelectionGroup CurrentSelection { get; private set; }

        public UnitSelectionChangedEvent(UnitSelectionGroup currentSelection)
        {
            CurrentSelection = currentSelection;
        }
    }
}