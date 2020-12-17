using TMPro;
using UnityEngine;

public class EntityLabeler : MonoBehaviour
{
    private GameObject labelsObject;
    private GameObject labelPrefab;

    private GameObject[] labels;

    private GameObject[] entities;


    void Awake()
    {
        labels = new GameObject[0];
        labelsObject = GameObject.Find("Labels");
        labelPrefab = Resources.Load<GameObject>("Prefabs/Label");

        entities = new GameObject[0];
    }


    void Update()
    {
        float cameraSizeRatio = ViewInfo.DefaultOrthographicSize / Camera.main.orthographicSize;

        for (int index = 0; index < entities.Length; index++)
        {
            Vector3 labelPosition = MapUtil.WorldToScreen(entities[index].transform.position);
            labelPosition.y += UIInfo.LabelYOffset * cameraSizeRatio;

            labels[index].transform.position = labelPosition;
        }
    }


    private GameObject CreateLabel(GameObject entity)
    {
        Vector3 screenPosition = MapUtil.WorldToScreen(entity.transform.position);

        GameObject newLabel = Instantiate(
            labelPrefab, screenPosition, Quaternion.identity
        );

        TextMeshProUGUI textMesh = newLabel.GetComponent<TextMeshProUGUI>();
        textMesh.text = entity.name;
        textMesh.fontSize = 14;

        newLabel.transform.SetParent(labelsObject.transform, true);

        return newLabel;
    }


    public void SelectEntity(GameObject entity)
    {
        Clear();

        entities = new GameObject[] { entity };
        labels = new GameObject[] { CreateLabel(entity) };
    }


    public void Clear()
    {
        entities = new GameObject[0];
        labels = new GameObject[0];

        foreach (Transform child in labelsObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
