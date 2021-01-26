using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.System
{
    public class MapSystem : MonoBehaviour
    {
        public static readonly Dictionary<Type.Direction, int2> Directions = new Dictionary<Type.Direction, int2>
        {
            [Type.Direction.EE] = new int2(+1, +0),
            [Type.Direction.NE] = new int2(+1, +1),
            [Type.Direction.NN] = new int2(+0, +1),
            [Type.Direction.NW] = new int2(-1, +1),
            [Type.Direction.WW] = new int2(-1, +0),
            [Type.Direction.SW] = new int2(-1, -1),
            [Type.Direction.SS] = new int2(+0, -1),
            [Type.Direction.SE] = new int2(+1, -1),
        };

        private RenderSystem renderSystem;

        private int size;
        public int Size => size;

        private int width;
        public int Width => width;

        private bool edgesValid;

        private int2 selectedCell;

        private List<Data.Cell> cells;
        private List<Data.Room> rooms;
        private List<int> edges;
        private List<RectInt> placeholders;

        private Dictionary<Type.Group, Data.ColonyBase> colonyBases = new Dictionary<Type.Group, Data.ColonyBase>();

        void Awake()
        {
            renderSystem = GameObject.Find("RenderSystem").GetComponent<RenderSystem>();

            size = Info.Map.Size;
            width = Info.Map.Width;

            edgesValid = false;

            selectedCell = new int2();

            cells = Enumerable.Repeat(new Data.Cell(), Info.Map.Area).ToList();
            edges = new List<int>(Info.Map.Area * Info.Map.Area);
            rooms = new List<Data.Room>(Info.Map.NumberOfSeedRooms);
            placeholders = new List<RectInt>();

            SetupCells();
            SetupBase();
            SetupPaths();

            RoomBuilder.LayoutRooms(rooms, placeholders);

            SetupRooms();
            SetupColonyBases();

            ConstructMap();
        }


        private void SetupCells()
        {
            selectedCell = new int2();

            for (int i = 0; i < Info.Map.Area; i++)
            {
                Data.Cell cellData = cells[i];
                cellData.Index = i;
                cellData.Position = Util.Map.IndexToPosition(i);

                cells[i] = cellData;
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



        // Construction Methods

        public void ConstructMap()
        {
            foreach (Data.Cell cell in cells)
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
            return cells[Util.Map.PositionToIndex(x, y)];
        }


        public Data.Cell GetCell(int index)
        {

            return cells[index];
        }


        public Data.Cell GetCell(int2 position)
        {
            return GetCell(position.x, position.y);
        }


        public List<Data.Cell> GetCells()
        {
            return cells;
        }


        public List<bool> GetSolidData()
        {
            List<bool> solidData = new List<bool>(cells.Count);

            for (int i = 0; i < cells.Count; i++)
            {
                Data.Cell cellData = cells[i];

                solidData.Add(cellData.Solid);
            }

            return solidData;
        }


        public List<int> GetEdgeData()
        {
            if (!edgesValid)
            {
                ResetEdges();

                for (int x = -Info.Map.Size + 2; x <= Info.Map.Size - 2; x++)
                {
                    for (int y = -Info.Map.Size + 2; y <= Info.Map.Size - 2; y++)
                    {
                        Data.Cell currentCell = GetCell(x, y);

                        if (currentCell.Solid)
                        {
                            continue;
                        }

                        foreach (KeyValuePair<Type.Direction, int2> keyValuePair in MapSystem.Directions)
                        {
                            Type.Direction neighborDirection = keyValuePair.Key;
                            int2 neighborOffset = keyValuePair.Value;

                            int2 neighborPosition = currentCell.Position + neighborOffset;

                            if (Util.Map.OnMap(neighborPosition))
                            {
                                Data.Cell neighborCell = GetCell(neighborPosition);

                                if (neighborCell.Solid)
                                {
                                    continue;
                                }

                                if (ValidEdge(neighborCell, neighborDirection))
                                {
                                    int currentCellIndex = Util.Map.PositionToIndex(currentCell.Position);
                                    int neighborCellIndex = Util.Map.PositionToIndex(neighborCell.Position);

                                    edges[currentCellIndex + Info.Map.Area * neighborCellIndex] = 1;
                                    edges[neighborCellIndex + Info.Map.Area * currentCellIndex] = 1;
                                }
                            }
                        }
                    }
                }

                edgesValid = true;
            }

            return edges;
        }


        private bool ValidEdge(Data.Cell neighborCell, Type.Direction neighborDirection)
        {
            int2 northPosition = neighborCell.Position + MapSystem.Directions[Type.Direction.NN];
            int2 eastPosition = neighborCell.Position + MapSystem.Directions[Type.Direction.EE];
            int2 southPosition = neighborCell.Position + MapSystem.Directions[Type.Direction.SS];
            int2 westPosition = neighborCell.Position + MapSystem.Directions[Type.Direction.WW];

            bool northSolid = !Util.Map.OnMap(northPosition) || cells[Util.Map.PositionToIndex(northPosition)].Solid;
            bool eastSolid = !Util.Map.OnMap(eastPosition) || cells[Util.Map.PositionToIndex(eastPosition)].Solid;
            bool southSolid = !Util.Map.OnMap(southPosition) || cells[Util.Map.PositionToIndex(southPosition)].Solid;
            bool westSolid = !Util.Map.OnMap(westPosition) || cells[Util.Map.PositionToIndex(westPosition)].Solid;

            if (neighborDirection == Type.Direction.NE)
            {
                if (northSolid || eastSolid)
                {
                    return false;
                }
            }

            if (neighborDirection == Type.Direction.SE)
            {
                if (southSolid || eastSolid)
                {
                    return false;
                }
            }

            if (neighborDirection == Type.Direction.NW)
            {
                if (northSolid || westSolid)
                {
                    return false;
                }
            }

            if (neighborDirection == Type.Direction.SW)
            {
                if (southSolid || westSolid)
                {
                    return false;
                }
            }

            return true;
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



        // Set Methods


        public void SetCell(int x, int y, Data.Cell cellData)
        {
            cells[Util.Map.PositionToIndex(x, y)] = cellData;
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


        public void ResetEdges()
        {
            edgesValid = false;
            edges = Enumerable.Repeat(0, cells.Count * cells.Count).ToList();
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
            //using (StreamWriter file = File.CreateText($"Assets/Resources/Data/{name}.json"))
            //{
            //    string jsonCellsText = JsonUtility.ToJson(mapData, true);

            //    file.Write(jsonCellsText);
            //}
        }


        public void LoadMapData(string name)
        {
            //using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
            //{
            //    string jsonText = reader.ReadToEnd();

            //    mapData = JsonUtility.FromJson<Data.Map>(jsonText);
            //}
        }
    }
}