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


    public MapData BuildMap(MapData mapData)
    {
        SetupBase();
        SetupPaths();

        //mapData = roomBuilder.LayoutRooms(mapData);
        mapData = roomBuilder.LayoutTestRooms(mapData);

        SetupRooms(mapData);

        mapSystem.SetCellSolid(1, 1);

        return mapData;
    }


    private void LayoutTestRooms()
    {
        
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

        mapSystem.SetupGround(0, 0, GroundType.Water);
    }


    private void SetupPaths()
    {
        RectInt mainEastWest = new RectInt(
            -MapInfo.Size, -MapInfo.PathWidth / 2,
            MapInfo.Width, MapInfo.PathWidth
        );
        RectInt mainNorthSouth = new RectInt(
            -MapInfo.PathWidth / 2, -MapInfo.Size,
            MapInfo.PathWidth, MapInfo.Width
        );

        RectInt north1st = new RectInt(
            -MapInfo.Size, MapInfo.Size / 2 - MapInfo.PathWidth / 2,
            MapInfo.Width, MapInfo.PathWidth / 2
        );
        RectInt south1st = new RectInt(
            -MapInfo.Size, -MapInfo.Size / 2 - MapInfo.PathWidth / 2,
            MapInfo.Width, MapInfo.PathWidth / 2
        );

        RectInt west1st = new RectInt(
            -MapInfo.Size / 2 - MapInfo.PathWidth / 2, -MapInfo.Size,
            MapInfo.PathWidth / 2, MapInfo.Width
        );
        RectInt east1st = new RectInt(
            MapInfo.Size / 2 - MapInfo.PathWidth / 2, -MapInfo.Size,
            MapInfo.PathWidth / 2, MapInfo.Width
        );

        mapSystem.SetupGround(mainEastWest, GroundType.Stone);
        mapSystem.SetupGround(mainNorthSouth, GroundType.Stone);

        mapSystem.SetupGround(north1st, GroundType.Stone);
        mapSystem.SetupGround(south1st, GroundType.Stone);
        mapSystem.SetupGround(west1st, GroundType.Stone);
        mapSystem.SetupGround(east1st, GroundType.Stone);

        mapSystem.SetPlaceholder(mainEastWest);
        mapSystem.SetPlaceholder(mainNorthSouth);

        mapSystem.SetPlaceholder(north1st);
        mapSystem.SetPlaceholder(south1st);
        mapSystem.SetPlaceholder(west1st);
        mapSystem.SetPlaceholder(east1st);
    }


    private void SetupRooms(MapData mapData)
    {
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
