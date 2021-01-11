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
            path = entitySystem.RequestPath(citizen.Entity.GridPosition, new Vector2Int(7, 4));

            if (path.Valid)
            {
                citizen.Entity.Speed = Info.Entity.DefaultWalkSpeed;
            }
        }


        void Update()
        {
            Decide();
            Move();
        }


        private void Decide()
        {
            if (path.Valid)
            {
                FollowPath();
            }
        }


        private void FollowPath()
        {
            citizen.Entity.Position = Vector2.MoveTowards(
                citizen.Entity.Position, path.Nodes[0].Position, Time.deltaTime * citizen.Entity.Speed
            );
            citizen.Entity.Direction = citizen.Entity.Position - path.Nodes[0].Position;

            if (Vector2.Distance(citizen.Entity.Position, path.Nodes[0].Position) < .01f)
            {
                path.Nodes.RemoveAt(0);
            }
        }


        private void Move()
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(citizen.Entity.Position);

            transform.position = worldPosition;
        }
    }
}


