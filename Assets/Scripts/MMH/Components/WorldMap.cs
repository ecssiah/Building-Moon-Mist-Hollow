using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class WorldMap : MonoBehaviour
    {
        public RoomBuilder RoomBuilder;

        public Data.Cell[] Cells;

        public bool ShowCollision;

        public int Size;
        public int Width { get => 2 * Size + 1; }

        public List<Data.Room> Rooms;
        public List<RectInt> Placeholders;


        void Awake()
        {
            RoomBuilder = gameObject.AddComponent<RoomBuilder>();

            Cells = new Data.Cell[Info.Map.Width * Info.Map.Width];

            ShowCollision = Info.Map.ShowCollision;

            Size = Info.Map.Size;

            Rooms = new List<Data.Room>(Info.Map.NumberOfSeedRooms);
            Placeholders = new List<RectInt>();
        }
    }
}


