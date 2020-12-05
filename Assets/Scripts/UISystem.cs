﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameObject entityLabelPrefab;

    private GameObject labelsObject;
    private GameObject entitiesObject;

    private GameObject[] entities;
    private GameObject[] labels;


    void Awake()
    {
        labelsObject = GameObject.Find("Labels");
    }


    void Start()
    {
        entitiesObject = GameObject.Find("Entities");

        entities = new GameObject[entitiesObject.transform.childCount];
        labels = new GameObject[entitiesObject.transform.childCount];

        int i = 0;
        foreach (Transform t in entitiesObject.transform)
        {
            entities[i] = t.transform.gameObject;
            labels[i] = CreateLabel(t.transform.gameObject);

            i++;
        }
    }


    private GameObject CreateLabel(GameObject entity)
    {
        Debug.Log(entity.transform.position);

        Vector3 screenPosition = Utilities.WorldToScreen(entity.transform.position);

        GameObject newLabel = Instantiate(
            entityLabelPrefab, screenPosition, Quaternion.identity
        );

        TextMeshProUGUI text = newLabel.GetComponent<TextMeshProUGUI>();
        text.text = entity.name;

        newLabel.transform.SetParent(labelsObject.transform, true);

        return newLabel;
    }


    void Update()
    {
        for (int i = 0; i < entities.Length; i++)
        {
            labels[i].transform.position = Utilities.WorldToScreen(
                entities[i].transform.position
            );
        }
    }
}
