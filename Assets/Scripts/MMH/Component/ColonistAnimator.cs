using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class ColonistAnimator : MonoBehaviour
    {
        private Colonist colonist;

        private SpriteRenderer spriteRenderer;

        private Dictionary<Type.Animation, Sprite[]> frames;

        private float timer;
        private int frameNumber;
        private float frameRate;


        void Awake()
        {
            colonist = GetComponent<Colonist>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            timer = 0f;
        }


        void Start()
        {
            string groupType = Enum.GetName(typeof(Type.Group), colonist.Id.GroupType);

            frameNumber = 0;
            frameRate = 1 / 10f;

            frames = new Dictionary<Type.Animation, Sprite[]>
            {
                [Type.Animation.IdleUp] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Back/Idle"),
                [Type.Animation.IdleDown] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Front/Idle"),
                [Type.Animation.IdleLeft] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Left/Idle"),
                [Type.Animation.IdleRight] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Right/Idle"),

                [Type.Animation.WalkUp] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Back/Walk"),
                [Type.Animation.WalkDown] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Front/Walk"),
                [Type.Animation.WalkLeft] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Left/Walk"),
                [Type.Animation.WalkRight] = Resources.LoadAll<Sprite>($"Colonists/{groupType}/Right/Walk")
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
                bool moving = colonist.Entity.Speed > 0;
                Type.Direction direction = colonist.Entity.Direction;

                switch (direction)
                {
                    case Type.Direction.N:
                    case Type.Direction.W:
                    case Type.Direction.NW:
                        return frames[moving ? Type.Animation.WalkLeft : Type.Animation.IdleLeft];
                    case Type.Direction.S:
                    case Type.Direction.E:
                    case Type.Direction.SE:
                        return frames[moving ? Type.Animation.WalkRight : Type.Animation.IdleRight];
                    case Type.Direction.NE:
                        return frames[moving ? Type.Animation.WalkUp : Type.Animation.IdleUp];
                    case Type.Direction.SW:
                    default:
                        return frames[moving ? Type.Animation.WalkDown : Type.Animation.IdleDown];
                }
            }
        }
    }
}