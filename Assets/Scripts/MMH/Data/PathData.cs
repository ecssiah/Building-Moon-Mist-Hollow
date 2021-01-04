using System.Collections.Generic;

public struct PathData
{
    public bool Success;

    public List<Node> Nodes;


    public override string ToString()
    {
        string output = $"Path: {Success}";

        if (Success)
        {
            for (int i = 0; i < Nodes.Count; i++)
            { 
                output += $" - {Nodes[i].Position}";

                if (Nodes[i].Index % 16 == 0)
                {
                    output += "\n";
                }
            }
        }

        return output;
    }
}