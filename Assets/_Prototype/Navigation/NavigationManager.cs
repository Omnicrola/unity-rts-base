using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Prototype.Navigation;
using Debug = UnityEngine.Debug;

namespace DefaultNamespace.Navigation
{
//    [ExecuteInEditMode]
    public class NavigationManager : MonoBehaviour
    {
        public Vector3 Origin;
        public Vector3 Destination;

        public float ElapsedTime { get; private set; }
        private List<Vector3> _path;

        private void Start()
        {
            FindPath();
        }

        public void FindPath()
        {
            StartCoroutine(FindPath(Origin, Destination));
        }

        private void OnValidate()
        {
//            _path = FindPath(Origin, Destination);
        }

        private void Update()
        {
//            _path = FindPath(Origin, Destination);
        }

        private List<AStarNode> _frontier;

        public IEnumerator FindPath(Vector3 originPosition, Vector3 destinationPosition)
        {
            var pathfindingStats = new PathfindingStats();
            pathfindingStats.Start();

            var origin = new AStarNode(null, 1, originPosition.Rounded());
            var destination = new AStarNode(null, 1, destinationPosition.Rounded());

            var usedPositions = new HashSet<AStarNode>();
            usedPositions.Add(origin);

            var frontier = new List<AStarNode>();
            frontier.Add(origin);

            var gScores = new Dictionary<AStarNode, float>();
            // cost of going from start to start is 0
            gScores[origin] = 0;
            
            var fScores = new Dictionary<AStarNode, float>();
            fScores[origin] = Distance(origin, destination);

            var notFinished = true;
            while (notFinished &&
                   pathfindingStats.ElapsedTime < 10000 &&
                   frontier.Count > 0 &&
                   frontier.Count < 1000)
            {
                ElapsedTime = pathfindingStats.ElapsedTime;
                pathfindingStats.Cycles++;
                var currentNode = frontier.OrderBy(n => fScores[n]).First();
                frontier.Remove(currentNode);
                usedPositions.Add(currentNode);

                if ((currentNode.Position - destinationPosition).magnitude <= 1)
                {
                    pathfindingStats.Stop(true);
//                    return currentNode.BuildPath();
                    _path = currentNode.BuildPath();
                    notFinished = false;
                }

                var neighbors = NeighborsOf(currentNode);
                foreach (var neighbor in neighbors)
                {
                    // already evaluated this node?
                    if (usedPositions.Contains(neighbor))
                    {
                        pathfindingStats.SkippedNodes++;
                        continue;
                    }

                    var tenantiveScore = gScores[currentNode] + Distance(currentNode, neighbor);
                    if (!frontier.Contains(neighbor))
                    {
                        frontier.Add(neighbor);
                    }
                    else if (tenantiveScore >= gScores[neighbor])
                    {
                        pathfindingStats.NodesWithHighScores++;
                        continue;
                    }

                    gScores[neighbor] = tenantiveScore;
                    fScores[neighbor] = tenantiveScore + Distance(neighbor, destination);
                }

                _frontier = frontier;
                _path = currentNode.BuildPath();
                yield return null;
            }

            pathfindingStats.Stop(false);
//            return new List<Vector3>();
        }

        private float Distance(AStarNode currentNode, AStarNode neighbor)
        {
            var magnitude = (currentNode.Position - neighbor.Position).magnitude;
            return magnitude;
        }

        private List<AStarNode> NeighborsOf(AStarNode node)
        {
            var neighbors = new List<AStarNode>
            {
                new AStarNode(node, 1f, (node.Position + Vector3.forward).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.forward + Vector3.right).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.right).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.right + Vector3.back).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.back).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.back + Vector3.left).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.left).Rounded()),
                new AStarNode(node, 1f, (node.Position + Vector3.left + Vector3.forward).Rounded())
            };
            return neighbors;
        }

        private void OnDrawGizmos()
        {
            if (_path != null && _frontier != null)
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
                foreach (var node in _frontier)
                {
                    Gizmos.DrawSphere(node.Position, .125f);
                }
            }
        }
    }
}