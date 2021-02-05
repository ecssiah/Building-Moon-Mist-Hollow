using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH.Data
{
    [Serializable]
    public class Room
    {
        public RectInt Bounds;

        public bool Fill;

        public Type.Ground GroundType;
        public Type.Structure StructureType;
        public Type.Overlay OverlayType;

        public List<Entrance> Entrances;


        public Room()
        {
            Bounds = new RectInt();

            Fill = false;

            GroundType = Type.Ground.None;
            StructureType = Type.Structure.None;
            OverlayType = Type.Overlay.None;

            Entrances = new List<Entrance>();
        }


        public Room(Room room)
        {
            Bounds = room.Bounds;

            Fill = room.Fill;

            GroundType = room.GroundType;
            StructureType = room.StructureType;
            OverlayType = room.OverlayType;

            Entrances = room.Entrances;
        }
    }
}