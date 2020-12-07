using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameObject EntityLabelPrefab;

    private GameObject LabelsObject;
    private GameObject EntitiesObject;

    private int[] SelectedEntities;

    private GameObject[] Entities;
    private GameObject[] Labels;

    private const int DEFAULT_ORTHOGRAPHIC_SIZE = 4;


    void Awake()
    {
        LabelsObject = GameObject.Find("Labels");
    }


    void Start()
    {
        EntitiesObject = GameObject.Find("Entities");

        Entities = new GameObject[EntitiesObject.transform.childCount];
        Labels = new GameObject[EntitiesObject.transform.childCount];

        int i = 0;
        foreach (Transform t in EntitiesObject.transform)
        {
            Entities[i] = t.transform.gameObject;
            Labels[i] = CreateLabel(t.transform.gameObject);

            i++;
        }
    }


    private GameObject CreateLabel(GameObject entity)
    {
        Vector3 ScreenPosition = Utilities.WorldToScreen(entity.transform.position);

        GameObject NewLabel = Instantiate(
            EntityLabelPrefab, ScreenPosition, Quaternion.identity
        );

        TextMeshProUGUI Text = NewLabel.GetComponent<TextMeshProUGUI>();
        Text.text = entity.name;
        Text.fontSize = 14;

        NewLabel.transform.SetParent(LabelsObject.transform, true);

        return NewLabel;

    }


    void Update()
    {
        float CameraSizeRatio = DEFAULT_ORTHOGRAPHIC_SIZE / Camera.main.orthographicSize;

        for (int i = 0; i < Entities.Length; i++)
        {
            Vector3 LabelPosition = Utilities.WorldToScreen(Entities[i].transform.position);
            LabelPosition.y += 64 * CameraSizeRatio;

            Labels[i].transform.position = LabelPosition;
        }
    }
}
