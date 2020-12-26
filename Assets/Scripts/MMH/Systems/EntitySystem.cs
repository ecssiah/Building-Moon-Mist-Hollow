using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private MapSystem mapSystem;

    private NameGenerator nameGenerator;

    private EntityData entityData;

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

            while (mapSystem.GetCellData(position).solid)
            {
                position = MapUtil.GetRandomMapPosition();
            }

            GenerateCitizen(position);
        }
    }


    private void GenerateCitizen(Vector2 position)
    {
        Vector2 worldPosition = MapUtil.IsoToWorld(position);

        GameObject newCitizenObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        GroupType groupType = Util.RandomEnumValue<GroupType>();

        newCitizenObject.transform.parent = entitiesObject.transform;
        newCitizenObject.name = nameGenerator.GetName(groupType);
        newCitizenObject.layer = LayerMask.NameToLayer("Citizens");

        CitizenComponent newCitizen = newCitizenObject.AddComponent<CitizenComponent>();
        newCitizen.CitizenData = new CitizenData()
        {
            name = newCitizenObject.name,
            citizenNumber = entityData.nextCitizenNumber++,
            groupData = new GroupData { groupType = groupType },
        };
    }


    public CitizenData GetCitizenData(GameObject citizenObject)
    {
        CitizenComponent citizenComponent = citizenObject.GetComponent<CitizenComponent>();

        return citizenComponent.CitizenData;
    }
}
