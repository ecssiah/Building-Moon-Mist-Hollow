using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingSystem : MonoBehaviour
{
    public string[] names;

    private int nextNameIndex;

    private string[] initialElements = { "La", "At", "Se", "Dea", "Tel", "Ce", "Re" };
    private string[] middleElements = { "ekah", "tere", "ede", "she", "rin", "e", "il", "" };
    private string[] finalElements = {  "cale", "si", "slt", "treh", "ehta", "ta", "lit", "ca" };
    


    void Start()
    {
        GenerateNames();
    }


    void Update()
    {
        
    }


    private void GenerateNames()
    {
        nextNameIndex = 0;
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
        if (nextNameIndex >= names.Length)
        {
            GenerateNames();
        }

        return names[nextNameIndex++];
    }
}
