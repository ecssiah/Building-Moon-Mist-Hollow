using System;
using System.Collections.Generic;
using Priority_Queue;

public class Node : FastPriorityQueueNode
{
    public List<Edge> Edges;


    public Node()
    {
        Edges = new List<Edge>();
    }
}
