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
        private bool intact;
        
        public Texture2D getImage()
        {
            return sheet.image;
        }

        public Stairs(float X, float Y,SpriteSheet sheet)
        {

            this.sheet = sheet;
            position = new Vector2(X, Y);
            intact = true;

        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            Rectangle sprite = new Rectangle(0, 0, 146, 96);
            renderer.Draw(sheet.image, position, sprite, Color.White, 0f, new Vector2(), 1f, SpriteEffects.None, 0f);
        }

        public bool isSolid()
        {
            return intact;
        }

        public Rectangle getBounds()
        {

            return new Rectangle((int)position.X, (int)position.Y, 120, 96);

        }

    }
}
