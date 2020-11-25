using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float decisionTimer = 0;
    private float decisionDeadline = 4f;

    private float speed = 48f;

    private Vector2 direction = Vector2.zero;

    private Rigidbody2D rigidBody;
    private CharacterAnimator animator;


    void Start()
    {
        animator = GetComponent<CharacterAnimator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Tick();
        Move();
    }


    private void Tick()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDeadline)
        {
            decisionTimer = 0;

            direction = GetDirection();
            animator.SetAnimationDirection(direction);
        }
    }


    private void Move()
    {
        rigidBody.velocity = Time.deltaTime * speed * direction;
    }


    private Vector2 GetDirection()
    {
        System.Random random = new System.Random();

        Vector3 newDirection = new Vector3(
            random.Next(-1, 2), random.Next(-1, 2), 0
        );

        return ConversionUtilities.CartesianToIso(newDirection);
    }
}
