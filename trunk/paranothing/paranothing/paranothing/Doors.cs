using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Doors : Collideable, Audible, Updatable, Drawable, Interactive
    {
        # region Attributes

        //Collidable
        private Vector2 position;
        private Rectangle bounds;
        //Audible
        private Cue drCue;
        //Drawable
        private SpriteSheet sheet;
        private bool locked;
        private int frameTime;
        private int frameLength;
        private int frame;
        private string animName;
        private List<int> animFrames;
        private enum DoorsState { Closed, Opening, Open }
        private DoorsState state;

        # endregion

        # region Constructor

        public Doors(int x, int y, SpriteSheet sheet, bool startLocked)
        {
            this.sheet = sheet;
            position = new Vector2(x, y);
            bounds = new Rectangle((int)position.X, (int)position.Y, 69, 82);
            locked = startLocked;
            if (locked)
            {
                Animation = "doorsclosed";
                state = DoorsState.Closed;
            }
            else
            {
                Animation = "doorsopening";
                state = DoorsState.Open;
            }
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
        public bool isLocked()
        {
            return locked;
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
        public Cue getCue()
        {
            return drCue;
        }

        public void setCue(Cue cue)
        {
            drCue = cue;
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
        public void update(GameTime time, GameController control)
        {
            switch (state)
            {
                case DoorsState.Open:
                    Animation = "doorsopen";
                    break;
                case DoorsState.Opening:
                    if (frame == 2)
                    {
                        Animation = "doorsopen";
                        state = DoorsState.Open;
                    }
                    else
                        Animation = "doorsopening";
                    break;
                case DoorsState.Closed:
                    Animation = "doorsclosed";
                    break;
            }
            if (frameTime >= frameLength)
            {
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        //Interactive
        public void Interact(Boy player)
        {
            //player.state = Boy.BoyState.Teleport;
            //player.X = X + 25;
        }

        # endregion
    }
}
