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

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar()
    {
        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
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

        Debug.Log(graph);
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
}