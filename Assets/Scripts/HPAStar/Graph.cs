using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Graph
{
    private Dictionary<int, Node> nodes;
    public Dictionary<int, Node> Nodes { get => nodes; }

    private float[,] adjacency;
    public float[,] Adjacency { get => adjacency; }


    public Graph(int nodeCount)
    {
        nodes = new Dictionary<int, Node>(nodeCount);
        adjacency = new float[nodeCount, nodeCount];
    }


    public void AddNode(Node node)
    {
        nodes[node.Index] = node;
    }


    public void AddEdge(Node node1, Node node2, float weight)
    {
        adjacency[node1.Index, node2.Index] = weight;
        adjacency[node2.Index, node1.Index] = weight;
    }


    public float GetEdge(Node node1, Node node2)
    {
        if (node1 is null || node2 is null) return 0;

        return adjacency[node1.Index, node2.Index];
    }


    public List<Node> Neighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int index = 0; index < adjacency.GetLength(1); index++)
        {
            if (adjacency[node.Index, index] != 0)
            {
                neighbors.Add(nodes[index]);
            }
        }

        return neighbors;
    }


    public override string ToString()
    {
        string output = $"Graph: {Nodes.Count} nodes\n";

        foreach (var key in Nodes.Keys)
        {
            output += $"  {Nodes[key]}";

            if (Nodes[key].Index % MapInfo.Width == MapInfo.Width - 1)
            {
                output += "\n"; 
            }
        }

        return output;
    }
}
