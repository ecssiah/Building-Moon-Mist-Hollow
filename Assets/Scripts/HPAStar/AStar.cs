using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public Graph graph;

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar(Graph graph)
    {
        this.graph = graph;

    }


    public List<Node> FindPath(Node start, Node end)
    {



        return new List<Node>();
    }
}
