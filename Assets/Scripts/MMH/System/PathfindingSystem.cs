using UnityEngine;
using HPAStar;

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
            aStar.Graph = new Graph(mapSystem.GetCells().Length);

            for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
            {
                for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                {
                    if (mapSystem.GetCell(x, y).Solid) continue;

                    Node node = aStar.GetNode(x, y) ?? aStar.BuildNode(x, y);

                    BuildEdges(node);
                }
            }
        }


        public Data.Path FindPath(Vector2Int start, Vector2Int end)
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

                    Vector2Int offset = new Vector2Int(x, y);

                    if (ValidEdgeLocation(targetNode.Position, offset))
                    {
                        Vector2Int neighborPosition = targetNode.Position + offset;

                        Node neighborNode = aStar.GetNode(neighborPosition) ?? aStar.BuildNode(neighborPosition);

                        float neighborDistance = Vector2Int.Distance(targetNode.Position, neighborPosition);

                        aStar.BuildEdge(targetNode, neighborNode, neighborDistance);
                    }
                }
            }
        }


        private bool ValidEdgeLocation(Vector2Int position, Vector2Int offset)
        {
            Vector2Int neighborPosition = position + offset;

            if (!Util.Map.OnMap(neighborPosition)) return false;

            if (mapSystem.GetCell(neighborPosition).Solid) return false;

            Vector2Int northPosition = position + new Vector2Int(0, 1);
            Vector2Int eastPosition = position + new Vector2Int(1, 0);
            Vector2Int southPosition = position + new Vector2Int(0, -1);
            Vector2Int westPosition = position + new Vector2Int(-1, 0);

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