using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Navigation;
using UnityEngine;

namespace navigation
{
    internal class GridBasedNodeSource : INavigationNodeSource
    {
        public AStarNode GetNodeAtPosition(Vector3 position)
        {
            return new AStarNode(null, 1, position.Rounded());
        }

        public IEnumerable<AStarNode> NeighborsOf(AStarNode node)
        {
            var neighbors = new List<AStarNode>();

            VoxelUtil.CubeWalk(3, (x, y, z, index) =>
            {
                if (x == 1 && y == 1 && z == 1) return;
                var position = (node.Position + new Vector3(x - 1, y - 1, z - 1)).Rounded();
                var neighbor = new AStarNode(node, 1f, position);
                neighbors.Add(neighbor);
            });

            return neighbors;
        }
    }
}