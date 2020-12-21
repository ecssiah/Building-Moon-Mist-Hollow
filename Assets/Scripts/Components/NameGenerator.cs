using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    public string[] names;

    private int currentNameIndex;

    private readonly string[] initialElements = {
        "La", "At", "Se", "Il", "Elq", "Ce", "Re", "Til"
    };
    private readonly string[] middleElements = {
        "ekah", "tere", "ede", "she", "rin", "e", "il", "ci"
    };
    private readonly string[] finalElements = {
        "cale", "si", "slt", "treh", "ehta", "ta", "lit", "ca"
    };


    void Awake()
    {
        GenerateNames();
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
