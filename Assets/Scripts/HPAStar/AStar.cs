using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace HPAStar
{
    public class AStar
    {
        public Graph Graph;

        private readonly FastPriorityQueue<Node> openSet;
        private readonly List<Node> closedSet;

        private readonly int MaxPriorityQueueNodes = 1000;


        public AStar()
        {
            openSet = new FastPriorityQueue<Node>(MaxPriorityQueueNodes);
            closedSet = new List<Node>(MaxPriorityQueueNodes);
        }


        public MMH.Data.Path FindPath(Node start, Node end)
        {
            openSet.Clear();
            closedSet.Clear();

            openSet.Enqueue(start, CalculateFCost(start, end));

            int timer = 0;
            int timeout = 1000;


            while ((openSet.First != end) && (++timer < timeout))
            {
                Node targetNode = openSet.Dequeue();
                closedSet.Add(targetNode);

                foreach (KeyValuePair<int, Node> keyValuePair in Graph.Neighbors(targetNode))
                {
                    Node neighbor = keyValuePair.Value;

                    float gCost = CalcuateGCost(targetNode, neighbor);

                    if (openSet.Contains(neighbor) && gCost < neighbor.GScore)
                    {
                        openSet.Remove(neighbor);
                    }

                    if (closedSet.Contains(neighbor) && gCost < neighbor.GScore)
                    {
                        closedSet.Remove(neighbor);
                    }

                    if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                    {
                        neighbor.GScore = gCost;
                        neighbor.FScore = CalculateFCost(neighbor, end);
                        neighbor.Previous = targetNode;

                        openSet.Enqueue(neighbor, neighbor.FScore);
                    }
                }
            }

            return PathFrom(end);
        }


        private MMH.Data.Path PathFrom(Node node)
        {
            MMH.Data.Path pathData = new MMH.Data.Path
            {
                Valid = true,
                Nodes = new List<Node>(),
            };

            for (Node current = node; current != null; current = current.Previous)
            {
                pathData.Nodes.Insert(0, current);
            }

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
            Node node = new Node
            {
                Index = MMH.Util.Map.CoordsToIndex(x, y),
                Position = new Vector2Int(x, y),
                GScore = 0,
                FScore = 0,
                Previous = null,
            };

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
    }
}