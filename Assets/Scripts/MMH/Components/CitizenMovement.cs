using UnityEngine;

namespace MMH
{
    public class CitizenMovement : MonoBehaviour
    {
        private Citizen citizen;


        void Awake()
        {
            citizen = GetComponent<Citizen>();
        }


        void Update()
        {
            if (citizen.Path.Valid)
            {
                FollowPath();
            }
        }


        private void FollowPath()
        {
            if (citizen.Path.Progress < 1.0f)
            {
                citizen.Path.Progress += Time.deltaTime;

                Vector2 isoPosition = Vector2.Lerp(
                    citizen.Entity.Position, citizen.Path.Nodes[0].Position, citizen.Path.Progress
                );

                transform.position = Util.Map.IsoToWorld(isoPosition);
            }
            else
            {
                citizen.Entity.Position = citizen.Path.Nodes[0].Position;

                citizen.Path.Progress = 0f;
                citizen.Path.Nodes.RemoveAt(0);

                if (citizen.Path.Nodes.Count > 0)
                {
                    citizen.Entity.Speed = Info.Entity.DefaultWalkSpeed;
                    citizen.Entity.Direction = Util.Map.CardinalDirection(
                        citizen.Path.Nodes[0].Position - citizen.Entity.Position
                    );
                }
                else
                {
                    citizen.Entity.Speed = 0;
                }
            }
        }
    }
}


