using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitActionViewController : MonoBehaviour
    {
        public CursorManager CursorManager;

        private void Update()
        {
            EvalKey(KeyCode.Q, Move);
            EvalKey(KeyCode.W, Stop);
            EvalKey(KeyCode.E, Attack);

            EvalKey(KeyCode.Q, Special1);
            EvalKey(KeyCode.Q, Special2);
            EvalKey(KeyCode.Q, Special3);
        }

        private void EvalKey(KeyCode keyCode, Action action)
        {
            if (Input.GetKeyDown(keyCode))
            {
                action();
            }
        }

        public void Move()
        {
            CursorManager.SetState(typeof(MoveCursorState));
        }

        public void Stop()
        {
            CursorManager.CursorAdapter.Halt();
        }

        public void Attack()
        {
            CursorManager.SetState(typeof(AttackCursorState));
        }

        public void Special1()
        {
            Debug.Log("Use special 1");
        }

        public void Special2()
        {
            Debug.Log("Use special 2");
        }

        public void Special3()
        {
            Debug.Log("Use special 3");
        }
    }
}