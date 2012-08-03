using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Chairs : Collideable, Audible, Updatable, Drawable, Interactive
    {
        # region Attributes

        private GameController control = GameController.getInstance();
        //Collidable
        private Vector2 position;
        private Rectangle bounds;
        //Audible
        private Cue crCue;
        //Drawable
        private SpriteSheet sheet;
        private int frameTime;
        private int frameLength;
        private int frame;
        private string animName;
        private List<int> animFrames;
        private enum ChairsState { Down, Lifting, Up, Moving, Falling }
        private ChairsState state;

        # endregion

        # region Constructor

        public Chairs(int x, int y, int width, int height, int frameLength, SpriteSheet sheet, bool startLocked)
        {
            this.sheet = sheet;
            position = new Vector2(x, y);
            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            this.frameLength = frameLength;
        }

        # endregion

        # region Methods

        //Accessors & Mutators
        public int X
        {
            get { return (int)position.X; }
            set { position.X = value; }
        }
        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value; }
        }
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
        public Rectangle getBounds()
        {
            return bounds;
        }
        public bool isSolid()
        {
            return true;
        }

        //Audible
        public Cue getCue()
        {
            return crCue;
        }

        public void setCue(Cue cue)
        {
            crCue = cue;
        }

        public void Play()
        {

        }

        //Drawable
        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            Rectangle sprite = sheet.getSprite(animFrames.ElementAt(frame));
            renderer.Draw(sheet.image, position, sprite, tint, 0f, new Vector2(), 1f, SpriteEffects.None, 0.3f);
        }

        //Updatable
        public void update(GameTime time)
        {
            switch (state)
            {
                case ChairsState.Down:
                    Animation = "chairdown";
                    break;
                case ChairsState.Lifting:
                    if (frame == 2)
                    {
                        Animation = "chairup";
                        state = ChairsState.Up;
                    }
                    else
                        Animation = "chairlifting";
                    break;
                case ChairsState.Up:
                    Animation = "chairup";
                    break;
                case ChairsState.Moving:
                    Animation = "chairmoving";
                    break;
                case ChairsState.Falling:
                    if (frame == 4)
                    {
                        Animation = "chairdown";
                        state = ChairsState.Down;
                    }
                    else
                        Animation = "chairmoving";
                    break;
            }
            if (frameTime >= frameLength)
            {
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        //Interactive
        public void Interact()
        {
        }

        # endregion
    }
}
