using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Navigation;
using UnityEngine;
using _Prototype.Navigation;

namespace navigation
{
    public class AStarPathFinder
    {
        public PathfindingStats LastStats { get; private set; }
        public float PathfindingTimeLimit { get; set; }
        public float MaximumFrontierSize { get; set; }
        
        private readonly INavigationNodeSource _nodeSource;
        private List<AStarNode> _frontier;

        public AStarPathFinder(INavigationNodeSource nodeSource)
        {
            LastStats = new PathfindingStats();
            _frontier = new List<AStarNode>();
            PathfindingTimeLimit = 1000;
            MaximumFrontierSize = 1000;
            _nodeSource = nodeSource;
        }

        public List<Vector3> FindPath(Vector3 originPosition, Vector3 destinationPosition)
        {
            LastStats = new PathfindingStats();
            LastStats.Start();

            var origin = _nodeSource.GetNodeAtPosition(originPosition);
            var destination = _nodeSource.GetNodeAtPosition(destinationPosition);

            var usedPositions = new HashSet<AStarNode>();
            usedPositions.Add(destination);

            _frontier = new List<AStarNode>();
            _frontier.Add(destination);

            var gScores = new Dictionary<AStarNode, float>();
            // cost of going from destination to destination is 0
            gScores[destination] = 0;

            var fScores = new Dictionary<AStarNode, float>();
            fScores[destination] = Distance(origin, destination);

            while (LastStats.ElapsedTime < PathfindingTimeLimit &&
                   _frontier.Count > 0 &&
                   _frontier.Count < MaximumFrontierSize)
            {
                LastStats.Cycles++;

                var currentNode = _frontier.OrderBy(n => fScores[n]).First();
                _frontier.Remove(currentNode);
                usedPositions.Add(currentNode);

                if ((currentNode.Position - originPosition).magnitude <= 1)
                {
                    LastStats.Stop(true);
                    return currentNode.BuildPath();
                }

                var neighbors = _nodeSource.NeighborsOf(currentNode);
                foreach (var neighbor in neighbors)
                {
                    // already evaluated this node?
                    if (usedPositions.Contains(neighbor))
                    {
                        LastStats.SkippedNodes++;
                        continue;
                    }

                    var tenantiveScore = gScores[currentNode] + Distance(currentNode, neighbor);
                    if (!_frontier.Contains(neighbor))
                    {
                        _frontier.Add(neighbor);
                    }
                    else if (tenantiveScore >= gScores[neighbor])
                    {
                        LastStats.NodesWithHighScores++;
                        continue;
                    }

                    gScores[neighbor] = tenantiveScore;
                    fScores[neighbor] = tenantiveScore + HeuristicScore(neighbor, origin);
                }
            }

            Debug.Log(
                string.Format("No path found in {0}ms frontier size: {1} ", LastStats.ElapsedTime, _frontier.Count));
            LastStats.Stop(false);
            return new List<Vector3>();
        }


        private float HeuristicScore(AStarNode neighbor, AStarNode destination)
        {
            var orthoginalDistance = (neighbor.Position - destination.Position).Absolute();
            return orthoginalDistance.x +
                   orthoginalDistance.y +
                   orthoginalDistance.z;
        }

        private float Distance(AStarNode currentNode, AStarNode neighbor)
        {
            var magnitude = (currentNode.Position - neighbor.Position).magnitude;
            return magnitude;
        }

        public ReadOnlyCollection<AStarNode> GetLastFrontier()
        {
            return _frontier.AsReadOnly();
        }
    }
}