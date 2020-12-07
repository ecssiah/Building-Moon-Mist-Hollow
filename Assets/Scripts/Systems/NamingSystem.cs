using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingSystem : MonoBehaviour
{
    public string[] names;

    private int currentNameIndex;

    private string[] initialElements = { "La", "At", "Se", "Dea", "Tel", "Ce", "Re", "Til" };
    private string[] middleElements = { "ekah", "tere", "ede", "she", "rin", "e", "il", "ci", "y" };
    private string[] finalElements = { "cale", "si", "silit", "treh", "ehta", "ta", "lit", "ca" };


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
