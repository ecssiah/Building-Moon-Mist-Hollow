using System;
using Unity.Mathematics;

namespace MMH.Data
{
    public class Node
    {
        public int Index;
        public int2 Position;

        public int PreviousNodeIndex;

        public bool Solid;

        public int FCost;
        public int GCost;
        public int HCost;


        public override bool Equals(object obj)
        {
            return obj is Node node &&
                   Index == node.Index &&
                   Position.Equals(node.Position);
        }


        public override int GetHashCode()
        {
            int hashCode = -606656326;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Position.GetHashCode();

            return hashCode;
        }


        public static bool operator ==(Node lhs, Node rhs)
        {
            return lhs.Equals(rhs);
        }


        public static bool operator !=(Node lhs, Node rhs)
        {
            return !(lhs.Equals(rhs));
        }
    }
}