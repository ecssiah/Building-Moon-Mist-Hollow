using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory : MonoBehaviour
{
    private MapSystem mapSystem;

    private RoomBuilder roomBuilder;


    void Awake()
    {
        mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

        roomBuilder = gameObject.AddComponent<RoomBuilder>();
    }


    public void SetupMap(ref MapData mapData)
    {
        SetupBase();
        SetupPaths(ref mapData);
        SetupRooms(ref mapData);
    }


    private void SetupBase()
    {
        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
            {
                mapSystem.SetupGround(x, y, GroundType.Grass);
            }
        }

        mapSystem.SetupGround(0, 0, GroundType.Wood);
    }


    private void SetupPaths(ref MapData mapData)
    {
        int pathWidth = 8;

        RectInt north1st = new RectInt(
            -MapInfo.Size, MapInfo.Size / 2,
            MapInfo.Width, pathWidth / 2
        );
        RectInt south1st = new RectInt(
            -MapInfo.Size, -MapInfo.Size / 2 - pathWidth / 2,
            MapInfo.Width, pathWidth / 2
        );

        RectInt west1st = new RectInt(
            -MapInfo.Size / 2 - pathWidth / 2, -MapInfo.Size,
            pathWidth / 2, MapInfo.Width
        );
        RectInt east1st = new RectInt(
            MapInfo.Size / 2, -MapInfo.Size,
            pathWidth / 2, MapInfo.Width
        );

        RectInt mainEastWest = new RectInt(
            -MapInfo.Size, -pathWidth / 2,
            MapInfo.Width, pathWidth
        );
        RectInt mainNorthSouth = new RectInt(
            -pathWidth / 2, -MapInfo.Size,
            pathWidth, MapInfo.Width
        );

        mapSystem.SetupGround(north1st, GroundType.Stone);
        mapSystem.SetupGround(south1st, GroundType.Stone);
        mapSystem.SetupGround(west1st, GroundType.Stone);
        mapSystem.SetupGround(east1st, GroundType.Stone);
        mapSystem.SetupGround(mainEastWest, GroundType.Stone);
        mapSystem.SetupGround(mainNorthSouth, GroundType.Stone);

        mapData.Placeholders.Add(north1st);
        mapData.Placeholders.Add(south1st);
        mapData.Placeholders.Add(west1st);
        mapData.Placeholders.Add(east1st);
        mapData.Placeholders.Add(mainEastWest);
        mapData.Placeholders.Add(mainNorthSouth);
    }


    private void SetupRooms(ref MapData mapData)
    {
        roomBuilder.Build(ref mapData);

        foreach (RoomData roomData in mapData.Rooms)
        {
            SetupRoom(roomData);
        }
    }


    private void SetupRoom(RoomData roomData)
    {
        for (int x = roomData.Bounds.xMin; x <= roomData.Bounds.xMax; x++)
        {
            for (int y = roomData.Bounds.yMin; y <= roomData.Bounds.yMax; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);

                mapSystem.SetupGround(cellPosition, roomData.GroundType);

                if (MapUtil.EntranceExistsAt(x, y, roomData) == false)
                {
                    if (roomData.Fill || MapUtil.OnRectBoundary(x, y, roomData.Bounds))
                    {
                        mapSystem.SetupWall(cellPosition, roomData.WallType);
                    }
                }

                mapSystem.SetupOverlay(cellPosition, roomData.OverlayType);
            }
        }
    }


}
