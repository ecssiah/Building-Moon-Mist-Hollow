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
        aStar.BuildGraph(worldMap);

        PathData path1 = FindPath(new Vector2Int(0, 0), new Vector2Int( 4,  4));
        Debug.Log(path1);

        PathData path2 = FindPath(new Vector2Int(0, 0), new Vector2Int(-4,  4));
        Debug.Log(path2);

        PathData path3 = FindPath(new Vector2Int(0, 0), new Vector2Int( 4, -4));
        Debug.Log(path3);

        PathData path4 = FindPath(new Vector2Int(0, 0), new Vector2Int(-4, -4));
        Debug.Log(path4);
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
}
