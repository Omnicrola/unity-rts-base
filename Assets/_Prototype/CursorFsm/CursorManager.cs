using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CursorManager : MonoBehaviour
    {
        public LayerMask TerrainMask;
        public LayerMask UnitMask;

        private Dictionary<Type, ICursorState> _states;
        private ICursorState _currentState;
        private CursorAdapter _cursorAdapter;
        private PlayerController _localPlayer;

        private void Start()
        {
            var cursorStates = GetComponents<ICursorState>();
            InitStateMachine(cursorStates);
        }

        public void RegisterLocalPlayer(PlayerController playerController)
        {
            _cursorAdapter = new CursorAdapter(playerController, TerrainMask, UnitMask);
            _localPlayer = playerController;
        }

        private void InitStateMachine(ICursorState[] cursorStates)
        {
            _states = cursorStates.ToDictionary(s => s.GetType(), s => s);
            _currentState = cursorStates.First(s => s.IsDefaultState);
            _currentState.EnterState();
        }

        private void Update()
        {
            if (_localPlayer == null)
            {
                return;
            }
            
            var transitionState = _currentState.Evaluate(_cursorAdapter);
            if (transitionState != null && transitionState != _currentState.GetType())
            {
                _currentState.ExitState();
                _currentState = _states[transitionState];
                _currentState.EnterState();
            }

            // spawner
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                var position = new Vector3(Random.value, Random.value, Random.value) * 5f;
                _localPlayer.CmdSpawnOneUnit(position);
            }
        }

        public void PickSpawnPosition()
        {
        }
    }
}