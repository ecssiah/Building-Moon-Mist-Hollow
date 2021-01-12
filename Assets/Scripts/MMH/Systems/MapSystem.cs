using System.IO;
using UnityEngine;

namespace MMH
{
    namespace System
    {
        public class MapSystem : MonoBehaviour
        {
            private RenderSystem renderSystem;

            private WorldMap worldMap;

            private Vector2Int selectedCell;


            void Awake()
            {
                renderSystem = GameObject.Find("RenderSystem").GetComponent<RenderSystem>();
    
                worldMap = gameObject.AddComponent<WorldMap>();

                SetupCells();
                SetupBase();
                SetupPaths();

                RoomBuilder.LayoutRooms(worldMap);

                SetupRooms();

                ConstructMap();
            }


            private void SetupCells()
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



            // Setup Methods
           
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
                SetPlaceholder(mainEastWest);

                SetupGround(mainNorthSouth, Type.Ground.Stone);
                SetPlaceholder(mainNorthSouth);

                SetupGround(north1st, Type.Ground.Stone);
                SetPlaceholder(north1st);

                SetupGround(south1st, Type.Ground.Stone);
                SetPlaceholder(south1st);

                SetupGround(west1st, Type.Ground.Stone);
                SetPlaceholder(west1st);

                SetupGround(east1st, Type.Ground.Stone);
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
                    if (cell.GroundType != Type.Ground.None)
                    {
                        renderSystem.SetTile(cell.Position, cell.GroundType);
                    }

                    if (cell.WallType != Type.Wall.None)
                    {
                        renderSystem.SetTile(cell.Position, cell.WallType);
                    }

                    if (cell.OverlayType != Type.Overlay.None)
                    {
                        renderSystem.SetTile(cell.Position, cell.OverlayType);
                    }
                }
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


            public Data.Cell GetFreeCell()
            {
                Data.Cell cellData = GetCell(Util.Map.GetRandomMapPosition());

                while (cellData.Solid)
                {
                    cellData = GetCell(Util.Map.GetRandomMapPosition());
                }

                return cellData;
            }


            public void SetCell(int x, int y, Data.Cell cellData)
            {
                worldMap.Cells[Util.Map.CoordsToIndex(x, y)] = cellData;
            }


            public void SetCell(Vector2Int position, Data.Cell cellData)
            {
                SetCell(position.x, position.y, cellData);
            }


            public void SetPlaceholder(RectInt bounds)
            {
                RectInt nonOverlappingBounds = new RectInt
                {
                    x = bounds.x - 1,
                    y = bounds.y - 1,
                    width = bounds.width + 1,
                    height = bounds.height + 1,
                };

                worldMap.Placeholders.Add(nonOverlappingBounds);
            }



            // Selection Methods

            public void SelectCell(Vector2Int position)
            {
                selectedCell = position;

                renderSystem.SetTile(position, Type.Overlay.Selection);
            }


            public void ClearSelection()
            {
                renderSystem.ClearTile(selectedCell, "Overlay");
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