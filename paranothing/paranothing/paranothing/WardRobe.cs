using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class WardRobe : Collideable, Drawable, Updatable
    {
        # region Attributes

        //Collidable
        private Rectangle wrBound;
        private bool wrSolid;
        //Drawable
        private Texture2D wrTexture;

        # endregion

        # region Constructor

        public WardRobe(Texture2D inTexture, bool inSolid)
        {
            wrTexture = inTexture;
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

        # endregion
    }
}
