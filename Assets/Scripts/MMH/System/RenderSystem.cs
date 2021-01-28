using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH.System
{
    public class RenderSystem : MonoBehaviour
    {
        private Dictionary<string, Tile> tiles;
        private Dictionary<string, Tilemap> tilemaps;


        void Awake()
        {
            Application.targetFrameRate = Info.Render.FPSTarget;

            InitTiles();
            InitTilemaps();
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
            collisionRenderer.enabled = Info.Map.ShowCollision;
        }


        public void SetCell(Data.Cell cellData)
        {
            SetGround(cellData.Position, cellData.GroundType);
            SetStructure(cellData.Position, cellData.StructureType);
            SetOverlay(cellData.Position, cellData.OverlayType);
        }


        public void SetGround(int2 position, Type.Ground groundType)
        {
            if (groundType == Type.Ground.None)
            {
                tilemaps["Ground"].SetTile(new Vector3Int(position.x, position.y, 0), null);
            }
            else
            {
                tilemaps["Ground"].SetTile(
                    new Vector3Int(position.x, position.y, 0),
                    tiles[Info.Tile.GroundTileNames[groundType]]
                );
            }
        }

        public void SetStructure(int2 position, Type.Structure structureType)
        {
            if (structureType == Type.Structure.None)
            {
                tilemaps["Structure"].SetTile(new Vector3Int(position.x, position.y, 0), null);
            }
            else
            {
                tilemaps["Structure"].SetTile(
                    new Vector3Int(position.x, position.y, 3),
                    tiles[Info.Tile.StructureTileNames[structureType]]
                );
            }
        }

        public void SetOverlay(int2 position, Type.Overlay overlayType)
        {
            if (overlayType == Type.Overlay.None)
            {
                tilemaps["Overlay"].SetTile(new Vector3Int(position.x, position.y, 0), null);
            }
            else
            {
                tilemaps["Overlay"].SetTile(
                    new Vector3Int(position.x, position.y, 0),
                    tiles[Info.Tile.OverlayTileNames[overlayType]]
                );
            }
        }


        public void ClearTile(int2 position, string layer)
        {
            tilemaps[layer].SetTile(new Vector3Int(position.x, position.y, 0), null);
        }
    }
}

