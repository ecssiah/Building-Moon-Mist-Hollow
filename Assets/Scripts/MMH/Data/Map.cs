using System.Collections.Generic;
using UnityEngine;

namespace MMH.Data
{
    public class Map
    {
        public int Size;
        public int Width => 2 * Size + 1;
        public int Area => Width * Width;

        public Cell[] Cells;

        public bool EdgesValid;

        public int[] Edges;
    }
}


