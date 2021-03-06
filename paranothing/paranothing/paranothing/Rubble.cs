﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Rubble : Collideable, Drawable, Saveable
    {
        # region Attributes
        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        //Collideable
        private Vector2 position;
        //Drawable
        private SpriteSheet sheet;

        # endregion

        # region Constructor

        public Rubble(int X, int Y)
        {
            this.sheet = sheetMan.getSheet("rubble");
            position = new Vector2(X, Y);
        }

        public Rubble(string saveString)
        {
            this.sheet = sheetMan.getSheet("rubble");
            X = 0;
            Y = 0;
            int lineNum = 0;
            string line = "";
            string[] lines = saveString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            while (!line.StartsWith("EndRubble") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                lineNum++;
            }
        }

        public void reset() { }

        # endregion

        # region Methods

        //Accessors & Mutators
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }
        private Rectangle bounds { get { return new Rectangle(X, Y, 37, 28); }}

        //Collideable
        public Rectangle getBounds()
        {
            return bounds;
        }

        public bool isSolid()
        {
            if (control.timePeriod == TimePeriod.Present)
                return true;
            return false;
        }

        //Drawable
        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            if (control.timePeriod == TimePeriod.Present)
                renderer.Draw(sheet.image, bounds, sheet.getSprite(0), tint, 0f, new Vector2(), SpriteEffects.None, DrawLayer.Rubble);
        }

        public string saveData()
        {
            return "StartRubble\nx:" + X + "\ny:" + Y + "\nEndRubble";
        }

        #endregion
    }
}
