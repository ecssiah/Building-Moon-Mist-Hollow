using UnityEngine;

public class PathfindingSystem : MonoBehaviour
{
    private AStar aStar;
    private WorldMap worldMap;


    void Awake()
    {
        worldMap = GameObject.Find("MapSystem").GetComponent<WorldMap>();
    }


    void Start()
    {
        aStar = new AStar();

        BuildGraph();
    }


    public void BuildGraph()
    {
        aStar.Graph = new Graph(worldMap.Cells.Length);

        for (int x = -worldMap.Size; x <= worldMap.Size; x++)
        {
            for (int y = -worldMap.Size; y <= worldMap.Size; y++)
            {
                if (worldMap.GetCell(x, y).Solid) continue;

                Node node = aStar.GetNode(x, y) ?? aStar.BuildNode(x, y);

                BuildEdges(node);
            }
        }
    }


    public PathData FindPath(Vector2Int position1, Vector2Int position2)
    {
        if (!MapUtil.OnMap(position1) || !MapUtil.OnMap(position2))
        {
            return new PathData { Valid = false };
        }

        Node startNode = aStar.GetNode(position1);
        Node endNode = aStar.GetNode(position2);

        if (startNode is null || endNode is null)
        {
            return new PathData { Valid = false };
        }

        CellData startCell = worldMap.GetCell(startNode.Position);
        CellData endCell = worldMap.GetCell(endNode.Position);

        if (startCell.Solid || endCell.Solid)
        {
            return new PathData { Valid = false };
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

        if (!MapUtil.OnMap(neighborPosition)) return false;

        if (worldMap.GetCell(neighborPosition).Solid) return false;

        Vector2Int northPosition = position + new Vector2Int(0, 1);
        Vector2Int eastPosition = position + new Vector2Int(1, 0);
        Vector2Int southPosition = position + new Vector2Int(0, -1);
        Vector2Int westPosition = position + new Vector2Int(-1, 0);

        bool northSolid = !MapUtil.OnMap(northPosition) || worldMap.GetCell(northPosition).Solid;
        bool eastSolid = !MapUtil.OnMap(eastPosition) || worldMap.GetCell(eastPosition).Solid;
        bool southSolid = !MapUtil.OnMap(southPosition) || worldMap.GetCell(southPosition).Solid;
        bool westSolid = !MapUtil.OnMap(westPosition) || worldMap.GetCell(westPosition).Solid;

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
