using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class ColonistMovement : MonoBehaviour
    {
        private Colonist colonist;


        void Awake()
        {
            colonist = GetComponent<Colonist>();
        }


        void Update()
        {
            if (colonist.Path.Valid)
            {
                FollowPath();
            }
        }


        private void FollowPath()
        {
            if (colonist.Path.Progress < 1.0f)
            {
                colonist.Path.Progress += Time.deltaTime;

                float2 isoPosition = new float2 (
                    math.lerp (
                        colonist.Entity.Position.x,
                        colonist.Path.Nodes[0].Position.x,
                        colonist.Path.Progress
                    ),
                    math.lerp (
                        colonist.Entity.Position.y,
                        colonist.Path.Nodes[0].Position.y,
                        colonist.Path.Progress
                    )
                );

                float2 worldPosition = Util.Map.IsoToWorld(isoPosition);

                transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            }
            else
            {
                colonist.Entity.Position = colonist.Path.Nodes[0].Position;

                colonist.Path.Progress = 0f;
                colonist.Path.Nodes.RemoveAt(0);

                if (colonist.Path.Nodes.Count > 0)
                {
                    colonist.Entity.Speed = Info.Entity.DefaultWalkSpeed;
                    colonist.Entity.Direction = Util.Map.CardinalDirection(
                        colonist.Path.Nodes[0].Position - colonist.Entity.Position
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


