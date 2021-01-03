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

        PathData pathData = FindPath(new Vector2Int(0, 0), new Vector2Int(3, 3));

        Debug.Log(pathData);
    }


    private PathData FindPath(Vector2Int position1, Vector2Int position2)
    {
        Node startNode = aStar.GetNode(position1);
        Node endNode = aStar.GetNode(position2);

        bool nodesDoNotExist = startNode is null || endNode is null;

        if (nodesDoNotExist) return new PathData { Success = false };

        bool startClear = !mapSystem.GetMapData().GetCell(startNode.Position).Solid;
        bool endClear = !mapSystem.GetMapData().GetCell(endNode.Position).Solid;

        if (startClear && endClear)
        {
            return aStar.FindPath(startNode, endNode);
        }

        return new PathData { Success = false };
    }
}
