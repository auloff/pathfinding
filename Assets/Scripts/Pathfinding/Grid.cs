using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class Grid : MonoBehaviour
    {
        [Header("Grid settings")]
        public LayerMask unwalkableMask;
        public Vector2 gridSize;
        [Min(0f)]
        public float nodeSize;

        public List<Node> FinalPath { get; private set; }
        public Node[,] Nodes { get; private set; }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }

        private void Start()
        {
            if (nodeSize == 0 && gridSize.x == 0 && gridSize.y == 0)
            {
                Debug.LogWarning("Wrong");
                return;
            }

            GridWidth = Mathf.RoundToInt(gridSize.x / nodeSize);
            GridHeight = Mathf.RoundToInt(gridSize.y / nodeSize);
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            Nodes = new Node[GridWidth, GridHeight];

            Vector3 bottomLeft = this.transform.position 
                - Vector3.right * (gridSize.x / 2) 
                - Vector3.forward * (gridSize.y / 2);

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    Vector3 worldPoint = bottomLeft 
                        + Vector3.right * (x * nodeSize + nodeSize / 2) 
                        + Vector3.forward * (y * nodeSize + nodeSize / 2);

                    bool isWalkable = !Physics.CheckSphere(worldPoint, nodeSize / 2, unwalkableMask);

                    Nodes[x, y] = new Node(isWalkable, worldPoint, x, y);
                }
            }
        }

        public void CalculateNodes()
        {
            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    bool isWalkable = !Physics.CheckSphere(Nodes[x, y].Position, nodeSize / 2, unwalkableMask);

                    Nodes[x, y].IsWalkable = isWalkable;
                }
            }
        }

        public Node NodeFromWorldPosition (Vector3 worldPoint)
        {
            float xPos = ((worldPoint.x + gridSize.x / 2) / gridSize.x);
            float yPos = ((worldPoint.z + gridSize.y / 2) / gridSize.y);

            xPos = Mathf.Clamp01(xPos);
            yPos = Mathf.Clamp01(yPos);

            int x = Mathf.RoundToInt((GridWidth - 1) * xPos);
            int y = Mathf.RoundToInt((GridHeight - 1) * yPos);

            return Nodes[x, y];
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

            if(Nodes != null)
            {
                foreach (var node in Nodes)
                {
                    if (node.IsWalkable)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                    }
                    Gizmos.DrawCube(node.Position, Vector3.one * nodeSize);
                }
            }
        }
    }
}