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
        private Vector2 positionOff;
        private bool solid;

        public float X { get { return position.X + positionOff.X; } set { position.X = value - positionOff.X; } }
        public float Y { get { return position.Y + positionOff.Y; } set { position.Y = value - positionOff.Y; } }

        public enum BoyState { Idle, Walk, Stairs }
        public BoyState state;
        public GameController.Direction direction;

        private Cue soundCue;

        public Boy(float X, float Y, SpriteSheet sheet)
        {
            this.sheet = sheet;
            frame = 0;
            frameTime = 0;
            frameLength = 70;
            position = new Vector2(X, Y);
            Width = 25;
            Height = 52;
            positionOff = new Vector2(20, 15);
            solid = true;
            state = BoyState.Idle;
            Animation = "standright";
            direction = GameController.Direction.Right;
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            Rectangle sprite = sheet.getSprite(animFrames.ElementAt(frame));
            renderer.Draw(sheet.image, position, sprite, tint, 0f, new Vector2(), 1f, SpriteEffects.None, 0.25f);
        }

        public void checkInput(GameController control)
        {

            if (control.keyState.IsKeyUp(Keys.Left) && control.keyState.IsKeyUp(Keys.Right))
            {
                if (direction == GameController.Direction.Right)
                {
                    Animation = "standright";
                    state = BoyState.Idle;
                }
                else
                {
                    Animation = "standleft";
                    state = BoyState.Idle;
                }
            }
            else
            {
                if (control.keyState.IsKeyDown(Keys.Right))
                {
                    Animation = "walkright";
                    direction = GameController.Direction.Right;
                    if (state != BoyState.Stairs)
                        state = BoyState.Walk;
                }
                else if (control.keyState.IsKeyDown(Keys.Left))
                {
                    Animation = "walkleft";
                    direction = GameController.Direction.Left;
                    if (state != BoyState.Stairs)
                        state = BoyState.Walk;
                }
            }

        }

        public void update(GameTime time, GameController control)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;
            frameTime += elapsed;
            checkInput(control);
            switch (state)
            {
                case BoyState.Idle:
                    moveSpeedX = 0;
                    moveSpeedY = 0;
                    break;
                case BoyState.Walk:
                    moveSpeedX = 3;
                    moveSpeedY = 0;
                    break;
                case BoyState.Stairs:
                    moveSpeedX = 3;
                    moveSpeedY = 2;
                    break;
            }
            if (frameTime >= frameLength)
            {
                int flip = 1;
                if (direction == GameController.Direction.Left)
                    flip = -1;
                X += moveSpeedX * flip;
                Y += moveSpeedY * flip;
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)(position.X + positionOff.X), (int)(position.Y + positionOff.Y), Width, Height);
        }

        public bool isSolid()
        {
            return solid;
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
