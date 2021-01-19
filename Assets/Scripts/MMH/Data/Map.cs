using System.Collections.Generic;
using UnityEngine;

namespace MMH.Data
{
    public struct Map
    {
        public Cell[] Cells;

        public int Size;
        public int Width { get => 2 * Size + 1; }

        public List<Data.Room> Rooms;
        public List<RectInt> Placeholders;
        public Dictionary<Type.Group, ColonyBase> ColonyBases;
    }
}


