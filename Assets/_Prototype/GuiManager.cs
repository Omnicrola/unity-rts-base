using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GuiManager : MonoBehaviour
    {
        public  CursorManager CursorManager;
        public UnitSelectionGroup CurrentSelection { get; private set; }
        public PlayerController LocalPlayer { get; set; }

        private void Start()
        {
            CurrentSelection = UnitSelectionGroup.EMPTY;
        }

        public void SelectAll()
        {
            var units = LocalPlayer.GetAllPlayerUnits();
            CurrentSelection = new UnitSelectionGroup(LocalPlayer, units);
        }

        public void ClearSelection()
        {
            CurrentSelection = UnitSelectionGroup.EMPTY;
        }

        public void Select(List<SelectableUnit> units)
        {
            Debug.Log("Selected " + units.Count + " units");
            CurrentSelection = new UnitSelectionGroup(LocalPlayer, units);
        }

    }
}