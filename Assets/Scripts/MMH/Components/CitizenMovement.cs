using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    private EntitySystem entitySystem;

    private Citizen citizen;

    private Rigidbody2D rigidBody2D;

    private PathData pathData;


    void Awake()
    {
        entitySystem = GameObject.Find("EntitySystem").GetComponent<EntitySystem>();

        citizen = GetComponent<Citizen>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        pathData = new PathData { Valid = false };
    }


    void Start()
    {
        Debug.Log(citizen.IdData.FullName);
        Debug.Log(citizen.EntityData.GridPosition);

        pathData = entitySystem.RequestPath(citizen.EntityData.GridPosition, new Vector2Int(4, 4));

        Debug.Log(pathData);
    }


    void Update()
    {
        Vector2 unscaledVelocity =
            citizen.EntityData.Speed *
            citizen.EntityData.Direction;

        rigidBody2D.velocity = Time.deltaTime * unscaledVelocity;
    }


    private Vector2 DecideVelocity()
    {
        return new Vector2(0, 0);
    }

}
