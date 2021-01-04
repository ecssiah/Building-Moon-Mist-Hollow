using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenComponent : MonoBehaviour
{
    public EntityData EntityData;
    public CitizenData CitizenData;


    void Awake()
    {
        gameObject.AddComponent<CitizenController>();
    }
}
