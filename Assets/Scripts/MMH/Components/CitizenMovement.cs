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

            path = new Data.Path { Valid = false };
        }


        void Start()
        {
            Debug.Log($"{citizen.Id.FullName} - {citizen.Entity.GridPosition}");

            path = entitySystem.RequestPath(citizen.Entity.GridPosition, new Vector2Int(4, 4));

            Debug.Log(path);
        }


        void Update()
        {
            Decide();
            Move();
        }


        private void Decide()
        {
            if (Vector2.Distance(citizen.Entity.Position, path.Nodes[0].Position) < 0.1f)
            {
                path.Nodes.RemoveAt(0);
            }

            path.Progress += Time.deltaTime * citizen.Entity.Speed;

            if (path.Progress >= 1f)
            {
                path.Progress = 0f;
            }

            citizen.Entity.Position = Vector2.Lerp(citizen.Entity.Position, path.Nodes[0].Position, path.Progress);
            citizen.Entity.Direction = path.Nodes[0].Position - citizen.Entity.Position;
        }


        private void Move()
        {
            Vector2 unscaledVelocity = citizen.Entity.Speed * citizen.Entity.Direction;
            Vector3 velocity = Time.deltaTime * new Vector3(unscaledVelocity.x, unscaledVelocity.y, 0);

            transform.position += velocity;
        }
    }
}


