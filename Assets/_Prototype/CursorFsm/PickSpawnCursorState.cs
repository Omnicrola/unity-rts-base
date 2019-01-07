using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PickSpawnCursorState : MonoBehaviour, ICursorState
    {
        public LayerMask TerrainMask;
        public float VerticalOffset;
        public GameObject SelectionMarker;

        public bool IsDefaultState
        {
            get { return false; }
        }

        public void EnterState()
        {
            SelectionMarker.SetActive(true);
        }

        public Type Evaluate(CursorAdapter cursorAdapter)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, TerrainMask))
            {
                SelectionMarker.SetActive(true);
                var offsetTerrainPosition = new Vector3(
                    hitInfo.point.x,
                    hitInfo.point.y + VerticalOffset,
                    hitInfo.point.z);
                transform.position = offsetTerrainPosition;

                if (Input.GetMouseButtonUp(0))
                {
                    
                    cursorAdapter.SpawnPlayerAt(hitInfo.point);
                    return typeof(StartingCursorState);
                }
            }
            else
            {
                SelectionMarker.SetActive(false);
            }
            return null;
        }

        public void ExitState()
        {
            SelectionMarker.SetActive(false);
        }
    }
}