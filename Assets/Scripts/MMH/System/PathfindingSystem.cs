using UnityEngine;
using HPAStar;
using Unity.Mathematics;
using System.Collections.Generic;

namespace MMH.System
{
    public class PathfindingSystem : MonoBehaviour
    {
        private MapSystem mapSystem;

        private AStar aStar;


        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            aStar = new AStar();
        }


        void Start()
        {
            BuildGraph();
        }


        public void BuildGraph()
        {
            List<int> edgeData = mapSystem.GetEdgeData();

            aStar.Graph = new Graph(Info.Map.Area);

            for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
            {
                for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                {
                    int edgeIndex = x + Info.Map.Area * y;

                    if (edgeData[edgeIndex] != 0) continue;

                    Node node = aStar.GetNode(x, y) ?? aStar.BuildNode(x, y);

                    BuildEdges(node);
                }
            }
        }


        public Data.Path FindPath(int2 start, int2 end)
        {
            if (!Util.Map.OnMap(start) || !Util.Map.OnMap(end))
            {
                return new Data.Path();
            }

            Node startNode = aStar.GetNode(start);
            Node endNode = aStar.GetNode(end);

            if (startNode is null || endNode is null)
            {
                return new Data.Path();
            }

            Data.Cell startCell = mapSystem.GetCell(startNode.Position);
            Data.Cell endCell = mapSystem.GetCell(endNode.Position);

            if (startCell.Solid || endCell.Solid)
            {
                return new Data.Path();
            }

            return aStar.FindPath(startNode, endNode);
        }


        public void BuildEdges(Node targetNode)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int2 offset = new int2(x, y);

                    if (ValidEdgeLocation(targetNode.Position, offset))
                    {
                        int2 neighborPosition = targetNode.Position + offset;

                        Node neighborNode = aStar.GetNode(neighborPosition) ?? aStar.BuildNode(neighborPosition);

                        float neighborDistance = math.distance(targetNode.Position, neighborPosition);

                        aStar.BuildEdge(targetNode, neighborNode, neighborDistance);
                    }
                }
            }
        }


        private bool ValidEdgeLocation(int2 position, int2 offset)
        {
            int2 neighborPosition = position + offset;

            if (!Util.Map.OnMap(neighborPosition)) return false;

            if (mapSystem.GetCell(neighborPosition).Solid) return false;

            int2 northPosition = position + new int2(0, 1);
            int2 eastPosition = position + new int2(1, 0);
            int2 southPosition = position + new int2(0, -1);
            int2 westPosition = position + new int2(-1, 0);

            bool northSolid = !Util.Map.OnMap(northPosition) || mapSystem.GetCell(northPosition).Solid;
            bool eastSolid = !Util.Map.OnMap(eastPosition) || mapSystem.GetCell(eastPosition).Solid;
            bool southSolid = !Util.Map.OnMap(southPosition) || mapSystem.GetCell(southPosition).Solid;
            bool westSolid = !Util.Map.OnMap(westPosition) || mapSystem.GetCell(westPosition).Solid;

            if (offset.x == 1 && offset.y == 1)
            {
                if (northSolid || eastSolid) return false;
            }

            if (offset.x == 1 && offset.y == -1)
            {
                if (southSolid || eastSolid) return false;
            }

            if (offset.x == -1 && offset.y == 1)
            {
                if (westSolid || northSolid) return false;
            }

            if (offset.x == -1 && offset.y == -1)
            {
                if (southSolid || westSolid) return false;
            }

            return true;
        }
    }
}