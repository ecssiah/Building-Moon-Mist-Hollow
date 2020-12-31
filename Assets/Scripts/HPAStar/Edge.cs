public class Edge
{
    public Node LeftNode;
    public Node RightNode;

    public float Weight;


    public override bool Equals(object obj)
    {
        return Equals(obj as Edge);
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


    public bool Equals(Edge edge)
    {
        if (edge is null)
        {
            return false;
        }

        if (ReferenceEquals(this, edge))
        {
            return true;
        }

        if (GetType() != edge.GetType())
        {
            return false;
        }

        return ValueEquals(edge);
    }


    private bool ValueEquals(Edge edge)
    {
        bool leftMatch = LeftNode == edge.LeftNode;
        bool rightMatch = RightNode == edge.RightNode;

        bool leftRightMatch = LeftNode == edge.RightNode;
        bool rightLeftMatch = RightNode == edge.LeftNode;

        return (
            (leftMatch && rightMatch) ||
            (leftRightMatch && rightLeftMatch)
        );
    }


    public static bool operator ==(Edge lhs, Edge rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            return false;
        }

        return lhs.Equals(rhs);
    }


    public static bool operator !=(Edge lhs, Edge rhs)
    {
        return !(lhs == rhs);
    }


    public override string ToString()
    {
        string output = "";

        output += $"Edge: {Weight} \n";
        output += $"  Left Node: {LeftNode}\n";
        output += $"  Right Node: {RightNode}\n";

        return output;
    }
}
