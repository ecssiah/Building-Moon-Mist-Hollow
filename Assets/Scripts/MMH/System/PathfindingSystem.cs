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
            List<int> edgeDataList = mapSystem.GetEdgeData();

            FindPathJob findPathJob = new FindPathJob
            {
                StartPosition = new int2(0, 0),
                EndPosition = new int2(8, 8),
            };

            findPathJob.Run();
        }


        private struct FindPathJob : IJob
        {
            public int2 StartPosition;
            public int2 EndPosition;

            public void Execute()
            {
                NativeArray<Data.Node> nativeNodeArray = new NativeArray<Data.Node>(Info.Map.Area, Allocator.Temp);

                for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
                {
                    for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                    {
                        int cellIndex = Util.Map.PositionToIndex(x, y);

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

                Data.Node startNode = nativeNodeArray[Util.Map.CoordsToIndex(StartPosition)];
                startNode.GCost = 0;
                startNode.FCost = startNode.GCost + startNode.HCost;

                nativeNodeArray[startNode.Index] = startNode;

                int endNodeIndex = Util.Map.CoordsToIndex(EndPosition);

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

                    foreach (KeyValuePair<Type.Direction, int2> keyValuePair in Info.Map.Directions)
                    {
                        _ = keyValuePair.Key;
                        int2 neighborOffset = keyValuePair.Value;

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