using System;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAnimator : MonoBehaviour
{
    private CitizenComponent citizenComponent;
    private CitizenMovement citizenMovement;

    private AnimationType animationType;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private Sprite[] currentFrames;

    private Dictionary<AnimationType, Sprite[]> frames;

    private float speed;
    private Vector2 direction;

    private float timer;
    private int frameNumber;
    private float frameRate;


    void Awake()
    {
        citizenComponent = GetComponent<CitizenComponent>();

        citizenMovement = GetComponent<CitizenMovement>();
        citizenMovement.OnDirectionChange = OnDirectionChange;

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = 82f;
        direction = new Vector2();

        timer = 0f;
        frameNumber = 0;
        frameRate = 1 / 10f;
    }


    void Start()
    {
        string groupType = Enum.GetName(
            typeof(GroupType),
            citizenComponent.CitizenData.GroupData.GroupType
        );

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

        PlayAnimation(AnimationType.WalkDown);
    }


    void Update()
    {
        citizenMovement.Think();

        Move();

        UpdateAnimation();
    }


    private void Move()
    {
        rigidBody.velocity = Time.deltaTime * speed * direction;
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


    public void OnDirectionChange(Vector2 direction)
    {
        this.direction = direction;

        if (direction.x == 0 && direction.y == 0)
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
