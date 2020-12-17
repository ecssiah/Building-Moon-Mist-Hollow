using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private NamingSystem namingSystem;

    private GameObject citizenPrefab;

    private GameObject[] entities;
    public GameObject[] Entities { get => entities; }

    void Awake()
    {
        namingSystem = GetComponent<NamingSystem>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        GenerateCitizen(new Vector2( 2, 2));
        GenerateCitizen(new Vector2(-2, 2));
        GenerateCitizen(new Vector2( 2, -2));
        GenerateCitizen(new Vector2(-2, -2));

        entities = new GameObject[transform.childCount];

        int i = 0;
        foreach (Transform transform in transform)
        {
            entities[i++] = transform.gameObject;
        }
    }


    private void GenerateCitizen(Vector2 position)
    {
        Vector2 worldPosition = MapUtil.IsoToWorld(position);

        GameObject newCharacterObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        newCharacterObject.transform.parent = transform;

        newCharacterObject.name = namingSystem.GetName();
        newCharacterObject.layer = LayerMask.NameToLayer("Citizens");

        newCharacterObject.AddComponent<CitizenMovement>();
        newCharacterObject.AddComponent<CitizenAnimator>();
    }
}
