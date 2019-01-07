using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class AttackCursorState : MonoBehaviour, ICursorState
    {
        public LayerMask UnitMask;
        public LayerMask TerrainMask;

        public bool IsDefaultState
        {
            get { return false; }
        }

        public void EnterState()
        {
        }

        public Type Evaluate(CursorAdapter cursorAdapter)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cursorAdapter.AttackMoveToCursorPosition();
                return typeof(StartingCursorState);
            }

            return null;
        }

        public void ExitState()
        {
        }
    }
}