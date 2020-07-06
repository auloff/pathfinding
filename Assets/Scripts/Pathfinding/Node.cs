using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsWalkable { get; set; }
        public Vector3 Position { get; set; }

        public Node Parent { get; set; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get => GCost + HCost; }

        public Node(bool isWalkable, Vector3 position, int x, int y)
        {
            X = x;
            Y = y;
            IsWalkable = isWalkable;
            Position = position;
        }
    }
}