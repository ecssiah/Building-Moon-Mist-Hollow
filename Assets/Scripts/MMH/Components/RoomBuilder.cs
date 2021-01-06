using System.Collections.Generic;
using UnityEngine;


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
        RoomData northEastTestRoom = new RoomData
        {
            Bounds = new RectInt(2, 2, 3, 3),
            Entrances = new List<EntranceData>(),
            WallType = WallType.WoodWall,
            GroundType = GroundType.Stone,
        };

        WorldMap.Rooms.Add(northEastTestRoom);

        RoomData northWestTestRoom = new RoomData
        {
            Bounds = new RectInt(-5, 2, 3, 3),
            Entrances = new List<EntranceData>(),
            WallType = WallType.WoodWall,
            GroundType = GroundType.Stone,
        };

        WorldMap.Rooms.Add(northWestTestRoom);

        GenerateEntrances();
    }


    private void SeedRooms()
    {
        for (int i = 0; i < MapInfo.NumberOfSeedRooms; i++)
        {
            RectInt bounds = GetNewRoomLocation();

            if (bounds.width == 0 && bounds.height == 0) continue;

            RoomData roomData = new RoomData
            {
                Bounds = bounds,
                GroundType = GroundType.Wood,
                WallType = WallType.StoneWall,
            };

            WorldMap.Rooms.Add(roomData);
        }
    }


    private void ExpandRooms()
    {
        for (int expansionAttempt = 0; expansionAttempt < MapInfo.MaximumExpansionAttempts; expansionAttempt++)
        {
            for (int roomNumber = 0; roomNumber < WorldMap.Rooms.Count; roomNumber++)
            {
                WorldMap.Rooms[roomNumber] = ExpandRoom(WorldMap.Rooms[roomNumber]);
            }
        }
    }


    private RoomData ExpandRoom(RoomData roomData)
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
                foreach (RoomData testRoomData in WorldMap.Rooms)
                {
                    if (testRoomData.Equals(roomData)) continue;

                    if (testRoomData.Bounds.Overlaps(expandedRoomData.Bounds))
                    {
                        roomCollision = true;
                    }
                }

                foreach (RectInt bounds in WorldMap.Placeholders)
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


    private void GenerateEntrances()
    {
        for (int i = 0; i < WorldMap.Rooms.Count; i++)
        {
            RoomData roomData = WorldMap.Rooms[i];
            roomData.Entrances = GenerateRoomEntrances(WorldMap.Rooms[i].Bounds);

            WorldMap.Rooms[i] = roomData;
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


    private RectInt GetNewRoomLocation()
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
                foreach (RoomData roomData in WorldMap.Rooms)
                {
                    if (roomBounds.Overlaps(roomData.Bounds))
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
