using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using Unity.Mathematics;

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
                    int gCost = CalcuateGCost(current, neighbor);

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

            return new MMH.Data.Path();
        }


        private MMH.Data.Path PathFrom(Node node)
        {
            MMH.Data.Path pathData = new MMH.Data.Path
            {
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

        private int CalcuateGCost(Node start, Node end)
        {
            bool horizontalMove = (start.Position.x == end.Position.x) || (start.Position.y == end.Position.y);

            return start.GScore + (horizontalMove ? 10 : 14);
        }


        private int CalculateFCost(Node start, Node end)
        {
            return start.GScore + (int)(10 * CalculateHCost(start, end));
        }


        private int CalculateHCost(Node start, Node end)
        {
            return CalculateDistanceCost(start, end);
        }


        private int CalculateDistanceCost(Node start, Node end)
        {
            int dx = math.abs(start.Position.x - end.Position.x);
            int dy = math.abs(start.Position.y - end.Position.y);

            int remainder = math.abs(dx - dy);

            return MMH.Info.Path.DiagonalWeight * math.min(dx, dy) + MMH.Info.Path.HorizontalWeight * remainder;
        }


        // Building Methods

        public Node BuildNode(int2 position)
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
            return GetNode(new int2(x, y));
        }


        public Node GetNode(int2 position)
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

                node.FScore = int.MaxValue;
                node.GScore = int.MaxValue;
            }
        }
    }
}