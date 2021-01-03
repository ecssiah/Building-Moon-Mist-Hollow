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

        PathData path = FindPath(new Vector2Int(0, 0), new Vector2Int(3, 3));

        if (path.Success)
        {
            string output = "Path: ";

            foreach (Node node in path.Nodes)
            {
                output += $"{node}";
            }

            Debug.Log(output);
        }
    }


    private PathData FindPath(Vector2Int position1, Vector2Int position2)
    {
        Node startNode = aStar.GetNode(position1);
        Node endNode = aStar.GetNode(position2);

        bool nodesExist = startNode != null && endNode != null;
        bool startClear = !mapSystem.GetMapData().GetCell(startNode.Position[0], startNode.Position[1]).Solid;
        bool endClear = !mapSystem.GetMapData().GetCell(endNode.Position[0], endNode.Position[1]).Solid;

        if (nodesExist && startClear && endClear)
        {
            return aStar.FindPath(startNode, endNode);
        }

        return new PathData { Success = false };
    }
}
