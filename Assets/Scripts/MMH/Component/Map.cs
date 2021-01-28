using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class Map : MonoBehaviour
    {
        private System.MapSystem mapSystem;

        private Data.Map data;

        private List<RectInt> placeholders;
        private List<Data.Room> rooms;
        private Dictionary<Type.Group, Data.ColonyBase> colonyBases;

        private int2 selectedCell;
        public int2 SelectedCell => selectedCell;

        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<System.MapSystem>();

            data = new Data.Map
            {
                Size = Info.Map.Size,
                Cells = new Data.Cell[Info.Map.Area],
            };

            placeholders = new List<RectInt>();
            rooms = new List<Data.Room>(Info.Map.NumberOfSeedRooms);
            colonyBases = new Dictionary<Type.Group, Data.ColonyBase>();

            selectedCell = new int2();

            SetupCells();
            SetupBase();
            SetupPaths();

            RoomBuilder.LayoutRooms(rooms, placeholders);

            SetupRooms();
            SetupColonyBases();

            ConstructMap();
        }


        public Data.Cell[] GetCells()
        {
            return data.Cells;
        }


        public Data.Cell GetCell(int x, int y)
        {
            return data.Cells[Util.Map.CoordsToIndex(x, y)];
        }


        public Data.Cell GetCell(int2 position)
        {
            return GetCell(position.x, position.y);
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
            return colonyBases[groupType];
        }


        public void SetCell(int x, int y, Data.Cell cellData)
        {
            data.Cells[Util.Map.CoordsToIndex(x, y)] = cellData;
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

            placeholders.Add(nonOverlappingBounds);
        }


        public void SelectCell(int2 position)
        {
            selectedCell = position;

            mapSystem.SelectCell(position);
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


        private void SetCellSolid(int x, int y, bool solid = true)
        {
            SetCellSolid(new int2(x, y), solid);
        }


        private void SetCellSolid(int2 position, bool solid = true)
        {
            if (Util.Map.OnMap(position))
            {
                Data.Cell cellData = GetCell(position);
                cellData.Solid = solid;

                SetCell(position, cellData);
            }
        }


        private void SetCellSolid(RectInt bounds, bool solid = true, bool fill = true)
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


        private void SetupGround(int x, int y, Type.Ground groundType)
        {
            SetupGround(new int2(x, y), groundType);
        }


        private void SetupGround(int2 position, Type.Ground groundType)
        {
            if (Util.Map.OnMap(position))
            {
                Data.Cell cellData = GetCell(position);
                cellData.GroundType = groundType;

                SetCell(position, cellData);
            }
        }


        private void SetupGround(RectInt bounds, Type.Ground groundType, bool fill = true)
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


        private void SetupStructure(int x, int y, Type.Structure structureType, bool solid = true)
        {
            SetupStructure(new int2(x, y), structureType, solid);
        }


        private void SetupStructure(int2 position, Type.Structure structureType, bool solid = true)
        {
            if (Util.Map.OnMap(position))
            {
                Data.Cell cellData = GetCell(position);

                cellData.Solid = solid;
                cellData.StructureType = structureType;

                SetCell(position, cellData);
            }
        }


        private void SetupStructure(RectInt bounds, Type.Structure structureType, bool fill = false, bool solid = true)
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


        private void SetupOverlay(int x, int y, Type.Overlay overlayType)
        {
            SetupOverlay(new int2(x, y), overlayType);
        }


        private void SetupOverlay(int2 position, Type.Overlay overlayType)
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
            foreach (Data.Room room in rooms)
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

            colonyBases[Type.Group.Guy] = guyColonyBase;
            colonyBases[Type.Group.Kailt] = kailtColonyBase;
            colonyBases[Type.Group.Taylor] = taylorColonyBase;

            foreach (KeyValuePair<Type.Group, Data.ColonyBase> keyValue in colonyBases)
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


        private void ConstructMap()
        {
            foreach (Data.Cell cell in data.Cells)
            {
                mapSystem.ConstructCell(cell);
            }
        }
    }
}