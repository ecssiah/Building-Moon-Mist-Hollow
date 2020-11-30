using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingSystem : MonoBehaviour
{
    public string[] names;

    private int currentNameIndex;

    private string[] initialElements = { "Lri", "Ats", "Kar", "Dea", "Tel" };
    private string[] middleElements = { "kah", "tre", "ede", "she", "rin" };
    private string[] finalElements = { "cale", "si", "silt", "treh", "ehta" };


    void Start()
    {
        GenerateNames();
    }


    void Update()
    {
        
    }


    private void GenerateNames()
    {
        currentNameIndex = 0;
        names = new string[50];

        for (int i = 0; i < names.Length; i++)
        {
            string initial = initialElements[Random.Range(0, initialElements.Length)];
            string middle = middleElements[Random.Range(0, middleElements.Length)];
            string final = finalElements[Random.Range(0, finalElements.Length)];

            string newName = $"{initial}{middle}{final}";

            names[i] = newName;
        }
    }


    public string GetName()
    {
        if (currentNameIndex >= names.Length)
        {
            GenerateNames();
        }

        return names[currentNameIndex++];
    } 
}
