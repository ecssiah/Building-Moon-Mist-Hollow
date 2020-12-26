using System;
using System.Collections.Generic;
using Priority_Queue;


public class Node : FastPriorityQueueNode
{
    public List<Edge> Edges;

    public float[] Position;


    public Node()
    {
        Edges = new List<Edge>();
        Position = new float[] { 0, 0 };
    }


    public Node(float x, float y)
    {
        Edges = new List<Edge>();
        Position = new float[] { x, y };
    }
}
