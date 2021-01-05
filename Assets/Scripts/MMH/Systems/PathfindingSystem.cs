using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingSystem : MonoBehaviour
{
    private AStar aStar;

    private MapSystem mapSystem;


    void Awake()
    {
        mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
    }


    void Start()
    {
        aStar = new AStar();
        aStar.BuildGraph(mapSystem.GetMapData());

        PathData path1 = FindPath(new Vector2Int(0, 0), new Vector2Int( 4,  4));
        Debug.Log(path1);

        PathData path2 = FindPath(new Vector2Int(0, 0), new Vector2Int(-4,  4));
        Debug.Log(path2);

        PathData path3 = FindPath(new Vector2Int(0, 0), new Vector2Int( 4, -4));
        Debug.Log(path3);

        PathData path4 = FindPath(new Vector2Int(0, 0), new Vector2Int(-4, -4));
        Debug.Log(path4);
    }


    private PathData FindPath(Vector2Int position1, Vector2Int position2)
    {
        Node startNode = aStar.GetNode(position1);
        Node endNode = aStar.GetNode(position2);

        if (startNode is null || endNode is null) return new PathData { Valid = false };

        bool startSolid = mapSystem.GetMapData().GetCell(startNode.Position).Solid;
        bool endSolid = mapSystem.GetMapData().GetCell(endNode.Position).Solid;

        if (startSolid || endSolid) return new PathData { Valid = false };

        return aStar.FindPath(startNode, endNode);
    }
}
