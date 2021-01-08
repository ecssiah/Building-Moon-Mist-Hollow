using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class RoomBuilder : MonoBehaviour
    {
        public WorldMap WorldMap;


        public void LayoutRooms()
        {
            SeedRooms();
            ExpandRooms();
            GenerateEntrances();
        }


        public void LayoutTestRooms()
        {
            Data.Room northEastTestRoom = new Data.Room
            {
                Bounds = new RectInt(2, 2, 3, 3),
                Entrances = new List<Data.Entrance>(),
                WallType = Type.Wall.WoodWall,
                GroundType = Type.Ground.Stone,
            };

            WorldMap.Rooms.Add(northEastTestRoom);

            Data.Room northWestTestRoom = new Data.Room
            {
                Bounds = new RectInt(-5, 2, 3, 3),
                Entrances = new List<Data.Entrance>(),
                WallType = Type.Wall.WoodWall,
                GroundType = Type.Ground.Stone,
            };

            WorldMap.Rooms.Add(northWestTestRoom);

            GenerateEntrances();
        }


        private void SeedRooms()
        {
            for (int i = 0; i < Info.Map.NumberOfSeedRooms; i++)
            {
                RectInt bounds = GetNewRoomLocation();

                if (bounds.width == 0 && bounds.height == 0) continue;

                Data.Room room = new Data.Room
                {
                    Bounds = bounds,
                    GroundType = Type.Ground.Wood,
                    WallType = Type.Wall.StoneWall,
                };

                WorldMap.Rooms.Add(room);
            }
        }


        private void ExpandRooms()
        {
            for (int expansionAttempt = 0; expansionAttempt < Info.Map.MaximumExpansionAttempts; expansionAttempt++)
            {
                for (int roomNumber = 0; roomNumber < WorldMap.Rooms.Count; roomNumber++)
                {
                    WorldMap.Rooms[roomNumber] = ExpandRoom(WorldMap.Rooms[roomNumber]);
                }
            }
        }


        private Data.Room ExpandRoom(Data.Room room)
        {
            bool expanded = false;

            Data.Room expandedRoom = room;

            List<int> directions = new List<int>(new int[] { 0, 1, 2, 3 });

            while (!expanded && directions.Count > 0)
            {
                int directionIndex = Random.Range(0, directions.Count - 1);
                int direction = directions[directionIndex];

                directions.RemoveAt(directionIndex);

                switch (direction)
                {
                    case 0:
                        expandedRoom.Bounds.xMin -= 1;
                        break;
                    case 1:
                        expandedRoom.Bounds.xMax += 1;
                        break;
                    case 2:
                        expandedRoom.Bounds.yMin -= 1;
                        break;
                    case 3:
                        expandedRoom.Bounds.yMax += 1;
                        break;
                    default:
                        break;
                }

                bool roomCollision = false;

                if (Util.Map.OnMap(expandedRoom.Bounds) == false)
                {
                    roomCollision = true;
                }
                else
                {
                    foreach (Data.Room testRoom in WorldMap.Rooms)
                    {
                        if (testRoom.Equals(room)) continue;

                        if (testRoom.Bounds.Overlaps(expandedRoom.Bounds))
                        {
                            roomCollision = true;
                        }
                    }

                    foreach (RectInt bounds in WorldMap.Placeholders)
                    {
                        if (bounds.Overlaps(expandedRoom.Bounds))
                        {
                            roomCollision = true;
                        }
                    }
                }

                if (!roomCollision)
                {
                    expanded = true;
                }
            }

            return expanded ? expandedRoom : room;
        }


        private void GenerateEntrances()
        {
            for (int i = 0; i < WorldMap.Rooms.Count; i++)
            {
                Data.Room room = WorldMap.Rooms[i];
                room.Entrances = GenerateRoomEntrances(WorldMap.Rooms[i].Bounds);

                WorldMap.Rooms[i] = room;
            }
        }


        private List<Data.Entrance> GenerateRoomEntrances(RectInt bounds, int number = 4)
        {
            List<Data.Entrance> entrances = new List<Data.Entrance>(number);

            for (int i = 0; i < number; i++)
            {
                Vector2Int wallPosition = Util.Map.GetRandomBorderPosition(bounds);

                RectInt entranceBounds = new RectInt(wallPosition, new Vector2Int(1, 1));

                if (entranceBounds.x == bounds.xMin || entranceBounds.x == bounds.xMax)
                {
                    entranceBounds.yMax = Random.Range(
                        entranceBounds.yMax,
                        entranceBounds.yMax + (bounds.yMax - entranceBounds.yMax)
                    );
                }
                else if (entranceBounds.y == bounds.yMin || entranceBounds.y == bounds.yMax)
                {
                    entranceBounds.xMax = Random.Range(
                        entranceBounds.xMax,
                        entranceBounds.xMax + (bounds.xMax - entranceBounds.xMax)
                    );
                }

                Data.Entrance entranceData = new Data.Entrance
                {
                    Bounds = entranceBounds,
                };

                entrances.Add(entranceData);
            }

            return entrances;
        }


        private RectInt GetNewRoomLocation()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2Int randomMapPosition = Util.Map.GetRandomMapPosition();

                RectInt roomBounds = new RectInt(
                    randomMapPosition,
                    new Vector2Int(Info.Map.SeedRoomSize, Info.Map.SeedRoomSize)
                );

                bool collision = false;

                if (Util.Map.OnMap(roomBounds) == false)
                {
                    collision = true;
                }
                else
                {
                    foreach (Data.Room room in WorldMap.Rooms)
                    {
                        if (roomBounds.Overlaps(room.Bounds))
                        {
                            collision = true;
                        }
                    }

                    foreach (RectInt placeholder in WorldMap.Placeholders)
                    {
                        if (roomBounds.Overlaps(placeholder))
                        {
                            collision = true;
                        }
                    }
                }

                if (collision == false)
                {
                    return roomBounds;
                }
            }

            return new RectInt(0, 0, 0, 0);
        }
    }
}
