using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Portrait : Drawable, Collideable, Interactive
    {
        private Vector2 position;
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
        private SpriteSheet sheet;

        public Portrait(int x, int y, SpriteSheet sheet)
        {
            position = new Vector2(x, y);
            this.sheet = sheet;
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            renderer.Draw(sheet.image, position, sheet.getSprite(0), tint, 0f, new Vector2(), 1f, SpriteEffects.None, 0.3f);       
        }

        public Rectangle getBounds()
        {
            return new Rectangle(X, Y, 35, 30); 
        }

        public bool isSolid()
        {
            return false;
        }

        public void Interact(Boy player)
        {
            player.state = Boy.BoyState.TimeTravel;
            player.X = X;
        }
    }
}
