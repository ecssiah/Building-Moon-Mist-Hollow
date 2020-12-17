using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private NamingSystem namingSystem;

    private GameObject[] entities;

    private GameObject citizenPrefab;


    void Awake()
    {
        namingSystem = GetComponent<NamingSystem>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        GenerateCitizen(new Vector3(-2, 2, 0));
        GenerateCitizen(new Vector3(-2, -2, 0));
        GenerateCitizen(new Vector3(2, -2, 0));
        GenerateCitizen(new Vector3(2, 2, 0));

        entities = new GameObject[this.transform.childCount];

        int i = 0;
        foreach (Transform transform in this.transform)
        {
            entities[i++] = transform.gameObject;
        }
    }


    private void GenerateCitizen(Vector3 position)
    {
        Vector3 worldPosition = MapUtil.IsoToWorld(position);

        GameObject newCharacterObject = Instantiate(
            citizenPrefab, worldPosition, Quaternion.identity
        );

        newCharacterObject.transform.parent = this.transform;

        newCharacterObject.name = namingSystem.GetName();
        newCharacterObject.layer = LayerMask.NameToLayer("Citizens");

        newCharacterObject.AddComponent<CitizenMovement>();
        newCharacterObject.AddComponent<CitizenAnimator>();
    }
}
