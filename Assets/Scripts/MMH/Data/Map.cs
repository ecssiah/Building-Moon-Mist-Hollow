using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Data
{
    public struct Map
    {
        public List<Cell> Cells;
        public List<int> Edges;

        public bool EdgesValid;

        public int2 SelectedCell;

        public int Size;
        public int Width;

        public List<Room> Rooms;
        public List<RectInt> Placeholders;

        public Dictionary<Type.Group, ColonyBase> ColonyBases;


        public void ResetEdges()
        {
            EdgesValid = false;
            Edges = Enumerable.Repeat(0, Cells.Count * Cells.Count).ToList();
        }
    }
}


