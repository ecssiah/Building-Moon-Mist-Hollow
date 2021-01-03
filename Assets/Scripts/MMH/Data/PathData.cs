using System.Collections.Generic;

public struct PathData
{
    public bool Success;

    public List<Node> Nodes;


    public override string ToString()
    {
        string output = $"Path: {Success}";

        foreach (Node node in Nodes)
        {
            output += $"- {node.Position}";
        }

        return output;
    }
}