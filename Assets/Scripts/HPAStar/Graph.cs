using System.Collections;
using System.Collections.Generic;


public class Graph
{
    public List<Node> Nodes;


    public Graph()
    {
        Nodes = new List<Node>();
    }


    public void AddNode(Node node)
    {
        Nodes.Add(node);
    }


    public Node GetNodeAt(int x, int y)
    {
        Node cellNode = Nodes.Find(node => node.Position[0] == x && node.Position[1] == y);

        return cellNode ?? new Node(x, y);
    }


    public void AddEdge(Node nodeA, Node nodeB, double weight)
    {
        bool edgeExists = nodeA.Edges.Exists(edge => {
            bool firstPairingMatches = edge.NodeA == nodeA && edge.NodeB == nodeB;
            bool secondPairingMatches = edge.NodeB == nodeA && edge.NodeB == nodeA;

            return firstPairingMatches || secondPairingMatches;
        });

        if (edgeExists == false)
        {
            Edge newEdge = new Edge
            {
                NodeA = nodeA,
                NodeB = nodeB,
                Weight = weight,
            };

            nodeA.Edges.Add(newEdge);
            nodeB.Edges.Add(newEdge);
        }
    }
}
