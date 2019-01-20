using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Navigation
{
    public class AStarNode
    {
        public AStarNode(AStarNode parent, float cost, Vector3 position)
        {
            Parent = parent;
            Cost = cost;
            Position = position;
        }

        public AStarNode Parent { get; private set; }
        public float Cost { get; private set; }
        public Vector3 Position { get; private set; }

        protected bool Equals(AStarNode other)
        {
            return Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AStarNode) obj);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        public List<Vector3> BuildPath()
        {
            var path = new List<Vector3>();
            var current = this;
            while (current != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }

            return path;
        }

        public override string ToString()
        {
            return string.Format("AStarNode[position:{0} cost:{1}]",
                Position,
                Cost);
        }
    }
}