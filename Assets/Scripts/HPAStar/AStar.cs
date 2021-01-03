using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System;

public class AStar
{
    public Graph graph;
    public MapData mapData;

    private Node target;

    private FastPriorityQueue<Node> openSet;

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar()
    {
        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
    }


    public PathData FindPath(Node start, Node end)
    {
        target = end;

        openSet.Enqueue(start, CalculateFCost(start, end));

        int timer = 0;
        int timeout = 10000;

        while (openSet.Count > 0 || timer++ > timeout)
        {
            Node current = openSet.Dequeue();

            if (current == target)
            {
                return PathFrom(target);
            }

            foreach (Node neighbor in graph.Neighbors(current))
            {
                neighbor.Previous = current;
                neighbor.GScore = CalcuateGCost(current, neighbor);
                neighbor.FScore = CalculateFCost(neighbor, end);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Enqueue(neighbor, neighbor.FScore);
                }
            }
        }

        return new PathData { Success = false };
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


    public Node GetNode(int x, int y)
    {
        return GetNode(new Vector2Int(x, y));
    }


    public Node GetNode(Vector2Int position)
    {
        return graph.Nodes.Find(node => node.Position == position);
    }


    private float CalcuateGCost(Node start, Node end)
    {
        return start.GScore + CalculateHCost(start, end);
    }


    private float CalculateHCost(Node start, Node end)
    {
        return Vector2Int.Distance(start.Position, end.Position);
    }


    private float CalculateFCost(Node start, Node end)
    {
        float hCost = CalcuateGCost(start, end);

        return start.GScore + hCost;
    }


    private PathData PathFrom(Node node)
    {
        PathData pathData = new PathData
        {
            Success = true,
            Nodes = new List<Node> { node }
        };

        Node current = node;

        int timer = 0;
        int timeout = 10000;

        while ((current.Previous != null) && (++timer < timeout))
        {
            pathData.Nodes.Insert(0, current);
            current = current.Previous;
        }

        Debug.Log(pathData);

        return pathData;
    }
}