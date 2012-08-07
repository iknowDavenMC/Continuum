using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Floor : Drawable, Collideable, Saveable
    {
        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        private Vector2 position;
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }
        public int Width, Height;
        private Rectangle Box
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }
        private SpriteSheet sheet;

        public Floor(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.sheet = sheetMan.getSheet("floor");
        }

        public Rectangle getBounds()
        {
            return Box;
        }

        public bool isSolid()
        {
            return true;
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            if (control.timePeriod == TimePeriod.Present)
                renderer.Draw(sheet.image, Box, sheet.getSprite(1), tint, 0f, new Vector2(), SpriteEffects.None, DrawLayer.Floor);
            else
                renderer.Draw(sheet.image, Box, sheet.getSprite(0), tint, 0f, new Vector2(), SpriteEffects.None, DrawLayer.Floor);
        }

        public string saveData()
        {
            return "StartFloor\nx:" + X + "\ny:" + Y + "\nwidth:" + Width + "\nheight:" + Height + "\nEndFloor";
        }
    }
}
