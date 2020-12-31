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

        ConstructGraph();


        List<Node> path = FindPath(new Vector2Int(0, 0), new Vector2Int(10, 10));

        Debug.Log("Path:");

        foreach (Node node in path)
        {
            Debug.Log(node);
        }
    }


    private void ConstructGraph()
    {
        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
            {
                if (mapSystem.GetCellData(x, y).Solid) continue;

                Node cellNode = aStar.graph.AddNode(new Node(x, y));

                ConstructEdge(cellNode, x + 0, y + 1, PathInfo.StraightMovementWeight);
                ConstructEdge(cellNode, x + 1, y + 1, PathInfo.DiagonalMovementWeight);
                ConstructEdge(cellNode, x + 1, y + 0, PathInfo.StraightMovementWeight);
                ConstructEdge(cellNode, x + 1, y - 1, PathInfo.DiagonalMovementWeight);
                ConstructEdge(cellNode, x + 0, y - 1, PathInfo.StraightMovementWeight);
                ConstructEdge(cellNode, x - 1, y - 1, PathInfo.DiagonalMovementWeight);
                ConstructEdge(cellNode, x - 1, y + 0, PathInfo.StraightMovementWeight);
                ConstructEdge(cellNode, x - 1, y + 1, PathInfo.DiagonalMovementWeight);
            }
        }
    }


    private void ConstructEdge(Node leftNode, int x, int y, float weight)
    {
        if (MapUtil.OnMap(x, y) && mapSystem.GetCellData(x, y).Solid == false)
        {
            Node rightNode = aStar.graph.GetNodeAt(x, y);

            Edge edge = new Edge
            {
                LeftNode = leftNode,
                RightNode = rightNode,
                Weight = weight,
            };

            aStar.graph.AddEdge(edge);
        }
    }


    private List<Node> FindPath(Vector2Int position1, Vector2Int position2)
    {
        Node startNode = aStar.GetNodeAt(position1);
        Node endNode = aStar.GetNodeAt(position2);

        bool startClear = !mapSystem.GetCellData(startNode.Position[0], startNode.Position[1]).Solid;
        bool endClear = !mapSystem.GetCellData(endNode.Position[0], endNode.Position[1]).Solid;

        if (startClear && endClear)
        {
            return aStar.FindPath(startNode, endNode);
        }

        return new List<Node>();
    }
}
