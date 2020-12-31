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
        graph = new Graph();

        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
    }


    public List<Node> FindPath(Node start, Node end)
    {
        openSet.Enqueue(start, CalculateCost(start, end));


        Node currentNode = openSet.Dequeue();

        if (currentNode == end)
        {
            return PathFrom(end);
        }

        foreach (Tuple<Node, float> nodeWeightTuple in currentNode.NeighborsAndWeights())
        {
            Node neighbor = nodeWeightTuple.Item1;
            float weight = nodeWeightTuple.Item2;

            neighbor.PreviousNode = currentNode;
            neighbor.GScore = currentNode.GScore + weight;
            neighbor.FScore = neighbor.GScore + CalculateCost(neighbor, end);

            if (!openSet.Contains(neighbor))
            {
                openSet.Enqueue(neighbor, neighbor.FScore);
            }
        }


        //while (openSet.Count > 0)
        //{
        //}

        return new List<Node>();
    }


    private List<Node> PathFrom(Node end)
    {
        Node currentNode = end;

        List<Node> path = new List<Node> { end };

        while (currentNode.PreviousNode != null)
        {
            path.Insert(0, currentNode.PreviousNode);

            currentNode = currentNode.PreviousNode;
        }

        return path;
    }


    public Node GetNodeAt(int x, int y)
    {
        return graph.GetNodeAt(x, y);
    }


    public Node GetNodeAt(Vector2Int position)
    {
        return GetNodeAt(position.x, position.y);
    }


    private float CalculateCost(Node node1, Node node2)
    {
        Vector2 node1Position = new Vector2(node1.Position.x, node1.Position.y);
        Vector2 node2Position = new Vector2(node2.Position.x, node2.Position.y);

        return Vector2.Distance(node1Position, node2Position);
    }
}