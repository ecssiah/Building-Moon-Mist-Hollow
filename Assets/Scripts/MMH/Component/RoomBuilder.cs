using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public static class RoomBuilder
    {
        public static void LayoutRooms(WorldMap worldMap)
        {
            SeedRooms(worldMap);
            ExpandRooms(worldMap);
            GenerateEntrances(worldMap);
        }


        private static void SeedRooms(WorldMap worldMap)
        {
            for (int i = 0; i < Info.Map.NumberOfSeedRooms; i++)
            {
                RectInt bounds = GetNewRoomLocation(worldMap);

                if (bounds.width == 0 && bounds.height == 0) continue;

                Data.Room room = new Data.Room
                {
                    Bounds = bounds,
                    GroundType = Type.Ground.Wood,
                    WallType = Type.Structure.Stone_Wall,
                };

                worldMap.Rooms.Add(room);
            }
        }


        private static void ExpandRooms(WorldMap worldMap)
        {
            for (int expansionAttempt = 0; expansionAttempt < Info.Map.MaximumExpansionAttempts; expansionAttempt++)
            {
                for (int roomNumber = 0; roomNumber < worldMap.Rooms.Count; roomNumber++)
                {
                    worldMap.Rooms[roomNumber] = ExpandRoom(worldMap, worldMap.Rooms[roomNumber]);
                }
            }
        }


        private static Data.Room ExpandRoom(WorldMap worldMap, Data.Room room)
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
                    foreach (Data.Room testRoom in worldMap.Rooms)
                    {
                        if (testRoom.Equals(room)) continue;

                        if (testRoom.Bounds.Overlaps(expandedRoom.Bounds))
                        {
                            roomCollision = true;
                        }
                    }

                    foreach (RectInt bounds in worldMap.Placeholders)
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


        private static void GenerateEntrances(WorldMap worldMap)
        {
            for (int i = 0; i < worldMap.Rooms.Count; i++)
            {
                Data.Room room = worldMap.Rooms[i];
                room.Entrances = GenerateRoomEntrances(worldMap.Rooms[i].Bounds);

                worldMap.Rooms[i] = room;
            }
        }


        private static List<Data.Entrance> GenerateRoomEntrances(RectInt bounds, int number = 4)
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


        private static RectInt GetNewRoomLocation(WorldMap worldMap)
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
                    foreach (Data.Room room in worldMap.Rooms)
                    {
                        if (roomBounds.Overlaps(room.Bounds))
                        {
                            collision = true;
                        }
                    }

                    foreach (RectInt placeholder in worldMap.Placeholders)
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
