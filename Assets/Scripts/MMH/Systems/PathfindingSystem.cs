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

        List<Node> path = FindPath(new Vector2Int(0, 0), new Vector2Int(3, 3));



        string output = "Path: ";

        foreach (Node node in path)
        {
            output += $"{node}";
        }

        Debug.Log(output);
    }


    private List<Node> FindPath(Vector2Int position1, Vector2Int position2)
    {
        Node startNode = aStar.GetNode(position1);
        Node endNode = aStar.GetNode(position2);

        // Check if nodes exist first

        bool startClear = !mapSystem.GetMapData().GetCell(startNode.Position[0], startNode.Position[1]).Solid;
        bool endClear = !mapSystem.GetMapData().GetCell(endNode.Position[0], endNode.Position[1]).Solid;

        if (startClear && endClear)
        {
            return aStar.FindPath(startNode, endNode);
        }

        return new List<Node>();
    }
}
