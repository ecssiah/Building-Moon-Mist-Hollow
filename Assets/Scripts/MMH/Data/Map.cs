using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Data
{
    public struct Map
    {
        public Cell[] Cells;
        public List<int> Edges;

        public int2 SelectedCell;

        public bool EdgesValid;

        public int Size;
        public int Width { get => 2 * Size + 1; }

        public List<Room> Rooms;
        public List<RectInt> Placeholders;
        public Dictionary<Type.Group, ColonyBase> ColonyBases;


        public void ClearEdges()
        {
            Edges = Enumerable.Repeat(0, Edges.Count).ToList();
        }
    }
}


