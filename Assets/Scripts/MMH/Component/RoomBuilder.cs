using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MMH
{
    public static class RoomBuilder
    {
        public static void LayoutRooms(Data.Map mapData)
        {
            SeedRooms(mapData);
            ExpandRooms(mapData);
            GenerateEntrances(mapData);
        }


        private static void SeedRooms(Data.Map mapData)
        {
            for (int i = 0; i < Info.Map.NumberOfSeedRooms; i++)
            {
                RectInt bounds = GetNewRoomLocation(mapData);

                if (bounds.width == 0 && bounds.height == 0) continue;

                Data.Room room = new Data.Room
                {
                    Bounds = bounds,
                    GroundType = Type.Ground.Wood,
                    StructureType = Type.Structure.Stone_Wall,
                };

                mapData.Rooms.Add(room);
            }
        }


        private static void ExpandRooms(Data.Map mapData)
        {
            for (int expansionAttempt = 0; expansionAttempt < Info.Map.MaximumExpansionAttempts; expansionAttempt++)
            {
                for (int roomNumber = 0; roomNumber < mapData.Rooms.Count; roomNumber++)
                {
                    mapData.Rooms[roomNumber] = ExpandRoom(mapData, mapData.Rooms[roomNumber]);
                }
            }
        }


        private static Data.Room ExpandRoom(Data.Map mapData, Data.Room room)
        {
            bool expanded = false;
            Data.Room expandedRoom = room;

            List<Type.Direction> directions = new List<Type.Direction> {
                Type.Direction.S, Type.Direction.N, Type.Direction.W, Type.Direction.E
            };

            while (!expanded && directions.Count > 0)
            {
                int index = Random.Range(0, directions.Count - 1);

                Type.Direction direction = directions[index];

                directions.RemoveAt(index);

                switch (direction)
                {
                    case Type.Direction.S:
                        expandedRoom.Bounds.xMin -= 1;
                        break;
                    case Type.Direction.W:
                        expandedRoom.Bounds.xMax += 1;
                        break;
                    case Type.Direction.N:
                        expandedRoom.Bounds.yMin -= 1;
                        break;
                    case Type.Direction.E:
                        expandedRoom.Bounds.yMax += 1;
                        break;
                }

                bool roomCollision = false;

                if (Util.Map.OnMap(expandedRoom.Bounds) == false)
                {
                    roomCollision = true;
                }
                else
                {
                    foreach (Data.Room testRoom in mapData.Rooms)
                    {
                        if (testRoom.Equals(room)) continue;

                        if (testRoom.Bounds.Overlaps(expandedRoom.Bounds))
                        {
                            roomCollision = true;
                        }
                    }

                    foreach (RectInt bounds in mapData.Placeholders)
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


        private static void GenerateEntrances(Data.Map mapData)
        {
            for (int i = 0; i < mapData.Rooms.Count; i++)
            {
                Data.Room room = mapData.Rooms[i];
                room.Entrances = GenerateRoomEntrances(mapData.Rooms[i].Bounds);

                mapData.Rooms[i] = room;
            }
        }


        private static List<Data.Entrance> GenerateRoomEntrances(RectInt bounds, int number = 4)
        {
            List<Data.Entrance> entrances = new List<Data.Entrance>(number);

            for (int i = 0; i < number; i++)
            {
                int2 wallPosition = Util.Map.GetRandomBorderPosition(bounds);

                RectInt entranceBounds = new RectInt(
                    new Vector2Int(wallPosition.x, wallPosition.y),
                    new Vector2Int(1, 1)
                );

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


        private static RectInt GetNewRoomLocation(Data.Map mapData)
        {
            for (int i = 0; i < 10; i++)
            {
                int2 randomMapPosition = Util.Map.GetRandomMapPosition();

                RectInt roomBounds = new RectInt(
                    new Vector2Int(randomMapPosition.x, randomMapPosition.y),
                    new Vector2Int(Info.Map.SeedRoomSize, Info.Map.SeedRoomSize)
                );

                bool collision = false;

                if (Util.Map.OnMap(roomBounds) == false)
                {
                    collision = true;
                }
                else
                {
                    foreach (Data.Room room in mapData.Rooms)
                    {
                        if (roomBounds.Overlaps(room.Bounds))
                        {
                            collision = true;
                        }
                    }

                    foreach (RectInt placeholder in mapData.Placeholders)
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
