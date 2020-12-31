using System;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


public class Node : FastPriorityQueueNode
{
    public List<Edge> Edges;

    public Vector2Int Position;

    public Node PreviousNode;

    public float GScore;
    public float FScore;


    public Node()
    {
        Edges = new List<Edge>();

        Position = new Vector2Int(0, 0);

        PreviousNode = null;
    }


    public Node(int x, int y)
    {
        Edges = new List<Edge>();

        Position = new Vector2Int(x, y);

        PreviousNode = null;
    }


    public Edge AddEdge(Edge newEdge)
    {
        if (!Edges.Exists(edge => newEdge == edge))
        {
            Edges.Add(newEdge);
        }

        return newEdge;
    }


    public List<Tuple<Node, float>> NeighborsAndWeights()
    {
        List<Tuple<Node, float>> neighbors = new List<Tuple<Node, float>>();

        foreach (Edge edge in Edges)
        {
            if (edge.LeftNode == this) {
                neighbors.Add(Tuple.Create(edge.RightNode, edge.Weight));
            } else {
                neighbors.Add(Tuple.Create(edge.LeftNode, edge.Weight));
            }
        }

        return neighbors;
    }


    public override bool Equals(object obj)
    {
        return Equals(obj as Node);
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


    public bool Equals(Node node)
    {
        if (node is null)
        {
            return false;
        }

        if (ReferenceEquals(this, node))
        {
            return true;
        }

        if (GetType() != node.GetType())
        {
            return false;
        }

        return ValueEquals(node);
    }


    private bool ValueEquals(Node node)
    {
        bool xPosition = Position[0] == node.Position[0];
        bool yPosition = Position[1] == node.Position[1];

        return xPosition && yPosition;
    }


    public static bool operator ==(Node lhs, Node rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            return false;
        }

        return lhs.Equals(rhs);
    }


    public static bool operator !=(Node lhs, Node rhs)
    {
        return !(lhs == rhs);
    }


    public override string ToString()
    {
        string output = $"";

        output += $"Node: {Position} G: {GScore} F: {FScore}";

        return output;
    }
}
