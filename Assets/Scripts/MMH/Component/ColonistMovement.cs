using System.Collections.Generic;
using System.Linq;
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
                        int2 offset = new int2(0, 0);

                        List<int2> offsets = Info.Map.DirectionOffsets.Values.ToList();

                        while (offsets.Count > 0)
                        {
                            int testOffsetIndex = UnityEngine.Random.Range(0, offsets.Count - 1);
                            int2 testOffset = offsets[testOffsetIndex];

                            offsets.RemoveAt(testOffsetIndex);

                            int edgeValue = mapSystem.Map.GetEdge(
                                colonist.Entity.Position,
                                colonist.Entity.Position + testOffset
                            );

                            if (edgeValue != 0)
                            {
                                offset = testOffset;
                                break;
                            }
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
                    colonist.Entity.Speed = Info.Entity.DefaultWalkSpeed;

                    colonist.Entity.Direction = Util.Map.GetCardinalDirection(
                        colonist.Path.Positions[colonist.Path.Index] - colonist.Entity.Position
                    );
                }
                else
                {
                    colonist.Entity.Speed = 0;
                }
            }
        }
    }
}


