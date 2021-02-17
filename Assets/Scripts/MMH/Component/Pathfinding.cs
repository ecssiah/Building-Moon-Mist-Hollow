using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Collections;

namespace MMH.Component
{
    public class Pathfinding : MonoBehaviour
    {
        private System.MapSystem mapSystem;

        private Data.Cell[] cellsData;
        private int[] edgeData;

        private Data.Node[] nodes;

        private List<int> openList;
        private List<int> closedList;


        void Awake()
        {
            mapSystem = GameObject.Find("Map System").GetComponent<System.MapSystem>();

            nodes = new Data.Node[Info.Map.Area];

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

                    Visited = false,

                    GCost = int.MaxValue,
                    HCost = int.MaxValue,
                    FCost = int.MaxValue,

                    Position = cellData.Position,
                    Solid = cellData.Solid,
                };

                nodes[i] = node;
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

            StartCoroutine(Search(startNode, endNode));

            return CalculatePath(startNode, endNode);
        }


        private IEnumerator Search(Data.Node startNode, Data.Node endNode)
        {
            int iteration = 0;

            openList.Clear();
            openList.Add(startNode.Index);

            foreach (Data.Node node in nodes)
            {
                node.Visited = false;
            }

            startNode.PreviousIndex = -1;

            startNode.GCost = 0;
            startNode.HCost = CalculateHCost(startNode, endNode);
            startNode.FCost = startNode.GCost + startNode.HCost;

            while (openList.Count > 0)
            {
                Data.Node currentNode = GetNodeWithLowestFCost();

                print(currentNode.Position);
                print(currentNode.FCost);

                openList.RemoveAll(node => node == currentNode.Index);
                currentNode.Visited = true;

                if (currentNode == endNode)
                {
                    break;
                }

                foreach (Type.Direction neighborDirection in Info.Map.DirectionOffsets.Keys)
                {
                    int2 neighborPosition = currentNode.Position + Info.Map.DirectionOffsets[neighborDirection];

                    if (!Util.Map.OnMap(neighborPosition))
                    {
                        continue;
                    }

                    int neighborIndex = Util.Map.PositionToIndex(neighborPosition);
                    Data.Node neighborNode = nodes[neighborIndex];

                    if (neighborNode.Visited)
                    {
                        continue;
                    }

                    int edgeIndex = Util.Map.EdgeToIndex(currentNode.Index, neighborNode.Index);

                    if (edgeData[edgeIndex] == 0)
                    {
                        closedList.Add(neighborIndex);
                        continue;
                    }

                    int gCost = currentNode.GCost + edgeData[edgeIndex];

                    bool neighborNotInOpenList = !openList.Contains(neighborNode.Index);

                    if (neighborNotInOpenList || gCost < neighborNode.GCost)
                    {
                        neighborNode.PreviousIndex = currentNode.Index;

                        neighborNode.GCost = gCost;
                        neighborNode.HCost = CalculateHCost(neighborNode, endNode);
                        neighborNode.FCost = neighborNode.GCost + neighborNode.HCost;

                        if (neighborNotInOpenList)
                        {
                            openList.Add(neighborNode.Index);
                        }
                    }
                }

                if (iteration++ > Info.Path.SearchIterationsPerFrame)
                {
                    yield return null;
                }
            }
        }


        private Data.Path CalculatePath(Data.Node startNode, Data.Node endNode)
        {
            Data.Path pathData = new Data.Path
            {
                Index = 0,
                StepProgress = 0,
                Positions = new List<int2>(),
            };

            if (endNode.PreviousIndex != -1)
            {
                Data.Node currentNode = endNode;

                while (currentNode.PreviousIndex != -1)
                {
                    pathData.Positions.Insert(0, currentNode.Position);

                    currentNode = nodes[currentNode.PreviousIndex];
                }
            }

            pathData.Positions.Insert(0, startNode.Position);

            return pathData;
        }


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