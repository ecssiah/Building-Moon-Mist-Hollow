using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EntityMoveOrderSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Entities.ForEach(
                (Entity entity, ref Translation translation) =>
                {
                    EntityManager.AddComponentData(entity, new PathfindingParams
                    {
                        StartPosition = new int2(0, 0),
                        EndPosition = new int2(4, 0),
                    });
                }
            );
        }
    }
}
