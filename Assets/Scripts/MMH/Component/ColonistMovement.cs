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
            switch (colonist.Praxis.Movement)
            {
                case Type.Behavior.Movement.Wander:
                    Wander();
                    break;

                case Type.Behavior.Movement.Gather:
                    Gather();
                    break;

                case Type.Behavior.Movement.None:
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
                    colonist.Entity.Speed = Info.Entity.DefaultWalkSpeed;
                    colonist.Entity.Direction = Util.Map.GetCardinalDirection(
                        colonist.Path.Positions.Peek() - colonist.Entity.Position
                    );
                }
                else
                {
                    colonist.Entity.Speed = 0;
                }
            }
        }


        private void Gather()
        {
            if (colonist.HasPath())
            {
                FollowPath();
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
                colonist.Path.StepProgress = 0;

                colonist.Entity.Position = colonist.Path.Positions.Pop();

                if (colonist.HasPath())
                {
                    colonist.Entity.Direction = Util.Map.GetCardinalDirection(
                        colonist.Path.Positions.Peek() - colonist.Entity.Position
                    );
                }
            }
        }
    }
}


