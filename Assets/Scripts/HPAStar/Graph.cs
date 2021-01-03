using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Graph
{
    private List<Node> nodes;
    public List<Node> Nodes { get => nodes; }

    private float[,] adjacency;
    public float[,] Adjacency { get => adjacency; }


    public Graph(int nodeCount)
    {
        nodes = Enumerable.Repeat(new Node(), nodeCount).ToList();
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

        foreach (Node node in Nodes)
        {
            output += $"  {node}";

            if (node.Index % MapInfo.Width == MapInfo.Width - 1)
            {
                output += "\n"; 
            }
        }

        return output;
    }
}
