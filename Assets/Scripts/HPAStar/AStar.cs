using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System;

public class AStar
{
    public Graph graph;

    private FastPriorityQueue<Node> openSet;

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar()
    {
        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
    }


    public void BuildGraph(MapData mapData)
    {
        int[,] mapTestCells = new int[,]
        {
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0 },
        };

        graph = new Graph(mapTestCells.GetLength(0) * mapTestCells.GetLength(1));

        for (int y = 0; y < mapTestCells.GetLength(1); y++)
        {
            for (int x = 0; x < mapTestCells.GetLength(0); x++)
            {
                if (mapTestCells[x, y] == 1) continue;

                Node node = new Node(x, y);
                graph.AddNode(node);

                BuildEdges(mapTestCells, node);
            }
        }


        Node testNode = GetNode(1, 1);

        foreach (Node neighbor in graph.Neighbors(testNode))
        {
            Debug.Log(neighbor);
        }
    }


    public void BuildEdges(int[,] mapData, Node node)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int position = new Vector2Int(node.Position.x + x, node.Position.y + y);

                bool samePosition = position == node.Position;

                if (samePosition) continue;

                bool xOffMap = position.x < 0 || position.x >= mapData.GetLength(0);
                bool yOffMap = position.y < 0 || position.y >= mapData.GetLength(1);

                if (xOffMap || yOffMap) continue;

                bool solid = mapData[position.x, position.y] == 1;

                if (solid) continue;

                Node candidateNode = GetNode(position);
                float candidateDistance = Vector2Int.Distance(node.Position, position);

                graph.AddEdge(node, candidateNode, candidateDistance);
            }
        }
    }


    public Node GetNode(int x, int y)
    {
        return GetNode(new Vector2Int(x, y));
    }


    public Node GetNode(Vector2Int position)
    {
        Node node = graph.Nodes.Find(testNode => position == testNode.Position);

        return node ?? new Node(position);
    }
}