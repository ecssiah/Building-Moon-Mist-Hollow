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

            path = new Data.Path();
        }


        void Start()
        {
            path = entitySystem.RequestPath(
                citizen.Entity.GridPosition, new Vector2Int(0, 0)
            );

            if (path.Valid)
            {
                citizen.Entity.Speed = Info.Entity.DefaultWalkSpeed;
            }
        }


        void Update()
        {
            if (path.Valid)
            {
                Decide();
                Move();
            }
            else
            {
                citizen.Entity.Speed = 0f;
            }
        }


        private void Decide()
        {
            path.Progress += Time.deltaTime * citizen.Entity.Speed;

            if (path.Progress >= 1f)
            {
                path.Progress = 0f;
            }

            citizen.Entity.Position = Vector2.MoveTowards(
                citizen.Entity.Position,
                path.Nodes[0].Position,
                Time.deltaTime * citizen.Entity.Speed
            );

            if (Vector2.Distance(citizen.Entity.Position, path.Nodes[0].Position) < .001f)
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


