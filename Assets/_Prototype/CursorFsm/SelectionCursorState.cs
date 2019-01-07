using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class SelectionCursorState : MonoBehaviour, ICursorState
    {
        private const int BoxSelectThreshold = 5;

        public Rect SelectionBox = new Rect();
        private Vector3 _selectionStart;

        public bool IsDefaultState
        {
            get { return false; }
        }

        public bool IsSelecting { get; private set; }

        public void EnterState()
        {
            _selectionStart = Input.mousePosition;
            IsSelecting = true;
        }

        public Type Evaluate(CursorAdapter cursorAdapter)
        {
            UpdateSelectionBox();

            if (Input.GetMouseButtonUp(0))
            {
                var positionDelta = (_selectionStart - Input.mousePosition).magnitude;
                if (positionDelta <= BoxSelectThreshold)
                {
                    cursorAdapter.PointSelect();
                }
                else
                {
                    cursorAdapter.BoxSelect(SelectionBox);
                }

                return typeof(StartingCursorState);
            }

            return null;
        }

        public void ExitState()
        {
            IsSelecting = false;
        }

        private void UpdateSelectionBox()
        {
            var y = Screen.height - _selectionStart.y;
            var width = Input.mousePosition.x - _selectionStart.x;
            var height = (Screen.height - Input.mousePosition.y) - y;
            SelectionBox.Set(_selectionStart.x, y, width, height);
        }
    }
}