using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Floor : Drawable, Collideable
    {
        private Vector2 position;
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }
        public int Width, Height;
        private Rectangle Box
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }
        private Texture2D image;

        public Floor(int X, int Y, int Width, int Height, Texture2D image)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.image = image;
        }

        public Rectangle getBounds()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        public bool isSolid()
        {
            return true;
        }

        public Texture2D getImage()
        {
            return image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            renderer.Draw(image, Box, tint);
        }
    }
}
