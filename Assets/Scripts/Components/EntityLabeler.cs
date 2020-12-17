using TMPro;
using UnityEngine;

public class EntityLabeler : MonoBehaviour
{
    private GameObject[] labels;
    private GameObject labelsObject;
    private GameObject labelPrefab;

    private GameObject[] entities;


    void Awake()
    {
        labelsObject = GameObject.Find("Labels");

        labelPrefab = Resources.Load<GameObject>("Prefabs/Label");

        labels = new GameObject[0];
        entities = new GameObject[0];
    }


    void Update()
    {
        float cameraSizeRatio =
            ViewInfo.DefaultOrthographicSize / Camera.main.orthographicSize;

        for (int i = 0; i < entities.Length; i++)
        {
            Vector2 labelPosition = MapUtil.WorldToScreen(
                entities[i].transform.position
            );
            labelPosition.y += cameraSizeRatio * UIInfo.LabelOffset;

            labels[i].transform.position = labelPosition;
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
        ClearSelection();

        entities = new GameObject[] { entity };
        labels = new GameObject[] { CreateLabel(entity) };
    }


    public void ClearSelection()
    {
        entities = new GameObject[0];
        labels = new GameObject[0];

        foreach (Transform childTransform in labelsObject.transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
}
