using System.Collections;
using System.Collections.Generic;
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


        public void SetTile(Vector2Int position, Type.Ground groundType)
        {
            tilemaps["Ground"].SetTile(
                new Vector3Int(position.x, position.y, 0),
                tiles[Info.Tile.groundTileNames[groundType]]
            );
        }

        public void SetTile(Vector2Int position, Type.Structure wallType)
        {
            tilemaps["Walls"].SetTile(
                new Vector3Int(position.x, position.y, 3),
                tiles[Info.Tile.wallTileNames[wallType]]
            );
        }

        public void SetTile(Vector2Int position, Type.Overlay overlayType)
        {
            tilemaps["Overlay"].SetTile(
                new Vector3Int(position.x, position.y, 0),
                tiles[Info.Tile.overlayTileNames[overlayType]]
            );
        }


        public void ClearTile(Vector2Int position, string layer)
        {
            tilemaps[layer].SetTile(new Vector3Int(position.x, position.y, 0), null);
        }
    }
}

