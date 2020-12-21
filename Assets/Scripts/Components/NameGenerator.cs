using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    private string[] names;

    private int currentNameIndex;


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
            int initialIndex = Random.Range(0, NameInfo.InitialElements.Length);
            int middleIndex = Random.Range(0, NameInfo.MiddleElements.Length);
            int finalIndex = Random.Range(0, NameInfo.FinalElements.Length);

            string initial = NameInfo.InitialElements[initialIndex];
            string middle = NameInfo.MiddleElements[middleIndex];
            string final = NameInfo.FinalElements[finalIndex];

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
