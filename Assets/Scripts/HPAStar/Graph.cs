using System.Collections.Generic;


namespace HPAStar
{
    public class Graph
    {
        public Dictionary<int, Node> Nodes;
        public float[,] Adjacency;


        public Graph(int nodeCount)
        {
            Nodes = new Dictionary<int, Node>(nodeCount);
            Adjacency = new float[nodeCount, nodeCount];
        }


        public void AddNode(Node node)
        {
            Nodes[node.Index] = node;
        }


        public void AddEdge(Node node1, Node node2, float weight)
        {
            Adjacency[node1.Index, node2.Index] = weight;
            Adjacency[node2.Index, node1.Index] = weight;
        }


        public float GetEdge(Node node1, Node node2)
        {
            if (node1 is null || node2 is null) return 0;

            return Adjacency[node1.Index, node2.Index];
        }


        public Dictionary<int, Node> Neighbors(Node node)
        {
            Dictionary<int, Node> neighbors = new Dictionary<int, Node>();

            for (int i = 0; i < Adjacency.GetLength(1); i++)
            {
                if (Adjacency[node.Index, i] != 0)
                {
                    neighbors[i] = Nodes[i];
                }
            }

            return neighbors;
        }


        public override string ToString()
        {
            string output = $"Graph: {Nodes.Count} nodes\n";

            foreach (int key in Nodes.Keys)
            {
                output += Nodes[key];
            }

            return output;
        }
    }
}