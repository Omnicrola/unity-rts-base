using System.Collections.Generic;
using UnityEngine;

namespace navigation
{
//    [ExecuteInEditMode]
    public class NavigationManager : MonoBehaviour
    {
        public Vector3 Origin;
        public Vector3 Destination;
        public bool ShowDebug;

        private AStarPathFinder PathFinder = new AStarPathFinder(new GridBasedNodeSource());

        public float ElapsedTime { get; private set; }
        private List<Vector3> _path;

        private void Start()
        {
            FindPath();
        }

        public void FindPath()
        {
            _path = PathFinder.FindPath(Origin, Destination);
        }

        private void OnValidate()
        {
//            _path = FindPath(Origin, Destination);
        }

        private void Update()
        {
            _path = PathFinder.FindPath(Origin, Destination);
        }


        private void OnDrawGizmos()
        {
            if (ShowDebug && _path != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(Origin, .25f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Destination, .25f);
                Gizmos.color = Color.white;

                foreach (var p in _path)
                {
                    Gizmos.DrawSphere(p, .125f);
                }

                Gizmos.color = Color.yellow;
                foreach (var node in PathFinder.GetLastFrontier())
                {
                    Gizmos.DrawSphere(node.Position, .125f);
                }
            }
        }
    }
}