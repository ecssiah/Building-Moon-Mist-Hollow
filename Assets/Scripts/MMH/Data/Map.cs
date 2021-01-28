using System.Collections.Generic;
using UnityEngine;

namespace MMH.Data
{
    public struct Map
    {
        public int Size;
        public int Width { get => 2 * Size + 1; }

        public Cell[] Cells;
    }
}


