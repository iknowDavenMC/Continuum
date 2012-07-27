using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace paranothing
{
    class Stairs : Drawable, Collideable
    {

        private SpriteSheet sheet;
        private Vector2 position; 


        public Texture2D getImage()
        {
            return sheet.image;
        }

        public Stairs(float X, float Y,SpriteSheet sheet)
        {

            this.sheet = sheet;
            position = new Vector2(X, Y);

        }

        public void draw(SpriteBatch renderer)
        {
            Rectangle sprite = new Rectangle(0, 0, 146, 96);
            renderer.Draw(sheet.image, position, sprite, Color.White);
        }

        public bool isSolid()
        {

            return true;

        }

        public Rectangle getBounds()
        {

            return new Rectangle(0,0,0,0);

        }

    }
}
