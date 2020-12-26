using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class AStar
{
    public Graph graph;

    private FastPriorityQueue<Node> openSet;

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar(Graph graph)
    {
        this.graph = graph;

        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
    }


    public List<Node> FindPath(Node start, Node end)
    {
        openSet.Enqueue(start, 0);

        return new List<Node>();
    }


    public Node GetNodeAt(int x, int y)
    {
        return graph.GetNodeAt(x, y);
    }
}