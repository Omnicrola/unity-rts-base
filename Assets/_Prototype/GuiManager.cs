using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GuiManager : MonoBehaviour
    {
        public GameObject DebugPanel;

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                DebugPanel.SetActive(!DebugPanel.activeInHierarchy);
            }
        }
    }
}