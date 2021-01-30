using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Node[,] nodeGrid;

    // Start is called before the first frame update
    void Start()
    {
    }

    public List<Node> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        nodeGrid = CreateNodeGrid(GameTiles.instance.worldTileData);
        int gridMaxSize = nodeGrid.GetLength(0) * nodeGrid.GetLength(1);
        Node startNode = nodeGrid[startPos.x, startPos.y];
        Node targetNode = nodeGrid[targetPos.x, targetPos.y];

        Heap<Node> openSet = new Heap<Node>(gridMaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();            
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if(!neighbour.traversable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        } else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.position.x + x;
                int checkY = node.position.y + y;

                if (checkX >= 0 && checkX < nodeGrid.GetLength(0) && checkY >= 0 && checkY < nodeGrid.GetLength(1))
                {
                    neighbours.Add(nodeGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node[,] CreateNodeGrid(WorldTileData[,] worldTileData)
    {
        int gridX = worldTileData.GetLength(0);
        int gridY = worldTileData.GetLength(1);
        nodeGrid = new Node[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                nodeGrid[x, y] = new Node(worldTileData[x, y].traversable, worldTileData[x, y].position);
            }
        }

        return nodeGrid;
    }

    public class Node : IHeapItem<Node>
    {
        public bool traversable;
        public Vector3Int position;
        public int gCost;
        public int hCost;
        public Node parent;
        int heapIndex;

        public Node(bool _traversable, Vector3Int _position)
        {
            traversable = _traversable;
            position = _position;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
