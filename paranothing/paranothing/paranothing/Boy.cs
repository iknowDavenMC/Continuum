using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace paranothing
{
    class Boy : Drawable, Updatable, Collideable, Audible
    {
        private GameController control = GameController.getInstance();
        private SpriteSheet sheet;
        private int frame;
        private int frameLength;
        private int frameTime;
        private string animName;
        private List<int> animFrames;
        public string Animation
        {
            get { return animName; }
            set
            {
               
                if (sheet.hasAnimation(value) && animName != value)
                {
                    animName = value;
                    animFrames = sheet.getAnimation(animName);
                    frame = 0;
                    frameTime = 0;
                }
            }
        }
        private float moveSpeedX, moveSpeedY; // Pixels per animation frame
        private Vector2 position;
        public int Width, Height;

        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }

        public enum BoyState { Idle, Walk, StairsLeft, StairsRight, PushWalk, PushingStill, Teleport, TimeTravel }
        public BoyState state;
        public Direction direction;
        public ActionBubble actionBubble;
        private Vector2 teleportTo;
        private Cue soundCue;

        public Interactive interactor;

        public Boy(float X, float Y, ActionBubble actionBubble, SpriteSheet sheet)
        {
            this.sheet = sheet;
            frame = 0;
            frameTime = 0;
            frameLength = 70;
            position = new Vector2(X, Y);
            Width = 38;
            Height = 58;
            state = BoyState.Idle;
            Animation = "stand";
            direction = Direction.Right;
            this.actionBubble = actionBubble;
            actionBubble.Player = this;
            actionBubble.show();
            teleportTo = new Vector2();
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (direction == Direction.Left)
                flip = SpriteEffects.FlipHorizontally;
            Rectangle sprite = sheet.getSprite(animFrames.ElementAt(frame));
            renderer.Draw(sheet.image, position, sprite, tint, 0f, new Vector2(), 1f, flip, 0.25f);
        }

        private void checkInput(GameController control)
        {

            if (control.keyState.IsKeyDown(Keys.Space))
            {
                if ((state == BoyState.Walk || state == BoyState.Idle) && null != interactor)
                {
                    interactor.Interact(this);
                }
            }
            else if (control.keyState.IsKeyUp(Keys.Left) && control.keyState.IsKeyUp(Keys.Right) && state != BoyState.Teleport && state != BoyState.TimeTravel)
            {
                if (direction == Direction.Right)
                {
                    if (state != BoyState.StairsRight && state != BoyState.StairsLeft)
                        state = BoyState.Idle;
                }
                else
                {
                    if (state != BoyState.StairsRight && state != BoyState.StairsLeft) 
                        state = BoyState.Idle;
                }
            }
            else
            {
                if (control.keyState.IsKeyDown(Keys.Right))
                {
                    direction = Direction.Right;
                    if (state == BoyState.Idle)
                        state = BoyState.Walk;
                }
                else if (control.keyState.IsKeyDown(Keys.Left))
                {
                    direction = Direction.Left;
                    if (state == BoyState.Idle)
                        state = BoyState.Walk;
                }
            }

        }

        public void update(GameTime time)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;
            frameTime += elapsed;
            checkInput(control);
            switch (state)
            {
                case BoyState.Idle:
                    if (Animation == "pushstill")
                        Animation = "endpush";
                    if (Animation == "endpush" && frame == 2 || Animation == "walk")
                        Animation = "stand";
                    moveSpeedX = 0;
                    moveSpeedY = 0;
                    break;
                case BoyState.Walk:
                    Animation = "walk";
                    moveSpeedX = 3;
                    moveSpeedY = 0;
                    break;
                case BoyState.StairsLeft:
                    Animation = "walk";
                    moveSpeedX = 3;
                    moveSpeedY = 2;
                    break;
                case BoyState.StairsRight:
                    Animation = "walk";
                    moveSpeedX = 3;
                    moveSpeedY = 2;
                    break;
                case BoyState.PushingStill:
                    moveSpeedX = 0;
                    moveSpeedY = 0;
                    if (Animation == "walk")
                        Animation = "startpush";
                    if (Animation == "startpush" && frame == 3)
                        Animation = "pushstill";
                    break;
                case BoyState.Teleport:
                    moveSpeedX = 0;
                    moveSpeedY = 0;
                    if (Animation == "walk" || Animation == "stand")
                    {
                        Animation = "enterwardrobe";
                        Rectangle target = ((Wardrobe)interactor).getLinkedWR().getBounds();
                        teleportTo = new Vector2(target.X + 25, target.Y+24);
                        interactor = null;
                    }
                    if (Animation == "enterwardrobe" && frame == 6)
                    {
                        position = new Vector2(teleportTo.X, teleportTo.Y);
                        Animation = "leavewardrobe";
                    }
                    if (Animation == "leavewardrobe" && frame == 7)
                    {
                        Animation = "stand";
                        state = BoyState.Idle;
                    }
                    break;
                case BoyState.TimeTravel:
                    moveSpeedX = 0;
                    moveSpeedY = 0;
                    if (Animation == "walk" || Animation == "stand")
                    {
                        Animation = "enterportrait";
                        interactor = null;
                    }
                    if (Animation == "enterportrait" && frame == 7)
                    {
                        if (control.timePeriod == TimePeriod.Past)
                            control.timePeriod = TimePeriod.Present;
                        else
                            control.timePeriod = TimePeriod.Past;
                        Animation = "leaveportrait";
                    }
                    if (Animation == "leaveportrait" && frame == 7)
                    {
                        Animation = "stand";
                        state = BoyState.Idle;
                    }
                    break;
            }
            if (frameTime >= frameLength)
            {
                int flip = 1;
                if (direction == Direction.Left)
                    flip = -1;
                X += moveSpeedX * flip;
                if (moveSpeedY == 0)
                {
                    moveSpeedY = 1;
                    flip = 1;
                }
                Y += moveSpeedY * flip;
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)(position.X), (int)(position.Y), Width, Height);
        }

        public bool isSolid()
        {
            return true;
        }

        public void Play()
        {
            if (soundCue.IsPrepared)
                soundCue.Play();
        }

        public void setCue(Cue cue)
        {
            soundCue = cue;
        }

        public Cue getCue()
        {
            return soundCue;
        }
    }
}
