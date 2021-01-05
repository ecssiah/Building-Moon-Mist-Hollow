using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    private PathData pathData;

    private Citizen citizenComponent;

    private Rigidbody2D rigidBody2D;

    public Action OnDirectionChange;

    public delegate PathData RequestPathDelegate(Vector2Int start, Vector2Int end);

    public RequestPathDelegate RequestPath;


    void Awake()
    {
        pathData = new PathData { Valid = false };

        citizenComponent = GetComponent<Citizen>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        PathData pathData = RequestPath(new Vector2Int(0, 0), new Vector2Int(3, 3));

        Debug.Log(pathData);
    }


    void Update()
    {
        rigidBody2D.velocity = Time.deltaTime * citizenComponent.EntityData.Speed * citizenComponent.EntityData.Direction;
    }

}
