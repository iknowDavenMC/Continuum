using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Shadows : Collideable, Updatable, Drawable, Audible
    {
        # region Attributes

        //Drawable
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
        //Collideable
        private float moveSpeedX, moveSpeedY; // Pixels per animation frame
        private Vector2 position;
        private float patrolDisatnce;
        private Rectangle bounds;
        public int Width, Height;
        public enum ShadowState { Idle, Walk }
        public ShadowState state;
        public Direction direction;
        //Audible
        private Cue soundCue;

        # endregion

        # region Constructor

        public Shadows(float X, float Y, float distance, SpriteSheet sheet)
        {
            this.sheet = sheet;
            frame = 0;
            frameTime = 0;
            frameLength = 70;
            position = new Vector2(X, Y);
            Width = 38;
            Height = 58;
            patrolDisatnce = distance;
            state = ShadowState.Idle;
            Animation = "stand";
            direction = Direction.Right;
            bounds = new Rectangle((int)(position.X), (int)(position.Y), Width, Height);
        }

        # endregion

        # region Methods

        //Accessors & Mutators
        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }
        public float PatrolDistance { get { return patrolDisatnce; } set { patrolDisatnce = value; } }

        //Updatable
        public void update(GameTime time)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;
            frameTime += elapsed;
            switch (state)
            {
                case ShadowState.Idle:
                    if (frame == 2 || Animation == "walk")
                        Animation = "stand";
                    moveSpeedX = 0;
                    moveSpeedY = 0;
                    break;
                case ShadowState.Walk:
                    Animation = "walk";
                    moveSpeedX = 3;
                    moveSpeedY = 0;
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

        //Drawable
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

        //Collideable
        public Rectangle getBounds()
        {
            return bounds;
        }

        public bool isSolid()
        {
            return false;
        }

        //Audible
        public void setCue(Cue cue)
        {
            soundCue = cue;
        }

        public Cue getCue()
        {
            return soundCue;
        }

        public void Play()
        {
            if (soundCue.IsPrepared)
                soundCue.Play();
        }

        # endregion
    }
}
