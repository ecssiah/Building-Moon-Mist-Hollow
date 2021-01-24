using UnityEngine;
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
            List<int> solidDataList = mapSystem.GetSolidData();

            NativeArray<int> solidData = solidDataList.ToNativeArray(Allocator.TempJob);
            NativeArray<int> adjacencyData = new NativeArray<int>(solidDataList.Count, Allocator.TempJob);

            FindPathJob findPathJob = new FindPathJob
            {
                startPosition = new int2(0, 0),
                endPosition = new int2(8, 8),
                solidData = solidData,
            };

            findPathJob.Run();

            solidData.Dispose();
            adjacencyData.Dispose();
        }


        private struct FindPathJob : IJob
        {
            public int2 startPosition;
            public int2 endPosition;

            public NativeArray<int> solidData;

            public void Execute()
            {
                NativeArray<Data.Node> nativeNodeArray = new NativeArray<Data.Node>(solidData.Length, Allocator.Temp);

                for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
                {
                    for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                    {
                        Data.Node node = new Data.Node
                        {
                            Position = new int2(x, y),
                            Solid = solidData[Util.Map.CoordsToIndex(x, y)] == 1,

                            Index = Util.Map.CoordsToIndex(x, y),
                            PreviousIndex = -1,

                            GCost = int.MaxValue,
                            FCost = int.MaxValue,
                            HCost = 0,
                        };

                        nativeNodeArray[node.Index] = node;
                    }
                }

                Data.Node startNode = nativeNodeArray[Util.Map.CoordsToIndex(startPosition)];
                startNode.GCost = 0;
                startNode.FCost = startNode.GCost + startNode.HCost;

                nativeNodeArray[startNode.Index] = startNode;

                int endNodeIndex = Util.Map.CoordsToIndex(endPosition);

                NativeList<int> openList = new NativeList<int>(Allocator.Temp);
                NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

                openList.Add(startNode.Index);

                NativeArray<int2> neighborOffsets = new NativeArray<int2>(8, Allocator.Temp);
                neighborOffsets[0] = new int2(+1, +0);
                neighborOffsets[1] = new int2(+1, +1);
                neighborOffsets[2] = new int2(+0, +1);
                neighborOffsets[3] = new int2(-1, +1);
                neighborOffsets[4] = new int2(-1, +0);
                neighborOffsets[5] = new int2(-1, -1);
                neighborOffsets[6] = new int2(+0, -1);
                neighborOffsets[7] = new int2(+1, -1);

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

                    for (int i = 0; i < neighborOffsets.Length; i++)
                    {
                        int2 neighborOffset = neighborOffsets[i];
                        int2 neighborPosition = currentNode.Position + neighborOffset;

                        if (!Util.Map.OnMap(neighborPosition))
                        {
                            continue;
                        }

                        Data.Node neighborNode = nativeNodeArray[Util.Map.CoordsToIndex(neighborPosition)];

                        if (closedList.Contains(neighborNode.Index))
                        {
                            continue;
                        }

                        int tentativeGCost = currentNode.GCost + CalculateHCost(currentNode, neighborNode);

                        if (tentativeGCost < neighborNode.GCost)
                        {
                            neighborNode.PreviousIndex = currentNode.Index;

                            neighborNode.GCost = tentativeGCost;
                            neighborNode.HCost = CalculateHCost(neighborPosition, endPosition);
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
                    print("No path found");
                }
                else
                {
                    NativeList<int2> path = CalculatePath(nativeNodeArray, endNode);

                    foreach (int2 nodePosition in path)
                    {
                        print(nodePosition);
                    }
                }


                nativeNodeArray.Dispose();
                openList.Dispose();
                closedList.Dispose();
                neighborOffsets.Dispose();
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


            public int CalcuateGCost(Data.Node startNode, Data.Node endNode)
            {
                int additionalGCost = IsMovingStraight(startNode, endNode) ? Info.Path.StraightMoveCost : Info.Path.DiagonalMoveCost;

                return startNode.GCost + additionalGCost;
            }


            private bool IsMovingStraight(Data.Node startNode, Data.Node endNode)
            {
                bool xConstant = startNode.Position.x == endNode.Position.x;
                bool yConstant = startNode.Position.y == endNode.Position.y;

                return xConstant || yConstant;
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