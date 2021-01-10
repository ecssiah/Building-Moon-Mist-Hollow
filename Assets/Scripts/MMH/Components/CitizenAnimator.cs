using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class CitizenAnimator : MonoBehaviour
    {
        private Citizen citizen;

        private SpriteRenderer spriteRenderer;

        private Dictionary<Type.Animation, Sprite[]> frames;

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
            string groupType = Enum.GetName(typeof(Type.Group), citizen.Id.GroupType);

            frameNumber = 0;
            frameRate = 1 / 6f;

            frames = new Dictionary<Type.Animation, Sprite[]>
            {
                [Type.Animation.IdleUp] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Back/Idle"),
                [Type.Animation.IdleDown] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Front/Idle"),
                [Type.Animation.IdleLeft] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Left/Idle"),
                [Type.Animation.IdleRight] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Right/Idle"),

                [Type.Animation.WalkUp] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Back/Walk"),
                [Type.Animation.WalkDown] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Front/Walk"),
                [Type.Animation.WalkLeft] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Left/Walk"),
                [Type.Animation.WalkRight] = Resources.LoadAll<Sprite>($"Citizens/{groupType}/Right/Walk")
            };
        }


        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= frameRate)
            {
                timer -= frameRate;

                Sprite[] frames = CurrentFrames;

                frameNumber = (frameNumber + 1) % frames.Length;

                spriteRenderer.sprite = frames[frameNumber];
            }
        }


        private Sprite[] CurrentFrames
        {
            get
            {
                if (citizen.Entity.Velocity.x > 0 && citizen.Entity.Velocity.y > 0)
                {
                    return frames[Type.Animation.WalkDown];
                }
                else if (citizen.Entity.Velocity.x < 0 && citizen.Entity.Velocity.y < 0)
                {
                    return frames[Type.Animation.WalkUp];
                }
                else if (citizen.Entity.Velocity.y > 0)
                {
                    return frames[Type.Animation.WalkRight];
                }
                else if (citizen.Entity.Velocity.x > 0)
                {
                    return frames[Type.Animation.WalkLeft];
                }
                else if (citizen.Entity.Velocity.y < 0)
                {
                    return frames[Type.Animation.WalkLeft];
                }
                else if (citizen.Entity.Velocity.x < 0)
                {
                    return frames[Type.Animation.WalkRight];
                }
                else
                {
                    if (citizen.Entity.Direction.y > 0)
                    {
                        return frames[Type.Animation.WalkUp];
                    }
                    else if (citizen.Entity.Direction.y < 0)
                    {
                        return frames[Type.Animation.WalkUp];
                    }
                    else if (citizen.Entity.Direction.x > 0)
                    {
                        return frames[Type.Animation.WalkUp];
                    }
                    else if (citizen.Entity.Direction.x < 0)
                    {
                        return frames[Type.Animation.WalkUp];
                    }
                    else
                    {
                        return frames[Type.Animation.WalkUp];
                    }
                }
            }
        }
    }
}