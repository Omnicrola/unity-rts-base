using UnityEngine;

namespace DefaultNamespace
{
    public class SpawnPicker : MonoBehaviour
    {
        public Camera MainCamera;
        public LayerMask TerrainMask;
        public float VerticalOffset;
        public GameObject SelectionMarker;

        public bool IsPicking
        {
            get { return gameObject.activeInHierarchy; }
        }

        public bool HasPicked { get; private set; }
        public Vector3 PickedLocation { get; private set; }

        public void BeginPicking()
        {
            gameObject.SetActive(true);
            HasPicked = false;
        }
        public void EndPicking()
        {
            HasPicked = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            var ray = MainCamera.ScreenPointToRay(Input.mousePosition);

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
                    HasPicked = true;
                    PickedLocation = hitInfo.point;
                }
            }
            else
            {
                SelectionMarker.SetActive(false);
            }
        }

    }
}