using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 10;

        public Grid grid;

        private List<Node> _openNodes;
        private HashSet<Node> _closedNodes;

        private List<Node> _path;

        public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
        {
            grid.CalculateNodes();
            Node startNode = grid.NodeFromWorldPosition(startPos);
            Node endNode = grid.NodeFromWorldPosition(endPos);
            if (!endNode.IsWalkable) return null;

            _openNodes = new List<Node> { startNode };
            _closedNodes = new HashSet<Node>();

            foreach (var node in grid.Nodes)
            {
                node.GCost = int.MaxValue;
                node.Parent = null;
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);

            while (_openNodes.Count > 0)
            {
                Node currentNode = _openNodes.First(node => node.FCost == _openNodes.Min(n => n.FCost));

                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                _openNodes.Remove(currentNode);
                _closedNodes.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    if (!neighbourNode.IsWalkable) _closedNodes.Add(neighbourNode);
                    if (_closedNodes.Contains(neighbourNode)) continue;

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if(tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.Parent = currentNode;
                        neighbourNode.GCost = tentativeGCost; ;
                        neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);

                        if (!_openNodes.Contains(neighbourNode)) _openNodes.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        private List<Vector3> CalculatePath(Node endNode)
        {
            List<Node> path = new List<Node>();
            path.Add(endNode);
            Node currentNode = endNode;
            while (currentNode.Parent != null)
            {
                path.Add(currentNode.Parent);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            _path = path;
            return NodesToVectors(path);
        }

        private List<Vector3> NodesToVectors(List<Node> nodes)
        {
            List<Vector3> result = new List<Vector3>(nodes.Count);
            foreach(var node in nodes)
            {
                result.Add(node.Position);
            }
            return result;
        }

        private int CalculateDistanceCost(Node a, Node b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private List<Node> GetNeighbourList(Node currentNode)
        {
            List<Node> neighbourList = new List<Node>();
            int currX = currentNode.X, currY = currentNode.Y;
            if (currentNode.X - 1 >= 0)
            {
                // Left
                neighbourList.Add(grid.Nodes[currX - 1, currY]);
                // Left Down
                if (currY - 1 >= 0) neighbourList.Add(grid.Nodes[currX - 1, currY - 1]);
                // Left Up
                if (currY + 1 < grid.GridHeight) neighbourList.Add(grid.Nodes[currX - 1, currY + 1]);
            }
            if (currX + 1 < grid.GridWidth)
            {
                // Right
                neighbourList.Add(grid.Nodes[currX + 1, currY]);
                // Right Down
                if (currY - 1 >= 0) neighbourList.Add(grid.Nodes[currX + 1, currY - 1]);
                // Right Up
                if (currY + 1 < grid.GridHeight) neighbourList.Add(grid.Nodes[currX + 1, currY + 1]);
            }
            // Down
            if (currY - 1 >= 0) neighbourList.Add(grid.Nodes[currX, currY - 1]);
            // Up
            if (currY + 1 < grid.GridHeight) neighbourList.Add(grid.Nodes[currX, currY + 1]);

            return neighbourList;
        }

        private void OnDrawGizmos()
        {
            if (_path != null)
            {
                foreach (var node in _path)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(node.Position, Vector3.one * grid.nodeSize);
                }
            }
        }
    }
}