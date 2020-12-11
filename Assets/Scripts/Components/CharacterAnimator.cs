using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Sprite[] idleUpFrames;
    private Sprite[] idleDownFrames;
    private Sprite[] idleLeftFrames;
    private Sprite[] idleRightFrames;

    private Sprite[] walkUpFrames;
    private Sprite[] walkDownFrames;
    private Sprite[] walkLeftFrames;
    private Sprite[] walkRightFrames;

    private Sprite[] currentFrames;

    private SpriteRenderer spriteRenderer;

    private float timer = 0f;
    private int frameNumber = 0;
    private readonly float frameRate = 1 / 10f;

    private CharacterAnimationType currentAnimationType;

    private float decisionTimer = 0;
    private readonly float decisionDeadline = 4f;

    private float speed = 26f;

    private Vector2 direction = Vector2.zero;

    private Rigidbody2D rigidBody;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        idleUpFrames = Resources.LoadAll<Sprite>("Character/Idle/Up");
        idleDownFrames = Resources.LoadAll<Sprite>("Character/Idle/Down");
        idleLeftFrames = Resources.LoadAll<Sprite>("Character/Idle/Left");
        idleRightFrames = Resources.LoadAll<Sprite>("Character/Idle/Right");

        walkUpFrames = Resources.LoadAll<Sprite>("Character/Walk/Up");
        walkDownFrames = Resources.LoadAll<Sprite>("Character/Walk/Down");
        walkLeftFrames = Resources.LoadAll<Sprite>("Character/Walk/Left");
        walkRightFrames = Resources.LoadAll<Sprite>("Character/Walk/Right");

        PlayAnimation(CharacterAnimationType.WalkDown);
    }


    void Update()
    {
        UpdateFrame();

        Tick();
        Move();
    }


    private void UpdateFrame()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;

            frameNumber = (frameNumber + 1) % currentFrames.Length;

            spriteRenderer.sprite = currentFrames[frameNumber];
        }

    }


    private void PlayAnimation(CharacterAnimationType animationType)
    {
        timer = 0;
        frameNumber = 0;

        currentAnimationType = animationType;

        switch (animationType)
        {
            case CharacterAnimationType.IdleUp:
                currentFrames = idleUpFrames;
                break;
            case CharacterAnimationType.IdleDown:
                currentFrames = idleDownFrames;
                break;
            case CharacterAnimationType.IdleLeft:
                currentFrames = idleLeftFrames;
                break;
            case CharacterAnimationType.IdleRight:
                currentFrames = idleRightFrames;
                break;
            case CharacterAnimationType.WalkUp:
                currentFrames = walkUpFrames;
                break;
            case CharacterAnimationType.WalkDown:
                currentFrames = walkDownFrames;
                break;
            case CharacterAnimationType.WalkLeft:
                currentFrames = walkLeftFrames;
                break;
            case CharacterAnimationType.WalkRight:
                currentFrames = walkRightFrames;
                break;

        }
    }


    public void SetAnimationDirection(Vector3 direction)
    {
        if (direction.x == 0 && direction.y == 0)
        {
            if (currentAnimationType == CharacterAnimationType.WalkUp)
            {
                PlayAnimation(CharacterAnimationType.IdleUp);
            }
            else if (currentAnimationType == CharacterAnimationType.WalkDown)
            {
                PlayAnimation(CharacterAnimationType.IdleDown);
            }
            else if (currentAnimationType == CharacterAnimationType.WalkLeft)
            {
                PlayAnimation(CharacterAnimationType.IdleLeft);
            }
            else if (currentAnimationType == CharacterAnimationType.WalkRight)
            {
                PlayAnimation(CharacterAnimationType.IdleRight);
            }
        }
        else if (direction.x > 0)
        {
            PlayAnimation(CharacterAnimationType.WalkRight);
        }
        else if (direction.x < 0)
        {
            PlayAnimation(CharacterAnimationType.WalkLeft);
        }
        else if (direction.y > 0)
        {
            PlayAnimation(CharacterAnimationType.WalkUp);
        }
        else if (direction.y < 0)
        {
            PlayAnimation(CharacterAnimationType.WalkDown);
        }
    }


    private void Tick()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDeadline)
        {
            decisionTimer = 0;

            direction = GetDirection();
            SetAnimationDirection(direction);
        }
    }


    private void Move()
    {
        rigidBody.velocity = Time.deltaTime * speed * direction;
    }


    private Vector2 GetDirection()
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());

        Vector3 newDirection = new Vector3(
            random.Next(-1, 2), random.Next(-1, 2), 0
        );

        return Utilities.CartesianToIso(newDirection);
    }
}
