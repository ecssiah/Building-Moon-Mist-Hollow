using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

namespace MMH.System
{
    public class PathfindingSystem : MonoBehaviour
    {
        private MapSystem mapSystem;

        private List<Data.Node> nodes;

        private Data.Cell[] cellsData;
        private int[] edgeData;

        private List<int> openList;
        private List<int> closedList;


        void Awake()
        {
            mapSystem = GameObject.Find("Map System").GetComponent<MapSystem>();

            nodes = new List<Data.Node>(Info.Map.Area);

            openList = new List<int>();
            closedList = new List<int>();
        }


        void Start()
        {
            cellsData = mapSystem.Map.GetCells();
            edgeData = mapSystem.Map.GetEdgeData();

            BuildGraph();
        }


        public void BuildGraph()
        {
            for (int i = 0; i < cellsData.Length; i++)
            {
                Data.Cell cellData = cellsData[i];

                Data.Node node = new Data.Node
                {
                    Index = i,
                    PreviousIndex = -1,

                    GCost = int.MaxValue,
                    HCost = int.MaxValue,
                    FCost = int.MaxValue,

                    Position = cellData.Position,
                    Solid = cellData.Solid,
                };

                nodes.Add(node);
            }
        }


        public Data.Path FindPath(int2 startPosition, int2 endPosition)
        {
            if (!Util.Map.OnMap(startPosition) || !Util.Map.OnMap(endPosition))
            {
                return new Data.Path();
            }

            Data.Node startNode = nodes[Util.Map.PositionToIndex(startPosition)];
            Data.Node endNode = nodes[Util.Map.PositionToIndex(endPosition)];

            if (startNode.Solid || endNode.Solid)
            {
                return new Data.Path();
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                Data.Node node = nodes[i];
                node.HCost = CalculateHCost(node, endNode);
            }

            openList.Clear();
            closedList.Clear();

            startNode.GCost = 0;
            startNode.FCost = startNode.GCost + startNode.HCost;

            openList.Add(startNode.Index);

            while (openList.Count > 0)
            {
                Data.Node currentNode = GetNodeWithLowestFCost();

                if (currentNode == endNode) break;

                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i] == currentNode.Index)
                    {
                        openList.RemoveAt(i);
                        break;
                    }
                }

                closedList.Add(currentNode.Index);

                foreach (Type.Direction neighborDirection in Info.Map.DirectionVectors.Keys)
                {
                    int2 neighborPosition = currentNode.Position + Info.Map.DirectionVectors[neighborDirection];

                    if (!Util.Map.OnMap(neighborPosition)) continue;

                    Data.Node neighborNode = nodes[Util.Map.PositionToIndex(neighborPosition)];

                    if (neighborNode.Solid) continue;

                    int edgeIndex = Util.Map.EdgeToIndex(currentNode.Index, neighborNode.Index);

                    if (edgeData[edgeIndex] == 0) continue;

                    int gCost = currentNode.GCost + edgeData[edgeIndex];

                    if (gCost < neighborNode.GCost)
                    {
                        neighborNode.GCost = gCost;
                        neighborNode.FCost = neighborNode.GCost + neighborNode.HCost;

                        neighborNode.PreviousIndex = currentNode.Index;

                        if (!openList.Contains(neighborNode.Index))
                        {
                            openList.Add(neighborNode.Index);
                        }
                    }
                }
            }

            return CalculatePath(endNode);
        }


        private Data.Path CalculatePath(Data.Node endNode)
        {
            Data.Path pathData = new Data.Path
            {
                Index = 0,
                Progress = 0,
                Positions = new List<int2>(),
            };

            if (endNode.PreviousIndex != -1)
            {
                Data.Node currentNode = endNode;

                while (currentNode.PreviousIndex != -1)
                {
                    pathData.Positions.Add(currentNode.Position);

                    currentNode = nodes[currentNode.PreviousIndex];
                }

                pathData.Positions.Add(currentNode.Position);
            }

            pathData.Positions.Reverse();

            return pathData;
        }


        // Cost Methods

        private Data.Node GetNodeWithLowestFCost()
        {
            Data.Node lowestFCostNode = nodes[openList[0]];

            for (int i = 1; i < openList.Count; i++)
            {
                Data.Node testNode = nodes[openList[i]];

                if (testNode.FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = testNode; 
                }
            }

            return lowestFCostNode;
        }


        private int CalculateHCost(Data.Node node1, Data.Node node2)
        {
            return OctileDistance(node1, node2);
        }


        private int OctileDistance(Data.Node node1, Data.Node node2)
        {
            int dx = math.abs(node1.Position.x - node2.Position.x);
            int dy = math.abs(node1.Position.y - node2.Position.y);

            int straightDistance = math.min(dx, dy);
            int diagonalDistance = math.abs(dx - dy);

            return Info.Map.StraightMovementCost * straightDistance + Info.Map.DiagonalMovementCost * diagonalDistance;
        }
    }
}