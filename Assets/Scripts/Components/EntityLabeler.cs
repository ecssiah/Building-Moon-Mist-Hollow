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
        for (int i = 0; i < entities.Length; i++)
        {
            labels[i].transform.position = GetLabelPosition(entities[i].transform.position);
        }
    }


    private GameObject CreateLabel(GameObject entity)
    {
        GameObject newLabel = Instantiate(
            labelPrefab,
            GetLabelPosition(entity.transform.position),
            Quaternion.identity
        );

        TextMeshProUGUI textMesh = newLabel.GetComponent<TextMeshProUGUI>();
        textMesh.text = entity.name;
        textMesh.fontSize = 18;

        newLabel.transform.SetParent(labelsObject.transform, true);

        return newLabel;
    }


    private Vector3 GetLabelPosition(Vector3 entityPosition)
    {
        Vector3 screenPosition = MapUtil.WorldToScreen(entityPosition);
        screenPosition.y += UIInfo.LabelYOffset * UIUtil.CameraZoomRatio();

        return screenPosition;
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
            Destroy(child.gameObject);
        }
    }
}
