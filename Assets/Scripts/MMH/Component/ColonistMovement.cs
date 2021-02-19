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
            if (colonist.Path.Active)
            {
                FollowPath();
            }
            else
            {
                switch (colonist.Behavior)
                {
                    case Type.Behavior.Wander:
                        int2 offset = new int2(UnityEngine.Random.Range(-3, 4), UnityEngine.Random.Range(-3, 4));

                        colonist.FindPath(colonist.Entity.Position + offset);

                        print("Wander");
                        print(colonist.Path);

                        break;
                }
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


