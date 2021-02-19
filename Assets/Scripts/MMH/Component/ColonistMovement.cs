using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Component
{
    public class ColonistMovement : MonoBehaviour
    {
        private Colonist colonist;


        private void Awake()
        {
            colonist = GetComponent<Colonist>();
        }


        void Update()
        {
            switch (colonist.Behavior)
            {
                case Type.Behavior.Wander:
                    Wander();
                    break;

                default:
                    break;
            }
        }


        private void Wander()
        {
            if (colonist.HasPath())
            {
                FollowPath();
            }
            else
            {
                int2 offset = new int2(
                    UnityEngine.Random.Range(-3, 4),
                    UnityEngine.Random.Range(-3, 4)
                );

                colonist.FindPath(colonist.Entity.Position + offset);

                if (colonist.HasPath())
                {
                    colonist.Entity.Direction = Util.Map.GetCardinalDirection(
                        colonist.Path.Positions.Peek() - colonist.Entity.Position
                    );
                }

                print(colonist.Path);
            }
        }


        private void FollowPath()
        {
            if (colonist.Path.StepProgress < 1.0f)
            {
                int2 target = colonist.Path.Positions.Peek();

                float2 isoPosition = Util.Math.Lerp2(
                    colonist.Entity.Position, target, colonist.Path.StepProgress
                );
                float2 worldPosition = Util.Map.IsoToWorld(isoPosition);

                transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);

                colonist.Path.StepProgress += Info.Entity.DefaultWalkSpeed * Time.deltaTime;
            }
            else
            {
                colonist.Entity.Position = colonist.Path.Positions.Pop();

                colonist.Path.StepProgress = 0;

                if (colonist.Path.Positions.Count > 0)
                {
                    colonist.Entity.Direction = Util.Map.GetCardinalDirection(
                        colonist.Path.Positions.Peek() - colonist.Entity.Position
                    );
                }
            }
        }
    }
}


