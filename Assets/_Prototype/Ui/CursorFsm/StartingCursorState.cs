using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class StartingCursorState : MonoBehaviour, ICursorState
    {
        public LayerMask UnitLayerMask;
        public LayerMask TerrainLayerMask;
        
        public bool IsDefaultState
        {
            get { return true; }
        }

        public void EnterState()
        {
            
        }

        public Type Evaluate(CursorAdapter cursorAdapter)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return typeof(AttackCursorState);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                return typeof(MoveCursorState);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                cursorAdapter.Halt();
            }

            if (Input.GetMouseButtonDown(1))
            {
                cursorAdapter.AttackMoveToCursorPosition();
            }

            if (Input.GetMouseButtonDown(0))
            {
                return typeof(SelectionCursorState);
            }
            return null;
        }

        public void ExitState()
        {
        }
    }
}