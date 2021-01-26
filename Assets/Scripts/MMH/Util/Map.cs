using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMH.Util
{
    public struct Map
    {
        public static int PositionToIndex(int2 position)
        {
            return PositionToIndex(position.x, position.y);
        }


        public static int PositionToIndex(int x, int y)
        {
            return x + Info.Map.Size + Info.Map.Width * (y + Info.Map.Size);
        }


        public static int EdgeToIndex(Data.Node node1, Data.Node node2)
        {
            return node1.Index + Info.Map.Area * node2.Index;
        }


        public static int2 IndexToPosition(int i)
        {
            return new int2(
                (i % Info.Map.Width) - Info.Map.Size,
                (i / Info.Map.Width) - Info.Map.Size
            );
        }


        public static float2 WorldToIso(float2 worldPosition)
        {
            return new float2(
                worldPosition.x + 2 * worldPosition.y,
                -worldPosition.x + 2 * worldPosition.y
            );
        }


        public static int2 WorldToIsoGrid(float2 screenPosition)
        {
            float2 isoVector = WorldToIso(screenPosition);

            return new int2(
                (int)math.floor(isoVector.x),
                (int)math.floor(isoVector.y)
            );
        }


        public static float2 WorldToScreen(float2 worldPosition)
        {
            return RectTransformUtility.WorldToScreenPoint(
                Camera.main, new Vector3(worldPosition.x, worldPosition.y, 0)
            );
        }


        public static int2 ScreenToIsoGrid(float2 screenPosition)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(screenPosition.x, screenPosition.y, 0)
            );
            worldPosition.y += 0.25f;

            return WorldToIsoGrid(worldPosition);
        }


        public static float2 IsoToWorld(float2 isoVector)
        {
            float2x2 isoTransform = new float2x2(
                0.50f, -0.50f,
                0.25f,  0.25f
            );

            return math.mul(isoTransform, isoVector);
        }


        public static bool OnRectBoundary(int x, int y, RectInt rect)
        {
            bool topEdge = y == rect.yMax;
            bool bottomEdge = y == rect.yMin;
            bool leftEdge = x == rect.xMin;
            bool rightEdge = x == rect.xMax;

            return topEdge || bottomEdge || leftEdge || rightEdge;
        }


        public static bool OnMap(int x, int y)
        {
            return OnMap(new int2(x, y));
        }


        public static bool OnMap(int2 position)
        {
            RectInt mapBoundary = new RectInt
            {
                x = -Info.Map.Size,
                y = -Info.Map.Size,
                width = Info.Map.Width,
                height = Info.Map.Width,
            };

            return mapBoundary.Contains(new Vector2Int(position.x, position.y));
        }


        public static bool OnMap(RectInt bounds)
        {
            return OnMap(bounds.xMin, bounds.yMin) && OnMap(bounds.xMax, bounds.yMax);
        }


        public static bool EntranceExistsAt(int x, int y, Data.Room room)
        {
            return EntranceExistsAt(new int2(x, y), room);
        }


        public static bool EntranceExistsAt(int2 position, Data.Room room)
        {
            foreach (Data.Entrance entrance in room.Entrances)
            {
                if (entrance.Bounds.Contains(new Vector2Int(position.x, position.y)))
                {
                    return true;
                }
            }

            return false;
        }


        public static int2 GetRandomMapPosition()
        {
            return new int2(
                Random.Range(-Info.Map.Size, Info.Map.Size),
                Random.Range(-Info.Map.Size, Info.Map.Size)
            );
        }


        public static float2 RandomIsoDirection()
        {
            float2 newIsoDirection = new float2(Random.Range(-1, 2), Random.Range(-1, 2));

            return IsoToWorld(newIsoDirection);
        }


        public static int2 GetRandomBorderPosition(RectInt bounds, bool includeCorners = false)
        {
            int cornerModifier = includeCorners ? 0 : 1;

            switch (Random.Range(0, 4))
            {
                case 0:
                    int2 topWallPosition = new int2(
                        Random.Range(bounds.xMin + cornerModifier, bounds.xMax - cornerModifier),
                        bounds.yMax
                    );

                    return topWallPosition;
                case 1:
                    int2 bottomWallPosition = new int2(
                        Random.Range(bounds.xMin + cornerModifier, bounds.xMax - cornerModifier),
                        bounds.yMin
                    );

                    return bottomWallPosition;
                case 2:
                    int2 leftWallPosition = new int2(
                        bounds.xMin,
                        Random.Range(bounds.yMin + cornerModifier, bounds.yMax - cornerModifier)
                    );

                    return leftWallPosition;
                case 3:
                    int2 rightWallPosition = new int2(
                        bounds.xMax,
                        Random.Range(bounds.yMin + cornerModifier, bounds.yMax - cornerModifier)
                    );

                    return rightWallPosition;
                default:
                    return new int2(bounds.xMin + 1, bounds.yMin);
            }
        }


        public static Type.Direction CardinalDirection(float2 direction)
        {
            if (direction.x == 0 && direction.y > 0)
            {
                return Type.Direction.NN;
            }
            else if (direction.x == 0 && direction.y < 0)
            {
                return Type.Direction.SS;
            }
            else if (direction.y == 0 && direction.x > 0)
            {
                return Type.Direction.EE;
            }
            else if (direction.y == 0 && direction.x < 0)
            {
                return Type.Direction.WW;
            }
            else if (direction.x > 0 && direction.y > 0)
            {
                return Type.Direction.NE;
            }
            else if (direction.x < 0 && direction.y > 0)
            {
                return Type.Direction.NW;
            }
            else if (direction.x > 0 && direction.y < 0)
            {
                return Type.Direction.SE;
            }
            else if (direction.x < 0 && direction.y < 0)
            {
                return Type.Direction.SW;
            }

            return Type.Direction.SS;
        }
    }
}

