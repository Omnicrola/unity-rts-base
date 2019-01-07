using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SelectedUnitsViewController : MonoBehaviour
    {
        public CursorManager CursorManager;
        public TextMeshProUGUI TextDisplay;

        private void Start()
        {
            CursorManager.SelectionChanged += OnSelectionChanged;
        }

        private void OnDestroy()
        {
            CursorManager.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, UnitSelectionChangedEvent e)
        {
            UpdateDisplay(e.CurrentSelection);
        }

        private void UpdateDisplay(UnitSelectionGroup currentSelection)
        {
            if (currentSelection.units.Any())
            {
                var unitCounts = currentSelection.units
                    .GroupBy(u => u.UnitName)
                    .ToDictionary(k => k.Key, v => v.Count())
                    .Select(kv => kv.Key + " : " + kv.Value)
                    .Aggregate((a, b) => a + "\n" + b);
                TextDisplay.text = unitCounts;
            }
            else
            {
                TextDisplay.text = "No units selected";
            }
        }
    }
}