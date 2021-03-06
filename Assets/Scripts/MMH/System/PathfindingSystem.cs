﻿using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;

namespace MMH.System
{
    public class PathfindingSystem : MonoBehaviour
    {
        private MapSystem mapSystem;


        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
        }


        private void Start()
        {
            int findPathJobCount = 5;
            NativeArray<JobHandle> jobHandleArray = new NativeArray<JobHandle>(findPathJobCount, Allocator.TempJob);

            for (int i = 0; i < findPathJobCount; i++)
            {
                FindPathJob findPathJob = new FindPathJob
                {
                    GridSize = Info.Map.Size,
                    GridWidth = Info.Map.Width,
                    StartPosition = new int2(0, 0),
                    EndPosition = new int2(19, 19),
                    Nodes = GetNodes(),
                };

                jobHandleArray[i] = findPathJob.Schedule();
            }

            JobHandle.CompleteAll(jobHandleArray);
            jobHandleArray.Dispose();
        }


        private NativeArray<Data.Node> GetNodes()
        {
            NativeArray<Data.Node> nativeNodeArray = new NativeArray<Data.Node>(Info.Map.Area, Allocator.TempJob);

            for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
            {
                for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                {
                    Data.Cell cell = mapSystem.GetCell(x, y);

                    Data.Node node = new Data.Node
                    {
                        Index = cell.Index,
                        PreviousIndex = -1,

                        FCost = int.MaxValue,
                        GCost = int.MaxValue,
                        HCost = int.MaxValue,

                        Position = cell.Position,
                        Solid = cell.Solid,
                    };

                    nativeNodeArray[node.Index] = node;
                }
            }

            return nativeNodeArray;
        }

        

        [BurstCompile]
        private struct FindPathJob : IJob
        {
            public int GridSize;
            public int GridWidth;

            public int2 StartPosition;
            public int2 EndPosition;

            [DeallocateOnJobCompletion]
            public NativeArray<Data.Node> Nodes;


            public void Execute()
            {
                // Build Graph
                for (int i = 0; i < Nodes.Length; i++)
                {
                    Data.Node node = Nodes[i];
                    node.HCost = CalculateHCost(node.Position, EndPosition);
                    node.PreviousIndex = -1;

                    Nodes[i] = node;
                }

                // Pathfinding
                NativeArray<int2> neighborOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
                neighborOffsetArray[0] = new int2(+1, +0);
                neighborOffsetArray[1] = new int2(+1, +1);
                neighborOffsetArray[2] = new int2(+0, +1);
                neighborOffsetArray[3] = new int2(-1, +1);
                neighborOffsetArray[4] = new int2(-1, +0);
                neighborOffsetArray[5] = new int2(-1, -1);
                neighborOffsetArray[6] = new int2(+0, -1);
                neighborOffsetArray[7] = new int2(+1, -1);

                Data.Node startNode = Nodes[PositionToIndex(StartPosition, GridSize)];
                startNode.GCost = 0;
                startNode.FCost = startNode.GCost + startNode.HCost;
                Nodes[startNode.Index] = startNode;

                int endNodeIndex = PositionToIndex(EndPosition, GridSize);

                NativeList<int> openList = new NativeList<int>(Allocator.Temp);
                NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

                openList.Add(startNode.Index);

                while (openList.Length > 0)
                {
                    int currentIndex = GetIndexOfMinCostNode(openList);
                    Data.Node currentNode = Nodes[currentIndex];

                    if (currentIndex == endNodeIndex)
                    {
                        break;
                    }

                    for (int i = 0; i < openList.Length; i++)
                    {
                        if (openList[i] == currentIndex)
                        {
                            openList.RemoveAtSwapBack(i);
                            break;
                        }
                    }

                    closedList.Add(currentIndex);

                    for (int i = 0; i < neighborOffsetArray.Length; i++)
                    {
                        int2 neighborPosition = currentNode.Position + neighborOffsetArray[i];

                        if (!OnGrid(neighborPosition, GridSize))
                        {
                            continue;
                        }

                        Data.Node neighborNode = Nodes[PositionToIndex(neighborPosition, GridSize)];

                        if (closedList.Contains(neighborNode.Index))
                        {
                            continue;
                        }

                        if (neighborNode.Solid)
                        {
                            continue;
                        }

                        int testNeighborGCost = currentNode.GCost + CalculateDistanceCost(currentNode.Position, neighborPosition);

                        if (testNeighborGCost < neighborNode.GCost)
                        {
                            neighborNode.PreviousIndex = currentNode.Index;

                            neighborNode.GCost = testNeighborGCost;
                            neighborNode.HCost = CalculateHCost(neighborPosition, EndPosition);
                            neighborNode.FCost = neighborNode.GCost + neighborNode.HCost;

                            Nodes[neighborNode.Index] = neighborNode;

                            if (!openList.Contains(neighborNode.Index))
                            {
                                openList.Add(neighborNode.Index);
                            }
                        }
                    }
                }

                Data.Node endNode = Nodes[endNodeIndex];

                if (endNode.PreviousIndex == -1)
                {
                    NativeList<int2> path = CalculatePath(endNode);

                    path.Dispose();
                }

                openList.Dispose();
                closedList.Dispose();
            }


            private bool OnGrid(int2 position, int gridSize)
            {
                bool xOnGrid = position.x >= -gridSize && position.x <= gridSize;
                bool yOnGrid = position.x >= -gridSize && position.y <= gridSize;

                return xOnGrid && yOnGrid;
            }


            private int PositionToIndex(int2 position, int gridSize)
            {
                int gridWidth = 2 * gridSize + 1;

                return (position.x + gridSize) + gridWidth * (position.y + gridSize);
            }


            private NativeList<int2> CalculatePath(Data.Node endNode)
            {
                if (endNode.PreviousIndex == -1)
                {
                    return new NativeList<int2>(Allocator.Temp);
                }
                else
                {
                    NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                    path.Add(endNode.Position);

                    for (Data.Node currentNode = endNode; currentNode.PreviousIndex != -1; currentNode = Nodes[currentNode.PreviousIndex])
                    {
                        Data.Node previousNode = Nodes[currentNode.PreviousIndex];
                        path.Add(previousNode.Position);
                    }

                    return path;
                }
            }


            private int GetIndexOfMinCostNode(NativeList<int> openList)
            {
                Data.Node lowestCostNode = Nodes[openList[0]];

                for (int i = 1; i < openList.Length; i++)
                {
                    Data.Node testNode = Nodes[openList[i]];

                    if (testNode.FCost < lowestCostNode.FCost)
                    {
                        lowestCostNode = testNode;
                    }
                }

                return lowestCostNode.Index;
            }


            private bool IsMovingStraight(Data.Node startNode, Data.Node endNode)
            {
                bool xConstant = startNode.Position.x == endNode.Position.x;
                bool yConstant = startNode.Position.y == endNode.Position.y;

                return xConstant || yConstant;
            }


            public int CalcuateGCost(Data.Node startNode, Data.Node endNode)
            {
                int additionalGCost = IsMovingStraight(startNode, endNode) ? Info.Path.StraightMoveCost : Info.Path.DiagonalMoveCost;

                return startNode.GCost + additionalGCost;
            }


            public int CalculateFCost(Data.Node startNode, Data.Node endNode)
            {
                return startNode.GCost + CalculateHCost(startNode, endNode);
            }


            public int CalculateHCost(Data.Node startNode, Data.Node endNode)
            {
                return CalculateDistanceCost(startNode.Position, endNode.Position);
            }


            public int CalculateHCost(int2 startPosition, int2 endPosition)
            {
                return CalculateDistanceCost(startPosition, endPosition);
            }


            private int CalculateDistanceCost(int2 startPosition, int2 endPosition)
            {
                int dx = math.abs(startPosition.x - endPosition.x);
                int dy = math.abs(startPosition.y - endPosition.y);

                int straightDistance = math.min(dx, dy);
                int diagonalDistance = math.abs(dx - dy);

                return Info.Path.StraightMoveCost * straightDistance + Info.Path.DiagonalMoveCost * diagonalDistance;
            }
        }
    }
}