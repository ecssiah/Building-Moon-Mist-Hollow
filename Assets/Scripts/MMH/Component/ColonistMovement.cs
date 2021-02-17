using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Component
{
    public class ColonistMovement : MonoBehaviour
    {
        private Colonist colonist;

        private System.MapSystem mapSystem;


        void Awake()
        {
            colonist = GetComponent<Colonist>();

            mapSystem = GameObject.Find("Map System").GetComponent<System.MapSystem>();
        }


        void Update()
        {
            switch (colonist.Behavior)
            {
                case Type.Behavior.Wander:
                    if (colonist.Path.Active)
                    {
                        FollowPath();
                    }
                    else
                    {
                        int timeout = 0;
                        int2 offset = new int2(0, 0);

                        while (timeout++ < 12 && mapSystem.Map.GetEdge(colonist.Entity.Position, colonist.Entity.Position + offset) == 0)
                        {
                            offset = Info.Map.DirectionOffsets[Util.Map.GetRandomCardinalDirection()];
                        }

                        colonist.Path.StepProgress = 0;

                        colonist.Path.Index = 0;
                        colonist.Path.Positions = new List<int2>
                        {
                            colonist.Entity.Position,
                            colonist.Entity.Position + offset
                        };
                    }

                    break;

                case Type.Behavior.Gather:
                    break;

                case Type.Behavior.None:
                    break;

                default:
                    break;
            }
        }


        private void FollowPath()
        {
            if (colonist.Path.StepProgress < 1.0f)
            {
                colonist.Path.StepProgress += Time.deltaTime;

                float2 isoPosition = new float2 (
                    math.lerp (
                        colonist.Entity.Position.x,
                        colonist.Path.Positions[colonist.Path.Index].x,
                        colonist.Path.StepProgress
                    ),
                    math.lerp (
                        colonist.Entity.Position.y,
                        colonist.Path.Positions[colonist.Path.Index].y,
                        colonist.Path.StepProgress
                    )
                );

                float2 worldPosition = Util.Map.IsoToWorld(isoPosition);

                transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            }
            else
            {
                colonist.Entity.Position = colonist.Path.Positions[colonist.Path.Index];

                colonist.Path.Index++;
                colonist.Path.StepProgress = 0;

                if (colonist.Path.Index < colonist.Path.Positions.Count)
                {
                    colonist.Entity.Direction = Util.Map.GetCardinalDirection(
                        colonist.Path.Positions[colonist.Path.Index] - colonist.Entity.Position
                    );
                }
            }
        }
    }
}


