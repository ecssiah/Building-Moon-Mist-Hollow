using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


public class AStar
{
    private Graph graph;

    private FastPriorityQueue<Node> openSet;

    private readonly int MaxPriorityQueueNodes = 1000;


    public AStar(Graph graph)
    {
        this.graph = graph;

        openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
    }


}
