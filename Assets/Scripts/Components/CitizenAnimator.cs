using UnityEngine;

public class CitizenAnimator : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private Sprite[] currentFrames;

    private Sprite[] idleUpFrames;
    private Sprite[] idleDownFrames;
    private Sprite[] idleLeftFrames;
    private Sprite[] idleRightFrames;

    private Sprite[] walkUpFrames;
    private Sprite[] walkDownFrames;
    private Sprite[] walkLeftFrames;
    private Sprite[] walkRightFrames;

    private AnimationType animationType;

    private CitizenMovement citizenMovement;

    private float speed;
    private Vector2 direction;

    private float timer;
    private int frameNumber;
    private float frameRate;


    void Awake()
    {
        citizenMovement = GetComponent<CitizenMovement>();
        citizenMovement.OnDecision = SetAnimationDirection;

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = 82f;

        timer = 0f;
        frameNumber = 0;
        frameRate = 1 / 10f;

        idleUpFrames = Resources.LoadAll<Sprite>("Character/Idle/Up");
        idleDownFrames = Resources.LoadAll<Sprite>("Character/Idle/Down");
        idleLeftFrames = Resources.LoadAll<Sprite>("Character/Idle/Left");
        idleRightFrames = Resources.LoadAll<Sprite>("Character/Idle/Right");

        walkUpFrames = Resources.LoadAll<Sprite>("Character/Walk/Up");
        walkDownFrames = Resources.LoadAll<Sprite>("Character/Walk/Down");
        walkLeftFrames = Resources.LoadAll<Sprite>("Character/Walk/Left");
        walkRightFrames = Resources.LoadAll<Sprite>("Character/Walk/Right");

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
            default:
                currentFrames = idleDownFrames;
                break;
        }
    }


    public void SetAnimationDirection(Vector2 direction)
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
