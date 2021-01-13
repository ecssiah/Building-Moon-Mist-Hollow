using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class WorldMap : MonoBehaviour
    {
        public Data.Cell[] Cells;

        public int Size;
        public int Width { get => 2 * Size + 1; }

        public List<Data.Room> Rooms;
        public List<RectInt> Placeholders;


        void Awake()
        {
            Size = Info.Map.Size;
            Placeholders = new List<RectInt>();
            Cells = new Data.Cell[Info.Map.Width * Info.Map.Width];
            Rooms = new List<Data.Room>(Info.Map.NumberOfSeedRooms);
        }
    }
}


