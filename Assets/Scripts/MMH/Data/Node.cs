using Unity.Mathematics;

namespace MMH.Data
{
    public struct Node
    {
        public int Index;
        public int PreviousIndex;

        public int GCost;
        public int HCost;
        public int FCost;

        public int2 Position;
        public bool Solid;
    }
}
