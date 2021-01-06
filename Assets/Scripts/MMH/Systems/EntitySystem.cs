using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private WorldMap worldMap;

    private PopulationData populationData;

    private NameGenerator nameGenerator;

    private GameObject entitiesObject;
    private GameObject citizenPrefab;


    void Awake()
    {
        nameGenerator = gameObject.AddComponent<NameGenerator>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        entitiesObject = GameObject.Find("Entities");

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );
    }


    void Start()
    {
        worldMap = GameObject.Find("MapSystem").GetComponent<WorldMap>();

        for (int i = 0; i < EntityInfo.NumberOfSeedCitizens; i++)
        {
            Vector2Int position = MapUtil.GetRandomMapPosition();

            while (worldMap.GetCell(position).Solid)
            {
                position = MapUtil.GetRandomMapPosition();
            }

            GenerateCitizen(position);
        }
    }


    private void GenerateCitizen(Vector2 position)
    {
        Vector2 worldPosition = MapUtil.IsoToWorld(position);

        GroupType groupType = Util.RandomEnumValue<GroupType>();

        string citizenName = nameGenerator.GetName(groupType);

        GameObject newCitizenObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        newCitizenObject.transform.parent = entitiesObject.transform;
        newCitizenObject.name = citizenName;
        newCitizenObject.layer = LayerMask.NameToLayer("Citizens");

        Citizen newCitizen = newCitizenObject.AddComponent<Citizen>();

        newCitizen.EntityData = new EntityData
        {
            Entity = newCitizenObject,
            Speed = 0f,
            Position = new Vector2(position.x, position.y),
            Direction = new Vector2(0, -1),
        };

        newCitizen.IdData = new IdData
        {
            FullName = citizenName,
            IdNumber = populationData.NextCitizenNumber++,
            PopulationType = PopulationType.Citizen,
            GroupType = groupType,
        };
    }
}
