using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class AStar
{
    public Graph Graph;

    public WorldMap WorldMap;

    private readonly FastPriorityQueue<Node> openSet;
    private readonly List<Node> closedSet;

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar()
    {
        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
        closedSet = new List<Node>(MaxPriorityQueueNodes);
    }


    public PathData FindPath(Node start, Node end)
    {
        openSet.Enqueue(start, CalculateFCost(start, end));
        closedSet.Clear();

        int timer = 0;
        int timeout = 60;

        while (openSet.First != end || ++timer > timeout)
        {
            Node targetNode = openSet.Dequeue();
            closedSet.Add(targetNode);

            foreach (KeyValuePair<int, Node> keyValuePair in Graph.Neighbors(targetNode))
            {
                Node neighbor = keyValuePair.Value;

                float gCost = CalcuateGCost(targetNode, neighbor);

                if (openSet.Contains(neighbor) && gCost < neighbor.GScore)
                {
                    openSet.Remove(neighbor);
                }

                if (closedSet.Contains(neighbor) && gCost < neighbor.GScore)
                {
                    closedSet.Remove(neighbor);
                }

                if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                {
                    neighbor.GScore = gCost;
                    neighbor.FScore = CalculateFCost(neighbor, end);

                    openSet.Enqueue(neighbor, neighbor.FScore);

                    neighbor.Previous = targetNode;
                }
            }
        }

        return PathFrom(end);
    }


    public void BuildGraph(WorldMap worldMap)
    {
        WorldMap = worldMap;

        Graph = new Graph(worldMap.Cells.Length);

        for (int x = -worldMap.Size; x <= worldMap.Size; x++)
        {
            for (int y = -worldMap.Size; y <= worldMap.Size; y++)
            {
                if (worldMap.GetCell(x, y).Solid) continue;

                Node node = GetNode(x, y) ?? BuildNode(x, y);

                BuildEdges(node);
            }
        }
    }



    // Building Methods

    public Node BuildNode(Vector2Int position)
    {
        return BuildNode(position.x, position.y);
    }


    public Node BuildNode(int x, int y)
    {
        Node node = new Node
        {
            Index = MapUtil.CoordsToIndex(x, y),
            Position = new Vector2Int(x, y),
            GScore = 0,
            FScore = 0,
            Previous = null,
        };

        Graph.AddNode(node);

        return node;
    }


    private void BuildEdges(Node targetNode)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int offset = new Vector2Int(x, y);

                if (ValidEdgeLocation(targetNode.Position, offset))
                {
                    Vector2Int neighborPosition = targetNode.Position + offset;

                    Node neighborNode = GetNode(neighborPosition) ?? BuildNode(neighborPosition);

                    float neighborDistance = Vector2Int.Distance(targetNode.Position, neighborPosition);

                    Graph.AddEdge(targetNode, neighborNode, neighborDistance);
                }
            }
        }
    }


    private bool ValidEdgeLocation(Vector2Int position, Vector2Int offset)
    {
        Vector2Int neighborPosition = position + offset;

        if (!MapUtil.OnMap(neighborPosition)) return false;

        if (WorldMap.GetCell(neighborPosition).Solid) return false;

        Vector2Int northPosition = position + new Vector2Int(0, 1);
        Vector2Int eastPosition = position + new Vector2Int(1, 0);
        Vector2Int southPosition = position + new Vector2Int(0, -1);
        Vector2Int westPosition = position + new Vector2Int(-1, 0);

        bool northSolid = !MapUtil.OnMap(northPosition) || WorldMap.GetCell(northPosition).Solid;
        bool eastSolid = !MapUtil.OnMap(eastPosition) || WorldMap.GetCell(eastPosition).Solid;
        bool southSolid = !MapUtil.OnMap(southPosition) || WorldMap.GetCell(southPosition).Solid;
        bool westSolid = !MapUtil.OnMap(westPosition) || WorldMap.GetCell(westPosition).Solid;

        if (offset.x == 1 && offset.y == 1)
        {
            if (northSolid || eastSolid) return false;
        }

        if (offset.x == 1 && offset.y == -1)
        {
            if (southSolid || eastSolid) return false;
        }

        if (offset.x == -1 && offset.y == 1)
        {
            if (westSolid || northSolid) return false;
        }

        if (offset.x == -1 && offset.y == -1)
        {
            if (southSolid || westSolid) return false;
        }

        return true;
    }


    
    // Cost Methods

    private float CalculateHCost(Node start, Node end)
    {
        return OctileDistance(start, end);
    }


    private float CalcuateGCost(Node start, Node end)
    {
        return start.GScore + Vector2Int.Distance(start.Position, end.Position);
    }


    private float CalculateFCost(Node start, Node end)
    {
        return start.GScore + CalculateHCost(start, end);
    }


    private float OctileDistance(Node start, Node end)
    {
        Vector2Int differenceVector = end.Position - start.Position;

        int minDifference = Mathf.Min(Mathf.Abs(differenceVector.x), Mathf.Abs(differenceVector.y));
        int maxDifference = Mathf.Max(Mathf.Abs(differenceVector.x), Mathf.Abs(differenceVector.y));

        float octileDistance =
            PathInfo.HorizontalWeight * maxDifference +
            (PathInfo.DiagonalWeight - PathInfo.HorizontalWeight) * minDifference;

        return octileDistance;
    }



    // Helper Methods

    private PathData PathFrom(Node node)
    {
        PathData pathData = new PathData
        {
            Valid = true,
            Nodes = new List<Node>(),
        };

        for (Node current = node; current != null; current = current.Previous)
        {
            pathData.Nodes.Insert(0, current);
        }

        return pathData;
    }


    public Node GetNode(int x, int y)
    {
        return GetNode(new Vector2Int(x, y));
    }


    public Node GetNode(Vector2Int position)
    {
        int nodeIndex = MapUtil.CoordsToIndex(position);

        if (Graph.Nodes.ContainsKey(nodeIndex))
        {
            return Graph.Nodes[nodeIndex];
        }

        return null;
    }
}