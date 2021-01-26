using UnityEngine;
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
                    StartPosition = new int2(0, 0),
                    EndPosition = new int2(19, 19),
                };

                jobHandleArray[i] = findPathJob.Schedule();
            }

            JobHandle.CompleteAll(jobHandleArray);
            jobHandleArray.Dispose();
        }


        [BurstCompile]
        private struct FindPathJob : IJob
        {
            public int2 StartPosition;
            public int2 EndPosition;

            public void Execute()
            {
                // Build Graph
                int gridSize = 100;
                int gridWidth = 2 * gridSize + 1;

                NativeArray<Data.Node> nativeNodeArray = new NativeArray<Data.Node>(gridWidth * gridWidth, Allocator.Temp);

                for (int x = -gridSize; x <= gridSize; x++)
                {
                    for (int y = -gridSize; y <= gridSize; y++)
                    {
                        int cellIndex = PositionToIndex(new int2(x, y), gridSize);

                        Data.Node node = new Data.Node
                        {
                            Position = new int2(x, y),
                            Solid = false,

                            Index = cellIndex,
                            PreviousIndex = -1,

                            GCost = int.MaxValue,
                            FCost = int.MaxValue,
                            HCost = 0,
                        };

                        nativeNodeArray[node.Index] = node;
                    }
                }

                {
                    int position1Index = PositionToIndex(new int2(1, 0), gridSize);
                    int position2Index = PositionToIndex(new int2(1, 1), gridSize);
                    int position3Index = PositionToIndex(new int2(1, 2), gridSize);

                    Data.Node node = nativeNodeArray[position1Index];
                    node.Solid = true;
                    nativeNodeArray[position1Index] = node;

                    node = nativeNodeArray[position2Index];
                    node.Solid = true;
                    nativeNodeArray[position2Index] = node;

                    node = nativeNodeArray[position3Index];
                    node.Solid = true;
                    nativeNodeArray[position3Index] = node;
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

                Data.Node startNode = nativeNodeArray[PositionToIndex(StartPosition, gridSize)];
                startNode.GCost = 0;
                startNode.FCost = startNode.GCost + startNode.HCost;
                nativeNodeArray[startNode.Index] = startNode;

                int endNodeIndex = PositionToIndex(EndPosition, gridSize);

                NativeList<int> openList = new NativeList<int>(Allocator.Temp);
                NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

                openList.Add(startNode.Index);

                while (openList.Length > 0)
                {
                    int currentIndex = GetIndexOfMinCostNode(openList, nativeNodeArray);
                    Data.Node currentNode = nativeNodeArray[currentIndex];

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

                        if (!OnGrid(neighborPosition, gridSize))
                        {
                            continue;
                        }

                        Data.Node neighborNode = nativeNodeArray[PositionToIndex(neighborPosition, gridSize)];

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

                            nativeNodeArray[neighborNode.Index] = neighborNode;

                            if (!openList.Contains(neighborNode.Index))
                            {
                                openList.Add(neighborNode.Index);
                            }
                        }
                    }
                }

                Data.Node endNode = nativeNodeArray[endNodeIndex];

                if (endNode.PreviousIndex == -1)
                {
                    NativeList<int2> path = CalculatePath(nativeNodeArray, endNode);

                    path.Dispose();
                }

                openList.Dispose();
                closedList.Dispose();

                nativeNodeArray.Dispose();
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


            private NativeList<int2> CalculatePath(NativeArray<Data.Node> nativeNodeArray, Data.Node endNode)
            {
                if (endNode.PreviousIndex == -1)
                {
                    return new NativeList<int2>(Allocator.Temp);
                }
                else
                {
                    NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                    path.Add(endNode.Position);

                    for (Data.Node currentNode = endNode; currentNode.PreviousIndex != -1; currentNode = nativeNodeArray[currentNode.PreviousIndex])
                    {
                        Data.Node previousNode = nativeNodeArray[currentNode.PreviousIndex];
                        path.Add(previousNode.Position);
                    }

                    return path;
                }
            }


            private int GetIndexOfMinCostNode(NativeList<int> openList, NativeArray<Data.Node> nativeNodeArray)
            {
                Data.Node lowestCostNode = nativeNodeArray[openList[0]];

                for (int i = 1; i < openList.Length; i++)
                {
                    Data.Node testNode = nativeNodeArray[openList[i]];

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