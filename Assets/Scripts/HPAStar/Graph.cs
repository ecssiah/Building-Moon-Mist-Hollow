using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Graph
{
    public List<Node> Nodes;


    public Graph()
    {
        Nodes = new List<Node>();
    }


    public Node AddNode(Node newNode)
    {
        if (Nodes.Exists(node => newNode == node))
        {
            Debug.Log($"Node exists: ");
            Debug.Log(newNode);
        }
        else
        {
            Nodes.Add(newNode);
        }

        return newNode;
    }


    public void AddEdge(Edge newEdge)
    {
        bool edgeExistsOnLeftNode = newEdge.LeftNode.Edges.Exists(edge => newEdge == edge);
        bool edgeExistsOnRightNode = newEdge.RightNode.Edges.Exists(edge => newEdge == edge);

        if (edgeExistsOnLeftNode && edgeExistsOnRightNode)
        {
            Debug.Log("Edge exists:");
            Debug.Log(newEdge);
        }
        else if (edgeExistsOnLeftNode)
        {
            Debug.Log("Edge already exists on left node, adding right node reference");
            Debug.Log(newEdge);

            newEdge.RightNode.AddEdge(newEdge);
        }
        else if (edgeExistsOnRightNode)
        {
            Debug.Log("Edge already exists on right node, adding left node reference");
            Debug.Log(newEdge);

            newEdge.LeftNode.AddEdge(newEdge);
        }
        else
        {
            newEdge.LeftNode.AddEdge(newEdge);
            newEdge.RightNode.AddEdge(newEdge);
        }
    }


    public Node GetNodeAt(int x, int y)
    {
        Node cellNode = Nodes.Find(
            node => node.Position.x == x && node.Position.y == y
        );

        return cellNode ?? new Node(x, y);
    }
}
