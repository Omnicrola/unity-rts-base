using System;

namespace DefaultNamespace
{
    public interface ICursorState
    {
        bool IsDefaultState { get; }
        void EnterState();
        Type Evaluate(CursorAdapter cursorAdapter);
        void ExitState();
    }
}