using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingSystem : MonoBehaviour
{
    private string[] initialElements = { "Lri", "Ats", "Kar", "Dea" };
    private string[] middleElements = { "kah", "tre", "ede", "she" };
    private string[] finalElements = { "cale", "si", "silt", "treh" };

    public string[] names;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
