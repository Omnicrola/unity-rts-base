using System.Linq;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class DebugReadout : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        private void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hits = Physics.RaycastAll(ray);

            var units = hits.Select(c => c.transform.GetComponent<SelectableUnit>())
                .Where(u => u != null)
                .ToList();

            Text.text = string.Format("Under cursor : {0} objects {1} units", hits.Length, units.Count);
        }
    }
}