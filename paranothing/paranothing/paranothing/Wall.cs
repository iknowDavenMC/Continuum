using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Wall : Drawable, Collideable, Updatable, Saveable
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
        private bool intact;
        private bool startIntact;

        public Wall(int X, int Y, int Width, int Height, bool startIntact = true)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.sheet = sheetMan.getSheet("wall");
            this.startIntact = startIntact;
        }

        public Rectangle getBounds()
        {
            return Box;
        }

        public bool isSolid()
        {
            return intact;
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void update(GameTime time)
        {
            if (control.timePeriod == TimePeriod.Past)
                intact = true;
            else
                intact = startIntact;
        }
        public void draw(SpriteBatch renderer, Color tint)
        {
            if (intact)
                renderer.Draw(sheet.image, Box, sheet.getSprite(0), tint, 0f, new Vector2(), SpriteEffects.None, DrawLayer.Background - 0.01f);
        }

        public string saveData()
        {
            return "StartWall\nx:" + X + "\ny:" + Y + "\nwidth:" + Width + "\nheight:" + Height + "\nintact:" + (startIntact? "true" : "false") + "\nEndWall";
        }
    }
}
