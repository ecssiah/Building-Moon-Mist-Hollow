using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private PopulationData populationData;

    private MapSystem mapSystem;

    private NameGenerator nameGenerator;

    private GameObject entitiesObject;
    private GameObject citizenPrefab;

    void Awake()
    {
        mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

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
        for (int i = 0; i < EntityInfo.NumberOfSeedCitizens; i++)
        {
            Vector2Int position = MapUtil.GetRandomMapPosition();

            while (mapSystem.GetMapData().GetCell(position).Solid)
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

        GameObject newCitizenObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        newCitizenObject.transform.parent = entitiesObject.transform;
        newCitizenObject.name = nameGenerator.GetName(groupType);
        newCitizenObject.layer = LayerMask.NameToLayer("Citizens");

        CitizenComponent newCitizen = newCitizenObject.AddComponent<CitizenComponent>();

        newCitizen.EntityData = new EntityData
        {
            Entity = newCitizenObject,
            Speed = 0f,
            Position = new Vector2(position.x, position.y),
            Direction = new Vector2(0, -1),
        };

        newCitizen.CitizenData = new CitizenData
        {
            IdData = new IdData
            {
                FullName = newCitizenObject.name,
                IdNumber = populationData.NextCitizenNumber++,
                PopulationType = PopulationType.Citizen,
            },
            GroupData = new GroupData
            {
                GroupType = groupType
            },
        };
    }


    public CitizenData GetCitizenData(GameObject citizenObject)
    {
        CitizenComponent citizenComponent = citizenObject.GetComponent<CitizenComponent>();

        return citizenComponent.CitizenData;
    }
}
