using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenComponent : MonoBehaviour
{
    private CitizenAnimator animator;
    private CitizenMovement movement;

    private CitizenData citizenData;
    public CitizenData CitizenData { get => citizenData; set => citizenData = value; }


    void Awake()
    {
        movement = gameObject.AddComponent<CitizenMovement>();
        animator = gameObject.AddComponent<CitizenAnimator>();
    }
}
