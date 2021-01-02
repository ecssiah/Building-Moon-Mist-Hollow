using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomBuilder : MonoBehaviour
{
    public MapData Build(MapData mapData)
    {
        mapData = SeedRooms(mapData);
        mapData = ExpandRooms(mapData);
        mapData = GenerateEntrances(mapData);

        return mapData;
    }


    private MapData SeedRooms(MapData mapData)
    {
        for (int i = 0; i < MapInfo.NumberOfSeedRooms; i++)
        {
            RectInt bounds = GetNewRoomLocation(mapData);

            if (bounds.width == 0 && bounds.height == 0) continue;

            RoomData roomData = new RoomData
            {
                Bounds = bounds,
                GroundType = GroundType.Wood,
                WallType = WallType.StoneWall,
            };

            mapData.Rooms.Add(roomData);
        }

        return mapData;
    }


    private MapData ExpandRooms(MapData mapData)
    {
        for (int expansionAttempt = 0; expansionAttempt < MapInfo.MaximumExpansionAttempts; expansionAttempt++)
        {
            for (int roomNumber = 0; roomNumber < mapData.Rooms.Count; roomNumber++)
            {
                mapData.Rooms[roomNumber] = ExpandRoom(mapData.Rooms[roomNumber], mapData);
            }
        }

        return mapData;
    }


    private RoomData ExpandRoom(RoomData roomData, MapData mapData)
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
                foreach (RoomData testRoomData in mapData.Rooms)
                {
                    if (testRoomData.Equals(roomData)) continue;

                    if (testRoomData.Bounds.Overlaps(expandedRoomData.Bounds))
                    {
                        roomCollision = true;
                    }
                }

                foreach (RectInt bounds in mapData.Placeholders)
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


    private MapData GenerateEntrances(MapData mapData)
    {
        for (int i = 0; i < mapData.Rooms.Count; i++)
        {
            RoomData roomData = mapData.Rooms[i];
            roomData.Entrances = GenerateRoomEntrances(mapData.Rooms[i].Bounds);

            mapData.Rooms[i] = roomData;
        }

        return mapData;
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


    private RectInt GetNewRoomLocation(MapData mapData)
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
                foreach (RoomData roomData in mapData.Rooms)
                {
                    if (roomBounds.Overlaps(roomData.Bounds))
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
