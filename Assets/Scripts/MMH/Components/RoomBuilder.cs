using System.Collections.Generic;
using UnityEngine;


public class RoomBuilder : MonoBehaviour
{
    public void LayoutRooms(Map map)
    {
        SeedRooms(map);
        ExpandRooms(map);
        GenerateEntrances(map);
    }


    public void LayoutTestRooms(Map map)
    {
        RoomData northEastTestRoom = new RoomData
        {
            Bounds = new RectInt(2, 2, 3, 3),
            Entrances = new List<EntranceData>(),
            WallType = WallType.WoodWall,
            GroundType = GroundType.Stone,
        };

        map.Rooms.Add(northEastTestRoom);

        RoomData northWestTestRoom = new RoomData
        {
            Bounds = new RectInt(-5, 2, 3, 3),
            Entrances = new List<EntranceData>(),
            WallType = WallType.WoodWall,
            GroundType = GroundType.Stone,
        };

        map.Rooms.Add(northWestTestRoom);

        GenerateEntrances(map);
    }


    private void SeedRooms(Map map)
    {
        for (int i = 0; i < MapInfo.NumberOfSeedRooms; i++)
        {
            RectInt bounds = GetNewRoomLocation(map);

            if (bounds.width == 0 && bounds.height == 0) continue;

            RoomData roomData = new RoomData
            {
                Bounds = bounds,
                GroundType = GroundType.Wood,
                WallType = WallType.StoneWall,
            };

            map.Rooms.Add(roomData);
        }
    }


    private void ExpandRooms(Map map)
    {
        for (int expansionAttempt = 0; expansionAttempt < MapInfo.MaximumExpansionAttempts; expansionAttempt++)
        {
            for (int roomNumber = 0; roomNumber < map.Rooms.Count; roomNumber++)
            {
                map.Rooms[roomNumber] = ExpandRoom(map.Rooms[roomNumber], map);
            }
        }
    }


    private RoomData ExpandRoom(RoomData roomData, Map map)
    {
        bool expanded = false;

        RoomData expandedRoomData = roomData;

        List<int> directions = new List<int>(new int[] { 0, 1, 2, 3 });

        while (!expanded && directions.Count > 0)
        {
            int directionIndex = Random.Range(0, directions.Count - 1);
            int direction = directions[directionIndex];

            directions.RemoveAt(directionIndex);

            switch (direction)
            {
                case 0:
                    expandedRoomData.Bounds.xMin -= 1;
                    break;
                case 1:
                    expandedRoomData.Bounds.xMax += 1;
                    break;
                case 2:
                    expandedRoomData.Bounds.yMin -= 1;
                    break;
                case 3:
                    expandedRoomData.Bounds.yMax += 1;
                    break;
                default:
                    break;
            }

            bool roomCollision = false;

            if (MapUtil.OnMap(expandedRoomData.Bounds) == false)
            {
                roomCollision = true;
            }
            else
            {
                foreach (RoomData testRoomData in map.Rooms)
                {
                    if (testRoomData.Equals(roomData)) continue;

                    if (testRoomData.Bounds.Overlaps(expandedRoomData.Bounds))
                    {
                        roomCollision = true;
                    }
                }

                foreach (RectInt bounds in map.Placeholders)
                {
                    if (bounds.Overlaps(expandedRoomData.Bounds))
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

        return expanded ? expandedRoomData : roomData;
    }


    private void GenerateEntrances(Map map)
    {
        for (int i = 0; i < map.Rooms.Count; i++)
        {
            RoomData roomData = map.Rooms[i];
            roomData.Entrances = GenerateRoomEntrances(map.Rooms[i].Bounds);

            map.Rooms[i] = roomData;
        }
    }


    private List<EntranceData> GenerateRoomEntrances(RectInt bounds, int number = 4)
    {
        List<EntranceData> entrances = new List<EntranceData>(number);

        for (int i = 0; i < number; i++)
        {
            Vector2Int wallPosition = MapUtil.GetRandomBorderPosition(bounds);

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

            EntranceData entranceData = new EntranceData
            {
                Bounds = entranceBounds,
            };

            entrances.Add(entranceData);
        }

        return entrances;
    }


    private RectInt GetNewRoomLocation(Map map)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2Int randomMapPosition = MapUtil.GetRandomMapPosition();

            RectInt roomBounds = new RectInt(
                randomMapPosition,
                new Vector2Int(MapInfo.SeedRoomSize, MapInfo.SeedRoomSize)
            );

            bool collision = false;

            if (MapUtil.OnMap(roomBounds) == false)
            {
                collision = true;
            }
            else
            {
                foreach (RoomData roomData in map.Rooms)
                {
                    if (roomBounds.Overlaps(roomData.Bounds))
                    {
                        collision = true;
                    }
                }

                foreach (RectInt placeholder in map.Placeholders)
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
