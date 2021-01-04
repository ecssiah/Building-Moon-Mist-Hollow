using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenMovement : MonoBehaviour
{
    private PathData pathData;

    private CitizenComponent citizenComponent;

    private Rigidbody2D rigidBody2D;

    private float decisionTimer;
    private float decisionDeadline;


    public Action OnDirectionChange;


    void Awake()
    {
        citizenComponent = GetComponent<CitizenComponent>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        decisionTimer = 0;
        decisionDeadline = UnityEngine.Random.Range(2f, 6f);
    }


    void Update()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDeadline)
        {
            decisionTimer -= decisionDeadline;
            decisionDeadline = UnityEngine.Random.Range(2f, 6f);

            citizenComponent.EntityData.Direction = RandomIsoDirection();

            OnDirectionChange();
        }

        rigidBody2D.velocity = Time.deltaTime * citizenComponent.EntityData.Speed * citizenComponent.EntityData.Direction;
    }


    private Vector2 RandomIsoDirection()
    {
        Vector2 newIsoDirection = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));

        return MapUtil.IsoToWorld(newIsoDirection);
    }

}
