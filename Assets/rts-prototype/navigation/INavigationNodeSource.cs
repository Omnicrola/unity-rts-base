using System.Collections.Generic;
using DefaultNamespace.Navigation;
using UnityEngine;

namespace navigation
{
    public interface INavigationNodeSource
    {
        AStarNode GetNodeAtPosition(Vector3 position);
        IEnumerable<AStarNode> NeighborsOf(AStarNode currentNode);
    }
}