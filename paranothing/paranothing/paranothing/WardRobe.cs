using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace paranothing
{
    class Wardrobe : Collideable, Audible, Updatable, Drawable, Interactive, Lockable
    {
        # region Attributes

        private GameController control = GameController.getInstance();

        //Collidable
        private Vector2 positionPres;
        private Vector2 positionPast1;
        private Vector2 positionPast2;
        public int X
        {
            get
            {
                switch (control.timePeriod)
                {
                    case TimePeriod.FarPast:
                        return (int)positionPast2.X;
                    case TimePeriod.Past:
                        return (int)positionPast1.X;
                    case TimePeriod.Present:
                        return (int)positionPres.X;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (control.timePeriod)
                {
                    case TimePeriod.FarPast:
                        positionPast2.X = value;
                        break;
                    case TimePeriod.Past:
                        positionPast1.X = value;
                        break;
                    case TimePeriod.Present:
                        positionPres.X = value;
                        break;
                    default:
                        break;
                }
            }
        }
        public int Y
        {
            get
            {
                switch (control.timePeriod)
                {
                    case TimePeriod.FarPast:
                        return (int)positionPast2.Y;
                    case TimePeriod.Past:
                        return (int)positionPast1.Y;
                    case TimePeriod.Present:
                        return (int)positionPres.Y;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (control.timePeriod)
                {
                    case TimePeriod.FarPast:
                        positionPast2.Y = value;
                        break;
                    case TimePeriod.Past:
                        positionPast1.Y = value;
                        break;
                    case TimePeriod.Present:
                        positionPres.Y = value;
                        break;
                    default:
                        break;
                }
            }
        }
        //Audible
        private Cue wrCue;
        //Drawable
        private SpriteSheet sheet;
        private Wardrobe linkedWR;
        private bool locked;
        private int frameTime;
        private int frameLength;
        private int frame;
        private string animName;
        private List<int> animFrames;
        private enum WardrobeState { Closed, Opening, Open }
        private WardrobeState state;
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

        # endregion

        # region Constructor

        public Wardrobe(int x, int y, SpriteSheet sheet, bool startLocked = false)
        {
            this.sheet = sheet;
            positionPres = new Vector2(x, y);
            positionPast1 = new Vector2(x, y);
            positionPast2 = new Vector2(x, y);
            locked = startLocked;
            if (locked)
            {
                Animation = "wardrobeclosed";
                state = WardrobeState.Closed;
            }
            else
            {
                Animation = "wardrobeopening";
                state = WardrobeState.Open;
            }
            frameLength = 80;

        }

        # endregion

        # region Methods

        //Collideable
        public Rectangle getBounds()
        {
            return new Rectangle(X, Y, 69, 82);
        }
        public bool isSolid()
        {
            return false;
        }

        //Audible
        public Cue getCue()
        {
            return wrCue;
        }

        public void setCue(Cue cue)
        {
            wrCue = cue;
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
            renderer.Draw(sheet.image, new Vector2(X, Y), sprite, tint, 0f, new Vector2(), 1f, SpriteEffects.None, 0.3f);            
        }

        //Updatable
        public void update(GameTime time)
        {
            switch (state)
            {
                case WardrobeState.Open:
                    Animation = "wardrobeopen";
                    break;
                case WardrobeState.Opening:
                    if (frame == 2)
                    {
                        Animation = "wardrobeopen";
                        state = WardrobeState.Open;
                    }
                    else
                        Animation = "wardrobeopening";
                    break;
                case WardrobeState.Closed:
                    Animation = "wardrobeclosed";
                    break;
            }
            if (frameTime >= frameLength)
            {   
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        public void setLinkedWR(Wardrobe linkedWR)
        {
            this.linkedWR = linkedWR;
        }

        public Wardrobe getLinkedWR()
        {
            return linkedWR;
        }

        public void lockObj()
        {
            locked = true;
        }

        public void unlockObj()
        {
            locked = false;
            state = WardrobeState.Opening;
        }

        public bool isLocked()
        {
            return locked;
        }

        public void Interact(Boy player)
        {
            player.state = Boy.BoyState.Teleport;
            player.X = X + 25;
        }

        # endregion
    }
}
