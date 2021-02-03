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
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

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
            for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
            {
                for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                {
                    Data.Cell cellData = cellsData[Util.Map.PositionToIndex(x, y)];

                    Data.Node node = new Data.Node
                    {
                        Index = cellData.Index,
                        PreviousNodeIndex = -1,

                        GCost = int.MaxValue,
                        HCost = int.MaxValue,
                        FCost = int.MaxValue,

                        Position = cellData.Position,
                        Solid = cellData.Solid,
                    };

                    nodes.Add(node);
                }
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

            int2[] neighborOffsets = new int2[]
            {
                new int2(+1, +0),
                new int2(+1, +1),
                new int2(+0, +1),
                new int2(-1, +1),
                new int2(-1, +0),
                new int2(-1, -1),
                new int2(+0, -1),
                new int2(+1, -1),
            };

            int testCount = 0;

            //while (openList.Count > 0)
            while (testCount < 2)
            {
                testCount++;

                Data.Node currentNode = GetNodeWithLowestFCost();

                print($"Current: {currentNode.Position}");

                if (currentNode == endNode)
                {
                    break;
                }

                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i] == currentNode.Index)
                    {
                        openList.RemoveAt(i);
                        break;
                    }
                }

                closedList.Add(currentNode.Index);

                for (int i = 0; i < neighborOffsets.Length; i++)
                {
                    int2 neighborPosition = currentNode.Position + neighborOffsets[i];

                    if (!Util.Map.OnMap(neighborPosition)) continue;

                    int neighborIndex = Util.Map.PositionToIndex(neighborPosition);
                    Data.Node neighborNode = nodes[neighborIndex];

                    if (closedList.Contains(neighborNode.Index)) continue;

                    if (startNode.Solid || endNode.Solid) continue;

                    print($"Neighbor: {neighborNode.Position}");

                    int gCost = CalcuateGCost(currentNode, neighborNode);

                    if (gCost < neighborNode.GCost)
                    {
                        neighborNode.PreviousNodeIndex = currentNode.Index;

                        neighborNode.GCost = gCost;
                        neighborNode.HCost = CalculateHCost(neighborNode, endNode);
                        neighborNode.FCost = neighborNode.GCost + neighborNode.HCost;

                        if (!openList.Contains(neighborNode.Index))
                        {
                            openList.Add(neighborNode.Index);
                        }
                    }
                }
            }

            if (endNode.PreviousNodeIndex != -1)
            {
                List<int2> path = CalculatePath(endNode);

                foreach (int2 position in path)
                {
                    print(position);
                }

                return new Data.Path
                {
                    Index = 0,
                    Progress = 0f,
                    Positions = path,
                };
            }
            else
            {
                return new Data.Path();
            }
        }


        private List<int2> CalculatePath(Data.Node endNode)
        {
            if (endNode.PreviousNodeIndex == -1)
            {
                return new List<int2>();
            }
            else
            {
                List<int2> path = new List<int2>();

                Data.Node currentNode = endNode;

                while (currentNode.PreviousNodeIndex != -1)
                {
                    path.Add(currentNode.Position);

                    currentNode = nodes[currentNode.PreviousNodeIndex];
                }

                return path;
            }
        }


        private List<Data.Node> GetNeighbors(Data.Node node)
        {
            List<Data.Node> neighbors = new List<Data.Node>(9);

            for (int x = node.Position.x - 1; x <= node.Position.x + 1; x++)
            {
                for (int y = node.Position.y - 1; y <= node.Position.y + 1; y++)
                {
                    if (!Util.Map.OnMap(x, y)) continue;

                    int neighborIndex = Util.Map.PositionToIndex(x, y);
                    Data.Node neighborNode = nodes[neighborIndex];

                    if (neighborNode.Solid) continue;

                    int edgeIndex = Util.Map.EdgeToIndex(node.Index, neighborIndex);

                    if (edgeData[edgeIndex] == 0) continue;

                    neighbors.Add(neighborNode);
                }
            }

            return neighbors;
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


        private int CalcuateGCost(Data.Node node1, Data.Node node2)
        {
            bool horizontalMove = (node1.Position.x == node2.Position.x) || (node1.Position.y == node2.Position.y);

            return node1.GCost + (horizontalMove ? Info.Map.StraightMovementCost : Info.Map.DiagonalMovementCost);
        }


        private int CalculateHCost(Data.Node node1, Data.Node node2)
        {
            return OctileDistance(node1, node2);
        }


        private int OctileDistance(Data.Node start, Data.Node end)
        {
            int dx = math.abs(start.Position.x - end.Position.x);
            int dy = math.abs(start.Position.y - end.Position.y);

            int straightDistance = math.min(dx, dy);
            int diagonalDistance = math.abs(dx - dy);

            return Info.Map.StraightMovementCost * straightDistance + Info.Map.DiagonalMovementCost * diagonalDistance;
        }
    }
}