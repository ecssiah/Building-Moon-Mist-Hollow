using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityLabeler : MonoBehaviour
{
    private GameObject labelsObject;
    private GameObject labelPrefab;

    private GameObject[] labels;

    private GameObject[] entities;

    private const float LabelYOffset = 64;
    private const int DefaultOrthographicSize = 4;


    void Awake()
    {
        labelsObject = GameObject.Find("Labels");

        labelPrefab = Resources.Load<GameObject>("Prefabs/Label");

        labels = new GameObject[0];
        entities = new GameObject[0];
    }


    void Start()
    {
        
    }


    void Update()
    {
        float cameraSizeRatio = DefaultOrthographicSize / Camera.main.orthographicSize;

        for (int i = 0; i < entities.Length; i++)
        {
            Vector3 labelPosition = Utilities.WorldToScreen(entities[i].transform.position);
            labelPosition.y += LabelYOffset * cameraSizeRatio;

            labels[i].transform.position = labelPosition;
        }
    }


    private GameObject CreateLabel(GameObject entity)
    {
        Vector3 screenPosition = Utilities.WorldToScreen(entity.transform.position);

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
