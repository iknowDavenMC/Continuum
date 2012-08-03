using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Rubble : Collideable, Drawable
    {
        # region Attributes

        //Collideable
        private Vector2 position;
        private Rectangle bounds;
        //Drawable
        private SpriteSheet sheet;

        # endregion

        # region Constructor

        public Rubble(int X, int Y, int Width, int Height, SpriteSheet sheet)
        {
            this.sheet = sheet;
            position = new Vector2(X, Y);
            bounds = new Rectangle(X, Y, Width, Height);
        }

        # endregion

        # region Methods

        //Accessors & Mutators
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }

        //Collideable
        public Rectangle getBounds()
        {
            return bounds;
        }

        public bool isSolid()
        {
            return true;
        }

        //Drawable
        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            renderer.Draw(sheet.image, bounds, sheet.getSprite(0), tint, 0f, position, SpriteEffects.None, 0.3f);
        }

        #endregion
    }
}
