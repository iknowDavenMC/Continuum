using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace paranothing
{
    class Wardrobe// : Collideable, Audible, Updatable, Drawable
    {
        # region Attributes

        //Collidable
        private Rectangle wrBound;
        private bool wrSolid;
        //Audible
        private Cue wrCue;
        //Drawable
        private Texture2D wrTexture;
        private Wardrobe linkedWR;

        # endregion

        # region Constructor

        public Wardrobe(Texture2D inTexture, Rectangle inRect, bool inSolid)
        {
            wrTexture = inTexture;
            wrBound = inRect;
            wrSolid = inSolid;
        }

        # endregion

        # region Methods

        //Collideable
        public Rectangle getBound
        {
            get
            {
                return wrBound;
            }
        }
        public bool isSolid
        {
            get
            {
                return wrSolid;
            }
        }

        //Audible
        public Cue WardRobeCue
        {
            get
            {
                return wrCue;
            }
            set
            {
                wrCue = value;
            }
        }
        public void Play()
        {

        }

        //Drawable
        public Texture2D getImage
        {
            get
            {
                return wrTexture;
            }
        }
        public void draw(SpriteBatch reneder)
        {

        }
        public void draw(SpriteBatch reneder, Color tint)
        {

        }

        //Updatable
        public void update(GameTime time, GameController control)
        {

        }

        public void setLinkedWR(Wardrobe linkedWR)
        {
            this.linkedWR = linkedWR;
        }

        public Wardrobe getLinkedWR()
        {
            return linkedWR;
        }

        # endregion
    }
}
