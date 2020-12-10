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

    public GameObject[] Entities { get => entities; set => entities = value; }

    void Awake()
    {
        Entities = new GameObject[0];
        entitiesObject = GameObject.Find("Entities");

        entityLabelPrefab = Resources.Load<GameObject>("Prefabs/Entity Label");

        labelsObject = GameObject.Find("Labels");
    }


    void Start()
    {
        labels = new GameObject[entitiesObject.transform.childCount];

        int index = 0;
        foreach (Transform transform in entitiesObject.transform)
        {
            labels[index++] = CreateLabel(transform.gameObject);
        }
    }


    void Update()
    {
        float cameraSizeRatio = DefaultOrthographicSize / Camera.main.orthographicSize;

        for (int index = 0; index < Entities.Length; index++)
        {
            Vector3 labelPosition = Utilities.WorldToScreen(Entities[index].transform.position);
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
}
