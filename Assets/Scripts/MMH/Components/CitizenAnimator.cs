using System;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAnimator : MonoBehaviour
{
    private Citizen citizen;

    private AnimationType animationType;

    private SpriteRenderer spriteRenderer;

    private Dictionary<AnimationType, Sprite[]> frames;

    private float timer;
    private int frameNumber;
    private float frameRate;


    void Awake()
    {
        citizen = GetComponent<Citizen>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        timer = 0f;
    }


    void Start()
    {
        string groupType = Enum.GetName(typeof(GroupType), citizen.IdData.GroupType);

        frameNumber = 0;
        frameRate = 1 / 10f;

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
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;

            Sprite[] frames = GetCurrentFrames();

            frameNumber = (frameNumber + 1) % frames.Length;

            spriteRenderer.sprite = frames[frameNumber];
        }
    }


    private Sprite[] GetCurrentFrames()
    {
        if (citizen.EntityData.Direction.x > 0)
        {
            if (citizen.EntityData.Speed > 0)
            {
                return frames[AnimationType.WalkRight];
            }
            else
            {
                return frames[AnimationType.IdleRight];
            }
        }
        else if (citizen.EntityData.Direction.x < 0)
        {
            if (citizen.EntityData.Speed > 0)
            {
                return frames[AnimationType.WalkLeft];
            }
            else
            {
                return frames[AnimationType.IdleLeft];
            }
        }
        else
        {
            if (citizen.EntityData.Speed > 0)
            {
                if (citizen.EntityData.Direction.y > 0)
                {
                    return frames[AnimationType.WalkUp];
                }
                else if (citizen.EntityData.Direction.y < 0)
                {
                    return frames[AnimationType.WalkDown];
                }
            }
            else
            {
                if (citizen.EntityData.Direction.y > 0)
                {
                    return frames[AnimationType.IdleUp];
                }
                else if (citizen.EntityData.Direction.y < 0)
                {
                    return frames[AnimationType.IdleDown];
                }
            }
        }

        return frames[AnimationType.IdleDown];
    }
}
