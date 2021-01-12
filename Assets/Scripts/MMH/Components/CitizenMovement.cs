using UnityEngine;

namespace MMH
{
    public class CitizenMovement : MonoBehaviour
    {
        private System.EntitySystem entitySystem;

        private Citizen citizen;

        private Data.Path path;


        void Awake()
        {
            entitySystem = GameObject.Find("EntitySystem").GetComponent<System.EntitySystem>();

            citizen = GetComponent<Citizen>();
        }


        void Start()
        {
            path = entitySystem.RequestPath(citizen.Entity.Position, Vector2Int.zero);

            if (path.Valid)
            {
                citizen.Entity.Speed = Info.Entity.DefaultWalkSpeed;
            }
        }


        void Update()
        {
            if (path.Valid)
            {
                FollowPath();
            }
        }


        private void FollowPath()
        {
            if (path.Progress < 1.0f)
            {
                path.Progress += Time.deltaTime;

                Vector2 isoPosition = Vector2.Lerp(
                    citizen.Entity.Position, path.Nodes[0].Position, path.Progress
                );

                transform.position = Util.Map.IsoToWorld(isoPosition);
            }
            else
            {
                path.Progress = 0f;

                citizen.Entity.Position = path.Nodes[0].Position;

                path.Nodes.RemoveAt(0);
            }
        }
    }
}


