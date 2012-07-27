using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Boy : Drawable, Updatable
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
                if (sheet.hasAnimation(value))
                {
                    animName = value;
                    animFrames = sheet.getAnimation(animName);
                    frame = 0;
                    frameTime = 0;
                }
            }
        }
        private float moveSpeed; // Pixels per animation frame
        private Vector2 position;
        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }

        public enum BoyState { Idle, Walk }
        public BoyState state;
        public GameController.Direction direction;

        public Boy(float X, float Y, SpriteSheet sheet)
        {
            this.sheet = sheet;
            frame = 0;
            frameTime = 0;
            frameLength = 70;
            position = new Vector2(X, Y);
            Animation = "walkright";
            moveSpeed = 3;
            state = BoyState.Idle;
            direction = GameController.Direction.Right;
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer)
        {
            Rectangle sprite = sheet.getSprite(animFrames.ElementAt(frame));
            renderer.Draw(sheet.image, position, sprite, Color.White);
        }

        public void update(GameTime time, GameController control)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;
            frameTime += elapsed;

            switch (state)
            {
                case BoyState.Idle:
                    break;
                case BoyState.Walk:
                    moveSpeed = 3;
                    break;
            }
            if (frameTime >= frameLength)
            {
                int flip = 1;
                if (direction == GameController.Direction.Left)
                    flip = -1; 
                X += moveSpeed * flip;
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }
    }
}
