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
        aStar = new AStar(BuildGraph());
    }


    private Graph BuildGraph()
    {
        Graph graph = new Graph();

        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
            {
                if (mapSystem.GetCellData(x, y).solid) continue;

                Node cellNode = new Node(x, y);

                graph.AddNode(cellNode);

                if (MapUtil.OnMap(x, y + 1) && !mapSystem.GetCellData(x, y + 1).solid)
                {
                    Node northNode = graph.GetNodeAt(x, y + 1);

                    graph.AddEdge(cellNode, northNode, PathInfo.StraightMovementWeight);
                }

                if (MapUtil.OnMap(x + 1, y + 1) && !mapSystem.GetCellData(x + 1, y + 1).solid)
                {
                    Node northEastNode = graph.GetNodeAt(x + 1, y + 1);

                    graph.AddEdge(cellNode, northEastNode, PathInfo.DiagonalMovementWeight);

                }

                if (MapUtil.OnMap(x + 1, y) && !mapSystem.GetCellData(x + 1, y).solid)
                {
                    Node eastNode = graph.GetNodeAt(x + 1, y);

                    graph.AddEdge(cellNode, eastNode, PathInfo.StraightMovementWeight);
                }
            }
        }

        return graph;
    }
}
