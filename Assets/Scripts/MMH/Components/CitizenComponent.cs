using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenComponent : MonoBehaviour
{
    public EntityData EntityData;
    public CitizenData CitizenData;

    private CitizenAnimator citizenAnimator;
    private CitizenMovement citizenMovement;

    void Awake()
    {
        citizenAnimator = gameObject.AddComponent<CitizenAnimator>();
        citizenMovement = gameObject.AddComponent<CitizenMovement>();

        citizenMovement.OnDirectionChange = citizenAnimator.OnDirectionChange;
    }
}
