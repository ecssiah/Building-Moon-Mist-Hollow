﻿using System.Collections;
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
    private float frameRate = 1 / 10f;

    private AnimationType currentAnimationType;


    void Awake()
    {
        idleUpFrames = Resources.LoadAll<Sprite>("Citizen/Idle/Up");
        idleDownFrames = Resources.LoadAll<Sprite>("Citizen/Idle/Down");
        idleLeftFrames = Resources.LoadAll<Sprite>("Citizen/Idle/Left");
        idleRightFrames = Resources.LoadAll<Sprite>("Citizen/Idle/Right");

        walkUpFrames = Resources.LoadAll<Sprite>("Citizen/Walk/Up");
        walkDownFrames = Resources.LoadAll<Sprite>("Citizen/Walk/Down");
        walkLeftFrames = Resources.LoadAll<Sprite>("Citizen/Walk/Left");
        walkRightFrames = Resources.LoadAll<Sprite>("Citizen/Walk/Right");
    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayAnimation(AnimationType.WalkDown);
    }


    void Update()
    {
        UpdateFrame();
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


    private void PlayAnimation(AnimationType animationType)
    {
        timer = 0;
        frameNumber = 0;

        currentAnimationType = animationType;

        switch (animationType)
        {
            case AnimationType.IdleUp:
                currentFrames = idleUpFrames;
                break;
            case AnimationType.IdleDown:
                currentFrames = idleDownFrames;
                break;
            case AnimationType.IdleLeft:
                currentFrames = idleLeftFrames;
                break;
            case AnimationType.IdleRight:
                currentFrames = idleRightFrames;
                break;
            case AnimationType.WalkUp:
                currentFrames = walkUpFrames;
                break;
            case AnimationType.WalkDown:
                currentFrames = walkDownFrames;
                break;
            case AnimationType.WalkLeft:
                currentFrames = walkLeftFrames;
                break;
            case AnimationType.WalkRight:
                currentFrames = walkRightFrames;
                break;

        }
    }


    public void SetAnimationDirection(Vector3 direction)
    {
        if (direction.x == 0 && direction.y == 0)
        {
            if (currentAnimationType == AnimationType.WalkUp)
            {
                PlayAnimation(AnimationType.IdleUp);
            }
            else if (currentAnimationType == AnimationType.WalkDown)
            {
                PlayAnimation(AnimationType.IdleDown);
            }
            else if (currentAnimationType == AnimationType.WalkLeft)
            {
                PlayAnimation(AnimationType.IdleLeft);
            }
            else if (currentAnimationType == AnimationType.WalkRight)
            {
                PlayAnimation(AnimationType.IdleRight);
            }
        }
        else if (direction.x > 0)
        {
            PlayAnimation(AnimationType.WalkRight);
        }
        else if (direction.x < 0)
        {
            PlayAnimation(AnimationType.WalkLeft);
        }
        else if (direction.y > 0)
        {
            PlayAnimation(AnimationType.WalkUp);
        }
        else if (direction.y < 0)
        {
            PlayAnimation(AnimationType.WalkDown);
        }
    }
}
