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
            Vector2Int targetPosition = Random.Range(0, 2) == 0 ?
                new Vector2Int(-Info.Map.Size + 1, -Info.Map.Size + 1) :
                new Vector2Int(Info.Map.Size - 1, Info.Map.Size - 1);

            path = entitySystem.RequestPath(citizen.Entity.GridPosition, targetPosition);

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
            citizen.Entity.Position = Vector2.MoveTowards(
                citizen.Entity.Position, path.Nodes[0].Position, Time.deltaTime * citizen.Entity.Speed
            );

            if (Vector2.Distance(citizen.Entity.Position, path.Nodes[0].Position) < .001f)
            {
                path.Nodes.RemoveAt(0);

                if (path.Valid)
                {
                    citizen.Entity.Direction = citizen.Entity.Position - path.Nodes[0].Position;
                }
            }
        }


        private void Move()
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(citizen.Entity.Position);

            transform.position = worldPosition;
        }
    }
}


