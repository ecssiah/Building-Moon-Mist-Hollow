using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.System
{
    public class MapSystem : MonoBehaviour
    {
        private RenderSystem renderSystem;

        private Data.Map mapData;

        private int2 selectedCell;


        void Awake()
        {
            renderSystem = GameObject.Find("RenderSystem").GetComponent<RenderSystem>();

            mapData = new Data.Map
            {
                Size = Info.Map.Size,
                Placeholders = new List<RectInt>(),
                Cells = new Data.Cell[Info.Map.Width * Info.Map.Width],
                Rooms = new List<Data.Room>(Info.Map.NumberOfSeedRooms),
                ColonyBases = new Dictionary<Type.Group, Data.ColonyBase>(),
            };

            SetupCells();
            SetupBase();
            SetupPaths();

            RoomBuilder.LayoutRooms(mapData);

            SetupRooms();
            SetupColonyBases();

            ConstructMap();
        }


        private void SetupCells()
        {
            selectedCell = new int2();

            for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
            {
                for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                {
                    Data.Cell cellData = GetCell(x, y);
                    cellData.Position = new int2(x, y);

                    SetCell(x, y, cellData);
                }
            }
        }



        // Setup Methods

        public void SetCellSolid(int x, int y, bool solid = true)
        {
            SetCellSolid(new int2(x, y), solid);
        }


        public void SetCellSolid(int2 position, bool solid = true)
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
            SetupGround(new int2(x, y), groundType);
        }


        public void SetupGround(int2 position, Type.Ground groundType)
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


        public void SetupStructure(int x, int y, Type.Structure structureType, bool solid = true)
        {
            SetupStructure(new int2(x, y), structureType, solid);
        }


        public void SetupStructure(int2 position, Type.Structure structureType, bool solid = true)
        {
            if (Util.Map.OnMap(position))
            {
                Data.Cell cellData = GetCell(position);

                cellData.Solid = solid;
                cellData.StructureType = structureType;

                SetCell(position, cellData);
            }
        }


        public void SetupStructure(RectInt bounds, Type.Structure structureType, bool fill = false, bool solid = true)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    if (fill || Util.Map.OnRectBoundary(x, y, bounds))
                    {
                        SetupStructure(x, y, structureType);

                        if (solid) SetCellSolid(x, y);
                    }
                }
            }
        }


        public void SetupOverlay(int x, int y, Type.Overlay overlayType)
        {
            SetupOverlay(new int2(x, y), overlayType);
        }


        public void SetupOverlay(int2 position, Type.Overlay overlayType)
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
            foreach (Data.Room room in mapData.Rooms)
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
                    int2 cellPosition = new int2(x, y);

                    SetupGround(cellPosition, room.GroundType);

                    if (Util.Map.EntranceExistsAt(x, y, room) == false)
                    {
                        if (room.Fill || Util.Map.OnRectBoundary(x, y, room.Bounds))
                        {
                            SetupStructure(cellPosition, room.StructureType);
                        }
                    }

                    SetupOverlay(cellPosition, room.OverlayType);
                }
            }
        }


        private void SetupColonyBases()
        {
            Data.ColonyBase guyColonyBase = new Data.ColonyBase
            {
                GroupType = Type.Group.Guy,
                Position = GetFreeCell().Position,
            };

            Data.ColonyBase kailtColonyBase = new Data.ColonyBase
            {
                GroupType = Type.Group.Kailt,
                Position = GetFreeCell().Position,
            };

            Data.ColonyBase taylorColonyBase = new Data.ColonyBase
            {
                GroupType = Type.Group.Taylor,
                Position = GetFreeCell().Position,
            };

            mapData.ColonyBases[Type.Group.Guy] = guyColonyBase;
            mapData.ColonyBases[Type.Group.Kailt] = kailtColonyBase;
            mapData.ColonyBases[Type.Group.Taylor] = taylorColonyBase;

            foreach (KeyValuePair<Type.Group, Data.ColonyBase> keyValue in mapData.ColonyBases)
            {
                Data.ColonyBase colonyBaseData = keyValue.Value;

                Type.Structure structureType = Type.Structure.None;

                switch (colonyBaseData.GroupType)
                {
                    case Type.Group.Guy:
                        structureType = Type.Structure.Guy_Flag;
                        break;
                    case Type.Group.Kailt:
                        structureType = Type.Structure.Kailt_Flag;
                        break;
                    case Type.Group.Taylor:
                        structureType = Type.Structure.Taylor_Flag;
                        break;
                }

                SetupStructure(colonyBaseData.Position, structureType, false);
            }
        }



        // Construction Methods

        public void ConstructMap()
        {
            foreach (Data.Cell cell in mapData.Cells)
            {
                if (cell.GroundType != Type.Ground.None)
                {
                    renderSystem.SetTile(cell.Position, cell.GroundType);
                }

                if (cell.StructureType != Type.Structure.None)
                {
                    renderSystem.SetTile(cell.Position, cell.StructureType);
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
            return mapData.Cells[Util.Map.CoordsToIndex(x, y)];
        }


        public Data.Cell GetCell(int2 position)
        {
            return GetCell(position.x, position.y);
        }


        public Data.Cell[] GetCells()
        {
            return mapData.Cells;
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


        public Data.ColonyBase GetColonyBase(Type.Group groupType)
        {
            return mapData.ColonyBases[groupType];
        }



        // Set Methods


        public void SetCell(int x, int y, Data.Cell cellData)
        {
            mapData.Cells[Util.Map.CoordsToIndex(x, y)] = cellData;
        }


        public void SetCell(int2 position, Data.Cell cellData)
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

            mapData.Placeholders.Add(nonOverlappingBounds);
        }



        // Selection Methods

        public void SelectCell(int2 position)
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
                string jsonCellsText = JsonUtility.ToJson(mapData, true);

                file.Write(jsonCellsText);
            }
        }


        public void LoadMapData(string name)
        {
            using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
            {
                string jsonText = reader.ReadToEnd();

                mapData = JsonUtility.FromJson<Data.Map>(jsonText);
            }
        }
    }
}