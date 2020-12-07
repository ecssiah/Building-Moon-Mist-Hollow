using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;

    private InfoPanelController infoPanelController;

    private const float LabelYOffset = 64;

    private GameObject[] labels;
    public GameObject entityLabelPrefab;

    private GameObject entitiesObject;
    private GameObject labelsObject;

    private GameObject[] entities;

    private int[] selectedEntities;

    private const int DefaultOrthographicSize = 4;


    void Awake()
    {
        entitiesObject = GameObject.Find("Entities");
        entitySystem = entitiesObject.GetComponent<EntitySystem>();


        labelsObject = GameObject.Find("Labels");

        infoPanelController = GameObject.Find("Info Panel").GetComponent<InfoPanelController>();
    }


    void Start()
    {
        entities = new GameObject[entitiesObject.transform.childCount];
        labels = new GameObject[entitiesObject.transform.childCount];

        int index = 0;
        foreach (Transform transform in entitiesObject.transform)
        {
            entities[index] = transform.gameObject;
            labels[index] = CreateLabel(transform.gameObject);

            index++;
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
}
