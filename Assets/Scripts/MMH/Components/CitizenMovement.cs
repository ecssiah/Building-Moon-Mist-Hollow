using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    private PathData pathData;

    private CitizenComponent citizenComponent;

    private Rigidbody2D rigidBody2D;

    public Action OnDirectionChange;


    void Awake()
    {
        pathData = new PathData { Valid = false };

        citizenComponent = GetComponent<CitizenComponent>();
        rigidBody2D = GetComponent<Rigidbody2D>();


    }


    void Update()
    {
        rigidBody2D.velocity = Time.deltaTime * citizenComponent.EntityData.Speed * citizenComponent.EntityData.Direction;
    }

}
