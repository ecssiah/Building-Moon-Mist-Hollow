using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    namespace System
    {
        public class MapSystem : MonoBehaviour
        {
            private WorldMap worldMap;

            private RoomBuilder roomBuilder;

            private Vector2Int selectedCell;

            private Dictionary<string, Tile> tiles;
            private Dictionary<string, Tilemap> tilemaps;


            void Awake()
            {
                worldMap = gameObject.AddComponent<WorldMap>();

                roomBuilder = gameObject.AddComponent<RoomBuilder>();
                roomBuilder.WorldMap = worldMap;

                InitData();

                InitTiles();
                InitTilemaps();

                SetupMap();

                ConstructBoundary();
                ConstructMap();
            }


            private void InitData()
            {
                selectedCell = new Vector2Int();

                for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
                {
                    for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                    {
                        Data.Cell cellData = GetCell(x, y);
                        cellData.Position = new Vector2Int(x, y);

                        SetCell(x, y, cellData);
                    }
                }
            }


            private void InitTiles()
            {
                tiles = new Dictionary<string, Tile>();

                foreach (Tile tile in Resources.LoadAll<Tile>("Tiles"))
                {
                    tiles[tile.name] = tile;
                }
            }


            private void InitTilemaps()
            {
                tilemaps = new Dictionary<string, Tilemap>();

                Tilemap[] tilemapsArray = GameObject.Find("Tilemaps").GetComponentsInChildren<Tilemap>();

                foreach (Tilemap tilemap in tilemapsArray)
                {
                    tilemaps[tilemap.name] = tilemap;
                }

                TilemapRenderer collisionRenderer = tilemaps["Collision"].GetComponent<TilemapRenderer>();

                collisionRenderer.enabled = worldMap.ShowCollision;
            }



            private void SetupMap()
            {
                SetupBase();
                SetupPaths();

                roomBuilder.LayoutRooms();

                SetupRooms();
            }



            // Setup Methods

            public void SetPlaceholder(RectInt bounds)
            {
                worldMap.Placeholders.Add(bounds);
            }


            public void SetCellSolid(int x, int y, bool solid = true)
            {
                SetCellSolid(new Vector2Int(x, y), solid);
            }


            public void SetCellSolid(Vector2Int position, bool solid = true)
            {
                if (Util.Map.OnMap(position))
                {
                    Data.Cell cellData = GetCell(position);
                    cellData.Solid = solid;

                    SetCell(position, cellData);
                }
            }


            public void SetCellSolid(RectInt bounds, bool solid = true, bool fill = true)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    for (int y = bounds.yMin; y < bounds.yMax; y++)
                    {
                        if (fill || Util.Map.OnRectBoundary(x, y, bounds))
                        {
                            SetCellSolid(x, y, solid);
                        }
                    }
                }
            }


            public void SetupGround(int x, int y, Type.Ground groundType)
            {
                SetupGround(new Vector2Int(x, y), groundType);
            }


            public void SetupGround(Vector2Int position, Type.Ground groundType)
            {
                if (Util.Map.OnMap(position))
                {
                    Data.Cell cellData = GetCell(position);
                    cellData.GroundType = groundType;

                    SetCell(position, cellData);
                }
            }


            public void SetupGround(RectInt bounds, Type.Ground groundType, bool fill = true)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    for (int y = bounds.yMin; y < bounds.yMax; y++)
                    {
                        if (fill || Util.Map.OnRectBoundary(x, y, bounds))
                        {
                            SetupGround(x, y, groundType);
                        }
                    }
                }
            }


            public void SetupWall(int x, int y, Type.Wall wallType)
            {
                SetupWall(new Vector2Int(x, y), wallType);
            }


            public void SetupWall(Vector2Int position, Type.Wall wallType)
            {
                if (Util.Map.OnMap(position))
                {
                    Data.Cell cellData = GetCell(position);

                    cellData.Solid = true;
                    cellData.WallType = wallType;

                    SetCell(position, cellData);
                }
            }


            public void SetupWall(RectInt bounds, Type.Wall wallType, bool fill = false, bool solid = true)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    for (int y = bounds.yMin; y < bounds.yMax; y++)
                    {
                        if (fill || Util.Map.OnRectBoundary(x, y, bounds))
                        {
                            SetupWall(x, y, wallType);

                            if (solid) SetCellSolid(x, y);
                        }
                    }
                }
            }


            public void SetupOverlay(int x, int y, Type.Overlay overlayType)
            {
                SetupOverlay(new Vector2Int(x, y), overlayType);
            }


            public void SetupOverlay(Vector2Int position, Type.Overlay overlayType)
            {
                if (Util.Map.OnMap(position))
                {
                    Data.Cell cell = GetCell(position);
                    cell.OverlayType = overlayType;

                    SetCell(position, cell);
                }
            }


            private void SetupBase()
            {
                for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
                {
                    for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                    {
                        SetupGround(x, y, Type.Ground.Grass);
                    }
                }
            }


            private void SetupPaths()
            {
                RectInt mainEastWest = new RectInt(
                    -Info.Map.Size, -Info.Map.PathWidth / 2,
                    Info.Map.Width, Info.Map.PathWidth
                );
                RectInt mainNorthSouth = new RectInt(
                    -Info.Map.PathWidth / 2, -Info.Map.Size,
                    Info.Map.PathWidth, Info.Map.Width
                );

                RectInt north1st = new RectInt(
                    -Info.Map.Size, Info.Map.Size / 2 - Info.Map.PathWidth / 2,
                    Info.Map.Width, Info.Map.PathWidth / 2
                );
                RectInt south1st = new RectInt(
                    -Info.Map.Size, -Info.Map.Size / 2 - Info.Map.PathWidth / 2,
                    Info.Map.Width, Info.Map.PathWidth / 2
                );

                RectInt west1st = new RectInt(
                    -Info.Map.Size / 2 - Info.Map.PathWidth / 2, -Info.Map.Size,
                    Info.Map.PathWidth / 2, Info.Map.Width
                );
                RectInt east1st = new RectInt(
                    Info.Map.Size / 2 - Info.Map.PathWidth / 2, -Info.Map.Size,
                    Info.Map.PathWidth / 2, Info.Map.Width
                );

                SetupGround(mainEastWest, Type.Ground.Stone);
                SetupGround(mainNorthSouth, Type.Ground.Stone);

                SetupGround(north1st, Type.Ground.Stone);
                SetupGround(south1st, Type.Ground.Stone);
                SetupGround(west1st, Type.Ground.Stone);
                SetupGround(east1st, Type.Ground.Stone);

                SetPlaceholder(mainEastWest);
                SetPlaceholder(mainNorthSouth);

                SetPlaceholder(north1st);
                SetPlaceholder(south1st);
                SetPlaceholder(west1st);
                SetPlaceholder(east1st);
            }


            private void SetupRooms()
            {
                foreach (Data.Room room in worldMap.Rooms)
                {
                    SetupRoom(room);
                }
            }


            private void SetupRoom(Data.Room room)
            {
                for (int x = room.Bounds.xMin; x <= room.Bounds.xMax; x++)
                {
                    for (int y = room.Bounds.yMin; y <= room.Bounds.yMax; y++)
                    {
                        Vector2Int cellPosition = new Vector2Int(x, y);

                        SetupGround(cellPosition, room.GroundType);

                        if (Util.Map.EntranceExistsAt(x, y, room) == false)
                        {
                            if (room.Fill || Util.Map.OnRectBoundary(x, y, room.Bounds))
                            {
                                SetupWall(cellPosition, room.WallType);
                            }
                        }

                        SetupOverlay(cellPosition, room.OverlayType);
                    }
                }
            }



            // Construction Methods

            public void ConstructMap()
            {
                foreach (Data.Cell cell in worldMap.Cells)
                {
                    if (cell.Solid)
                    {
                        ConstructSolid(cell.Position);
                    }

                    if (cell.GroundType != Type.Ground.None)
                    {
                        ConstructGround(cell.Position, cell.GroundType);
                    }

                    if (cell.WallType != Type.Wall.None)
                    {
                        ConstructWall(cell.Position, cell.WallType);
                    }

                    if (cell.OverlayType != Type.Overlay.None)
                    {
                        ConstructOverlay(cell.Position, cell.OverlayType);
                    }
                }
            }


            private void ConstructBoundary()
            {
                for (int x = Info.Map.WorldBoundary.xMin; x <= Info.Map.WorldBoundary.xMax; x++)
                {
                    for (int y = Info.Map.WorldBoundary.yMin; y <= Info.Map.WorldBoundary.yMax; y++)
                    {
                        if (Util.Map.OnRectBoundary(x, y, Info.Map.WorldBoundary))
                        {
                            tilemaps["Collision"].SetTile(new Vector3Int(x, y, 0), tiles["Collision_1"]);
                        }
                    }
                }
            }


            private void ConstructSolid(Vector2Int position)
            {
                tilemaps["Collision"].SetTile(
                    new Vector3Int(position.x, position.y, 0),
                    tiles[Info.Tile.overlayTileNames[Type.Overlay.Collision]]
                );
            }


            private void ConstructGround(Vector2Int position, Type.Ground cellType)
            {
                tilemaps["Ground"].SetTile(
                    new Vector3Int(position.x, position.y, 0),
                    tiles[Info.Tile.groundTileNames[cellType]]
                );
            }


            private void ConstructWall(Vector2Int position, Type.Wall wallType)
            {
                tilemaps["Walls"].SetTile(
                    new Vector3Int(position.x, position.y, 3),
                    tiles[Info.Tile.wallTileNames[wallType]]
                );
            }


            private void ConstructOverlay(Vector2Int position, Type.Overlay overlayType)
            {
                tilemaps["Overlay"].SetTile(
                    new Vector3Int(position.x, position.y, 0),
                    tiles[Info.Tile.overlayTileNames[overlayType]]
                );
            }


            // Get Methods

            public Data.Cell GetCell(int x, int y)
            {
                return worldMap.Cells[Util.Map.CoordsToIndex(x, y)];
            }


            public Data.Cell GetCell(Vector2Int position)
            {
                return GetCell(position.x, position.y);
            }


            public Data.Cell[] GetCells()
            {
                return worldMap.Cells;
            }


            public void SetCell(int x, int y, Data.Cell cellData)
            {
                worldMap.Cells[Util.Map.CoordsToIndex(x, y)] = cellData;
            }


            public void SetCell(Vector2Int position, Data.Cell cellData)
            {
                SetCell(position.x, position.y, cellData);
            }



            // Selection Methods

            public void SelectCell(Vector2Int position)
            {
                selectedCell = position;

                var selectedPosition = new Vector2Int(position.x, position.y);

                ConstructOverlay(selectedPosition, Type.Overlay.Selection);
            }


            public void ClearSelection()
            {
                tilemaps["Overlay"].SetTile(new Vector3Int(selectedCell.x, selectedCell.y, 0), null);
            }



            // Saving/Loading - Serialization

            public void SaveMapData(string name)
            {
                using (StreamWriter file = File.CreateText($"Assets/Resources/Data/{name}.json"))
                {
                    string jsonCellsText = JsonUtility.ToJson(worldMap, true);

                    file.Write(jsonCellsText);
                }
            }


            public void LoadMapData(string name)
            {
                using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
                {
                    string jsonText = reader.ReadToEnd();

                    worldMap = JsonUtility.FromJson<WorldMap>(jsonText);
                }
            }
        }
    }
}