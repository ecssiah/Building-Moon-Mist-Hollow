using System;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


public class Node : FastPriorityQueueNode
{
    public readonly int Index;
    public readonly Vector2Int Position;


    public Node(int x, int y)
    {
        Index = x + (7 * y);
        Position = new Vector2Int(x, y);
    }


    public Node(Vector2Int position) : this(position.x, position.y) { }


    public override string ToString()
    {
        return $"{Index} - ({Position.x},{Position.y})";
    }

}
