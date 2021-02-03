using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class Map : MonoBehaviour
    {
        private System.MapSystem mapSystem;

        private Data.Map mapData;

        private List<Data.Room> rooms;
        private List<RectInt> placeholders;

        private Dictionary<Type.Group, Data.ColonyBase> colonyBases;

        private int2 selectedCell;
        public int2 SelectedCell => selectedCell;

        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<System.MapSystem>();

            mapData = new Data.Map
            {
                Size = Info.Map.Size,
                Cells = new Data.Cell[Info.Map.Area],
                Edges = new int[Info.Map.Area * Info.Map.Area],
            };

            selectedCell = new int2();

            rooms = new List<Data.Room>(Info.Map.NumberOfSeedRooms);
            placeholders = new List<RectInt>();
           
            colonyBases = new Dictionary<Type.Group, Data.ColonyBase>();

            SetupCells();
            SetupBase();
            SetupPaths();

            RoomBuilder.LayoutRooms(rooms, placeholders);

            SetupRooms();
            SetupColonyBases();

            CalculateEdges();

            ConstructMap();
        }


        public Data.Cell[] GetCells()
        {
            return mapData.Cells;
        }


        public int[] GetEdgeData()
        {
            return mapData.Edges;
        }


        public Data.Cell GetCell(int x, int y)
        {
            return mapData.Cells[Util.Map.PositionToIndex(x, y)];
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
            mapData.Cells[Util.Map.PositionToIndex(x, y)] = cellData;
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
        }


        private void SetupCells()
        {
            selectedCell = new int2();

            for (int x = -Info.Map.Size; x <= Info.Map.Size; x++)
            {
                for (int y = -Info.Map.Size; y <= Info.Map.Size; y++)
                {
                    Data.Cell cellData = new Data.Cell
                    {
                        Index = Util.Map.PositionToIndex(x, y),
                        Position = new int2(x, y),

                        Solid = false,

                        GroundType = Type.Ground.None,
                        StructureType = Type.Structure.None,
                        OverlayType = Type.Overlay.None,
                    };

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

            foreach (Data.ColonyBase colonyBaseData in colonyBases.Values)
            {
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


        public void CalculateEdges()
        {
            ResetEdges();

            for (int x = -Info.Map.Size + 2; x <= Info.Map.Size - 2; x++)
            {
                for (int y = -Info.Map.Size + 2; y <= Info.Map.Size - 2; y++)
                {
                    Data.Cell cellData = GetCell(x, y);

                    if (cellData.Solid) continue;

                    Data.Cell cellDataEE = GetCell(x + 1, y + 0);
                    Data.Cell cellDataNE = GetCell(x + 1, y + 1);
                    Data.Cell cellDataNN = GetCell(x + 0, y + 1);
                    Data.Cell cellDataNW = GetCell(x - 1, y + 1);
                    Data.Cell cellDataWW = GetCell(x - 1, y + 0);
                    Data.Cell cellDataSW = GetCell(x - 1, y - 1);
                    Data.Cell cellDataSS = GetCell(x + 0, y - 1);
                    Data.Cell cellDataSE = GetCell(x + 1, y - 1);

                    if (!cellDataEE.Solid)
                    {
                        AddEdge(cellData, cellDataEE, Info.Map.StraightMovementCost);
                    }

                    if (!cellDataNE.Solid && !cellDataNN.Solid)
                    {
                        AddEdge(cellData, cellDataNE, Info.Map.DiagonalMovementCost);
                    }

                    if (!cellDataNN.Solid)
                    {
                        AddEdge(cellData, cellDataNN, Info.Map.StraightMovementCost);
                    }

                    if (!cellDataNN.Solid && !cellDataNW.Solid && !cellDataWW.Solid)
                    {
                        AddEdge(cellData, cellDataNW, Info.Map.DiagonalMovementCost);
                    }

                    if (!cellDataWW.Solid)
                    {
                        AddEdge(cellData, cellDataWW, Info.Map.StraightMovementCost);
                    }

                    if (!cellDataWW.Solid && !cellDataSW.Solid && !cellDataSS.Solid)
                    {
                        AddEdge(cellData, cellDataSW, Info.Map.DiagonalMovementCost);
                    }

                    if (!cellDataSS.Solid)
                    {
                        AddEdge(cellData, cellDataSS, Info.Map.StraightMovementCost);
                    }

                    if (!cellDataSS.Solid && !cellDataSE.Solid && !cellDataEE.Solid)
                    {
                        AddEdge(cellData, cellDataSE, Info.Map.DiagonalMovementCost);
                    }
                }
            }
        }


        private void AddEdge(Data.Cell cellData1, Data.Cell cellData2, int weight)
        {
            int forwardEdgeIndex = Util.Map.EdgeToIndex(cellData1.Index, cellData2.Index);
            int backwardEdgeIndex = Util.Map.EdgeToIndex(cellData2.Index, cellData1.Index); 

            mapData.Edges[forwardEdgeIndex] = weight;
            mapData.Edges[backwardEdgeIndex] = weight;
        }


        private void ResetEdges()
        {
            mapData.Edges = new int[Info.Map.Area * Info.Map.Area];
        }


        public void ConstructMap()
        {
            foreach (Data.Cell cell in mapData.Cells)
            {
                mapSystem.RenderCell(cell);
            }
        }
    }
}