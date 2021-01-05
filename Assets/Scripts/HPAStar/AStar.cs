using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System;

public class AStar
{
    public Graph graph;
    public MapData mapData;

    private FastPriorityQueue<Node> openSet;
    private List<Node> closedSet;

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

            string output = $"Target: {targetNode}\n";

            foreach (Node neighbor in graph.Neighbors(targetNode))
            {
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


    public void BuildGraph(MapData mapData)
    {
        this.mapData = mapData;

        graph = new Graph(mapData.Cells.Length);

        for (int x = -mapData.Size; x <= mapData.Size; x++)
        {
            for (int y = -mapData.Size; y <= mapData.Size; y++)
            {
                if (mapData.GetCell(x, y).Solid) continue;

                Node node = BuildNode(x, y);

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
            Index = (x + mapData.Size) + mapData.Width * (y + mapData.Size),
            Position = new Vector2Int(x, y),
            GScore = 0,
            FScore = 0,
            Previous = null,
        };

        graph.AddNode(node);

        return node;
    }


    private void BuildEdges(Node targetNode)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int position = new Vector2Int(
                    targetNode.Position.x + x, targetNode.Position.y + y
                );

                bool xOffMap = position.x < -mapData.Size || position.x > mapData.Size;
                bool yOffMap = position.y < -mapData.Size || position.y > mapData.Size;

                if (xOffMap || yOffMap) continue;

                bool solid = mapData.GetCell(position).Solid;

                if (solid) continue;

                Node candidateNode = GetNode(position) ?? BuildNode(position);

                float candidateDistance = Vector2Int.Distance(targetNode.Position, position);

                graph.AddEdge(targetNode, candidateNode, candidateDistance);
            }
        }
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

        Node current = node;

        int timer = 0;
        int timeout = 16;

        while (current.Previous != null && ++timer < timeout)
        {
            pathData.Nodes.Insert(0, current);
            current = current.Previous;
        }

        if (timer >= timeout) Debug.Log("Timed out, invalid path");

        return pathData;
    }


    public Node GetNode(int x, int y)
    {
        return GetNode(new Vector2Int(x, y));
    }


    public Node GetNode(Vector2Int position)
    {
        return graph.Nodes.Find(node => node.Position == position);
    }

}