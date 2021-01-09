using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


namespace HPAStar
{
    public class AStar
    {
        public Graph Graph;

        private readonly FastPriorityQueue<Node> openSet;
        private readonly Dictionary<Node, Node> previous;

        private readonly int MaxPriorityQueueNodes = 1000;


        public AStar()
        {
            openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
            previous = new Dictionary<Node, Node>(MaxPriorityQueueNodes);
        }


        public MMH.Data.Path FindPath(Node start, Node end)
        {
            openSet.Clear();
            previous.Clear();

            Reset();

            start.GScore = 0;
            openSet.Enqueue(start, start.GScore);

            while (openSet.Count > 0)
            {
                Node current = openSet.Dequeue();

                if (current == end)
                {
                    return PathFrom(end);
                }

                foreach (KeyValuePair<int, Node> keyValuePair in Graph.Neighbors(current))
                {
                    Node neighbor = keyValuePair.Value;
                    float gCost = CalcuateGCost(current, neighbor);

                    if (gCost < neighbor.GScore)
                    {
                        previous[neighbor] = current;

                        neighbor.GScore = gCost;
                        neighbor.FScore = CalculateFCost(neighbor, end);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, neighbor.FScore);
                        }
                    }
                }
            }

            return new MMH.Data.Path { Valid = false };
        }


        private MMH.Data.Path PathFrom(Node node)
        {
            MMH.Data.Path pathData = new MMH.Data.Path
            {
                Valid = true,
                Nodes = new List<Node>(),
            };

            Node current = node;

            while (previous.ContainsKey(current))
            {
                pathData.Nodes.Insert(0, current);
                current = previous[current];
            }

            pathData.Nodes.Insert(0, current);

            return pathData;
        }



        // Cost Methods

        private float CalculateHCost(Node start, Node end)
        {
            return OctileDistance(start, end);
        }


        private float CalcuateGCost(Node start, Node end)
        {
            return start.GScore + Vector2Int.Distance(start.Position, end.Position);
        }


        private float CalculateFCost(Node start, Node end)
        {
            return start.GScore + CalculateHCost(start, end);
        }


        private float OctileDistance(Node start, Node end)
        {
            Vector2Int differenceVector = end.Position - start.Position;

            int minDifference = Mathf.Min(Mathf.Abs(differenceVector.x), Mathf.Abs(differenceVector.y));
            int maxDifference = Mathf.Max(Mathf.Abs(differenceVector.x), Mathf.Abs(differenceVector.y));

            float octileDistance =
                MMH.Info.Path.HorizontalWeight * maxDifference +
                (MMH.Info.Path.DiagonalWeight - MMH.Info.Path.HorizontalWeight) * minDifference;

            return octileDistance;
        }



        // Building Methods

        public Node BuildNode(Vector2Int position)
        {
            return BuildNode(position.x, position.y);
        }


        public Node BuildNode(int x, int y)
        {
            Node node = new Node(x, y);

            Graph.AddNode(node);

            return node;
        }


        public void BuildEdge(Node start, Node end, float weight)
        {
            Graph.AddEdge(start, end, weight);
        }



        // Helper Methods


        public Node GetNode(int x, int y)
        {
            return GetNode(new Vector2Int(x, y));
        }


        public Node GetNode(Vector2Int position)
        {
            int nodeIndex = MMH.Util.Map.CoordsToIndex(position);

            if (Graph.Nodes.ContainsKey(nodeIndex))
            {
                return Graph.Nodes[nodeIndex];
            }

            return null;
        }


        private void Reset()
        {
            foreach (KeyValuePair<int, Node> keyValue in Graph.Nodes)
            {
                Node node = keyValue.Value;

                node.FScore = Mathf.Infinity;
                node.GScore = Mathf.Infinity;
            }
        }
    }
}