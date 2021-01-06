using UnityEngine;


public struct EntityData
{
    public GameObject Entity;

    public Vector2 Position;
    public Vector2Int GridPosition => new Vector2Int((int)Position.x, (int)Position.y);

    public float Speed;
    public Vector2 Direction;
}