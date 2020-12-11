using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityLabeler : MonoBehaviour
{
    private GameObject entitiesObject;

    private const float LabelYOffset = 64;

    private GameObject labelsObject;
    private GameObject entityLabelPrefab;

    private GameObject[] labels;
    private GameObject[] entities;

    private const int DefaultOrthographicSize = 4;


    void Awake()
    {
        entityLabelPrefab = Resources.Load<GameObject>("Prefabs/Label");

        entities = new GameObject[0];
        entitiesObject = GameObject.Find("Entities");

        labels = new GameObject[0];
        labelsObject = GameObject.Find("Labels");
    }


    void Start()
    {
    }


    void Update()
    {
        float cameraSizeRatio = DefaultOrthographicSize / Camera.main.orthographicSize;

        for (int index = 0; index < entities.Length; index++)
        {
            Vector3 labelPosition = Utilities.WorldToScreen(entities[index].transform.position);
            labelPosition.y += LabelYOffset * cameraSizeRatio;

            labels[index].transform.position = labelPosition;
        }
    }


    private GameObject CreateLabel(GameObject entity)
    {
        Vector3 screenPosition = Utilities.WorldToScreen(entity.transform.position);

        GameObject newLabel = Instantiate(
            entityLabelPrefab, screenPosition, Quaternion.identity
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
