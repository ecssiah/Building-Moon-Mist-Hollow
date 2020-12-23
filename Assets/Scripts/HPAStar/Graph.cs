using System.Collections;
using System.Collections.Generic;


public class Graph
{
    public List<Node> Nodes;


    public Graph()
    {
        Nodes = new List<Node>();
    }


    public Node AddNode()
    {
        Node node = new Node();

        Nodes.Add(node);

        return node;

        //return Nodes[Nodes.IndexOf(node)];
    }


    public void AddEdge(Node node1, Node node2, double weight)
    {
        Edge newEdge = new Edge
        {
            node1 = node1,
            node2 = node2,
            weight = weight,
        };

        node1.Edges.Add(newEdge);
        node2.Edges.Add(newEdge);
    }
}
