using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenController : MonoBehaviour
{
    private CitizenAnimator citizenAnimator;
    private CitizenMovement citizenMovement;

    void Awake()
    {
        citizenAnimator = gameObject.AddComponent<CitizenAnimator>();
        citizenMovement = gameObject.AddComponent<CitizenMovement>();

        citizenMovement.OnDirectionChange = citizenAnimator.OnDirectionChange;
    }
}
