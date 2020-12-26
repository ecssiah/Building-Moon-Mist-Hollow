using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    public string GetName(GroupType groupType)
    {
        int initialIndex = 0;
        int middleIndex = 0;
        int finalIndex = 0;

        string initial = "[";
        string middle = "";
        string final = "]";

        switch (groupType)
        {
            case GroupType.Guy:
                initialIndex = Random.Range(0, NameInfo.GuyInitialElements.Length);
                middleIndex = Random.Range(0, NameInfo.GuyMiddleElements.Length);
                finalIndex = Random.Range(0, NameInfo.GuyFinalElements.Length);

                initial = NameInfo.GuyInitialElements[initialIndex];
                middle = NameInfo.GuyMiddleElements[middleIndex];
                final = NameInfo.GuyFinalElements[finalIndex];

                break;
            case GroupType.Kailt:
                initialIndex = Random.Range(0, NameInfo.KailtInitialElements.Length);
                middleIndex = Random.Range(0, NameInfo.KailtMiddleElements.Length);
                finalIndex = Random.Range(0, NameInfo.KailtFinalElements.Length);

                initial = NameInfo.KailtInitialElements[initialIndex];
                middle = NameInfo.KailtMiddleElements[middleIndex];
                final = NameInfo.KailtFinalElements[finalIndex];

                break;
            case GroupType.Taylor:
                initialIndex = Random.Range(0, NameInfo.TaylorInitialElements.Length);
                middleIndex = Random.Range(0, NameInfo.TaylorMiddleElements.Length);
                finalIndex = Random.Range(0, NameInfo.TaylorFinalElements.Length);

                initial = NameInfo.TaylorInitialElements[initialIndex];
                middle = NameInfo.TaylorMiddleElements[middleIndex];
                final = NameInfo.TaylorFinalElements[finalIndex];

                break;
        }

        return $"{initial}{middle}{final}";
    } 
}
