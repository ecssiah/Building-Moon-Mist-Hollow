using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    public void Build(ref MapData mapData)
    {
        SeedRooms(ref mapData);
        ExpandRooms(ref mapData);
        GenerateEntrances(ref mapData);
    }


    private void SeedRooms(ref MapData mapData)
    {
        for (int i = 0; i < MapInfo.NumberOfSeedRooms; i++)
        {
            RectInt bounds = GetNewRoomLocation(ref mapData);

            RoomData roomData = new RoomData
            {
                bounds = bounds,
                groundType = GroundType.Wood,
                wallType = WallType.StoneWall,
            };

            mapData.rooms.Add(roomData);
        }
    }


    private void ExpandRooms(ref MapData mapData)
    {
        for (int expansionRound = 0; expansionRound < 400; expansionRound++)
        {
            for (int roomNumber = 0; roomNumber < mapData.rooms.Count; roomNumber++)
            {
                mapData.rooms[roomNumber] = ExpandRoom(
                    mapData.rooms[roomNumber], ref mapData
                );
            }
        }
    }


    private RoomData ExpandRoom(RoomData roomData, ref MapData mapData)
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
                    expandedRoomData.bounds.xMin -= 1;
                    break;
                case 1:
                    expandedRoomData.bounds.xMax += 1;
                    break;
                case 2:
                    expandedRoomData.bounds.yMin -= 1;
                    break;
                case 3:
                    expandedRoomData.bounds.yMax += 1;
                    break;
                default:
                    break;
            }

            bool roomCollision = false;

            if (MapUtil.OnMap(expandedRoomData.bounds) == false)
            {
                roomCollision = true;
            }
            else
            {
                foreach (RoomData testRoomData in mapData.rooms)
                {
                    if (testRoomData.Equals(roomData)) continue;

                    if (testRoomData.bounds.Overlaps(expandedRoomData.bounds))
                    {
                        roomCollision = true;
                    }
                }

                foreach (RectInt bounds in mapData.placeholders)
                {
                    if (bounds.Overlaps(expandedRoomData.bounds))
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


    private void GenerateEntrances(ref MapData mapData)
    {
        for (int i = 0; i < mapData.rooms.Count; i++)
        {
            RoomData roomData = mapData.rooms[i];
            roomData.entrances = GenerateRoomEntrances(mapData.rooms[i].bounds);

            mapData.rooms[i] = roomData;
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
                bounds = entranceBounds,
            };

            entrances.Add(entranceData);
        }

        return entrances;
    }


    private RectInt GetNewRoomLocation(ref MapData mapData)
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
                foreach (RoomData roomData in mapData.rooms)
                {
                    if (roomBounds.Overlaps(roomData.bounds))
                    {
                        collision = true;
                    }
                }

                foreach (RectInt placeholder in mapData.placeholders)
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
