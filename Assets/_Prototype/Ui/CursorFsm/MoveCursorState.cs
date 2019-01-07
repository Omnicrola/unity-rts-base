using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveCursorState : MonoBehaviour, ICursorState
    {
        public bool IsDefaultState
        {
            get { return false; }
        }

        public void EnterState()
        {
            
        }

        public Type Evaluate(CursorAdapter cursorAdapter)
        {
            if (Input.GetKeyDown(0))
            {
                if (cursorAdapter.MoveToTerrainPosition())
                {
                    return typeof(StartingCursorState);
                }
            }

            return null;
        }

        public void ExitState()
        {
        }
    }
}