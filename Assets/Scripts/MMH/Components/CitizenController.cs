using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenController : MonoBehaviour
{
    void Awake()
    {
        gameObject.AddComponent<CitizenAnimator>();
        gameObject.AddComponent<RandomPathMovement>();
    }
}
