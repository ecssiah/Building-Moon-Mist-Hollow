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
            Vector2 unscaledVelocity = citizen.Entity.Speed * citizen.Entity.Direction;
            Vector3 velocity = Time.deltaTime * new Vector3(unscaledVelocity.x, unscaledVelocity.y, 0);

            transform.position += velocity;
        }


        private Vector2 DecideVelocity()
        {
            return new Vector2(0, 0);
        }
    }
}


