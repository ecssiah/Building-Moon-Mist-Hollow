using System;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAnimator : MonoBehaviour
{
    private AnimationType animationType;

    private CitizenComponent citizenComponent;

    private SpriteRenderer spriteRenderer;

    private Sprite[] currentFrames;

    private Dictionary<AnimationType, Sprite[]> frames;

    private float timer;
    private int frameNumber;
    private float frameRate;


    void Awake()
    {
        citizenComponent = GetComponent<CitizenComponent>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        timer = 0f;
        frameNumber = 0;
        frameRate = 1 / 10f;
    }


    void Start()
    {
        string groupType = Enum.GetName(typeof(GroupType), citizenComponent.CitizenData.GroupData.GroupType);

        frames = new Dictionary<AnimationType, Sprite[]>
        {
            [AnimationType.IdleUp] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Back/Idle"),
            [AnimationType.IdleDown] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Front/Idle"),
            [AnimationType.IdleLeft] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Left/Idle"),
            [AnimationType.IdleRight] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Right/Idle"),

            [AnimationType.WalkUp] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Back/Walk"),
            [AnimationType.WalkDown] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Front/Walk"),
            [AnimationType.WalkLeft] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Left/Walk"),
            [AnimationType.WalkRight] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Right/Walk")
        };

        PlayAnimation(AnimationType.WalkUp);
    }


    void Update()
    {
        UpdateAnimation();
    }


    private void UpdateAnimation()
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

        this.animationType = animationType;

        switch (animationType)
        {
            case AnimationType.IdleUp:
                currentFrames = frames[AnimationType.IdleUp];
                break;
            case AnimationType.IdleDown:
                currentFrames = frames[AnimationType.IdleDown];
                break;
            case AnimationType.IdleLeft:
                currentFrames = frames[AnimationType.IdleLeft];
                break;
            case AnimationType.IdleRight:
                currentFrames = frames[AnimationType.IdleRight];
                break;
            case AnimationType.WalkUp:
                currentFrames = frames[AnimationType.WalkUp];
                break;
            case AnimationType.WalkDown:
                currentFrames = frames[AnimationType.WalkDown];
                break;
            case AnimationType.WalkLeft:
                currentFrames = frames[AnimationType.WalkLeft];
                break;
            case AnimationType.WalkRight:
                currentFrames = frames[AnimationType.WalkRight];
                break;
            default:
                currentFrames = frames[AnimationType.IdleDown];
                break;
        }
    }


    public void OnDirectionChange()
    {
        Vector2 newDirection = citizenComponent.EntityData.Direction;

        if (newDirection.x == 0 && newDirection.y == 0)
        {
            if (animationType == AnimationType.WalkUp)
            {
                PlayAnimation(AnimationType.IdleUp);
            }
            else if (animationType == AnimationType.WalkDown)
            {
                PlayAnimation(AnimationType.IdleDown);
            }
            else if (animationType == AnimationType.WalkLeft)
            {
                PlayAnimation(AnimationType.IdleLeft);
            }
            else if (animationType == AnimationType.WalkRight)
            {
                PlayAnimation(AnimationType.IdleRight);
            }
        }
        else if (newDirection.x > 0)
        {
            PlayAnimation(AnimationType.WalkRight);
        }
        else if (newDirection.x < 0)
        {
            PlayAnimation(AnimationType.WalkLeft);
        }
        else if (newDirection.y > 0)
        {
            PlayAnimation(AnimationType.WalkUp);
        }
        else if (newDirection.y < 0)
        {
            PlayAnimation(AnimationType.WalkDown);
        }
    }
}
