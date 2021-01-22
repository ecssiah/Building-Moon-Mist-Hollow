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


        public void SetTile(int2 position, Type.Ground groundType)
        {
            tilemaps["Ground"].SetTile(
                new Vector3Int(position.x, position.y, 0),
                tiles[Info.Tile.groundTileNames[groundType]]
            );
        }

        public void SetTile(int2 position, Type.Structure structureType)
        {
            tilemaps["Structure"].SetTile(
                new Vector3Int(position.x, position.y, 3),
                tiles[Info.Tile.structureTileNames[structureType]]
            );
        }

        public void SetTile(int2 position, Type.Overlay overlayType)
        {
            tilemaps["Overlay"].SetTile(
                new Vector3Int(position.x, position.y, 0),
                tiles[Info.Tile.overlayTileNames[overlayType]]
            );
        }


        public void ClearTile(int2 position, string layer)
        {
            tilemaps[layer].SetTile(new Vector3Int(position.x, position.y, 0), null);
        }
    }
}

