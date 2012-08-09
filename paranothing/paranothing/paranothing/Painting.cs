using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Portrait : Drawable, Collideable, Interactive, Saveable
    {
        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
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

        public Portrait(int x, int y)
        {
            position = new Vector2(x, y);
            this.sheet = sheetMan.getSheet("portrait");
        }

        public Portrait(string saveString, string str)
        {
            this.sheet = sheetMan.getSheet("portrait");
            parseString(saveString, str);
        }

        //present past constructor
        public Portrait(string saveString, TimePeriod period)
        {
            this.sheet = sheetMan.getSheet("portrait");
            if (control.timePeriod == TimePeriod.Present)
            {
                parseString(saveString, "EndPresentPortrait");
            }
            if (control.timePeriod == TimePeriod.FarPast)
            {
                parseString(saveString, "EndPastPortrait");
            }
        }
        private void parseString(string saveString, string str)
        {
            string[] lines = saveString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            X = 0;
            Y = 0;
            int lineNum = 0;
            string line = "";
            while (!line.StartsWith(str) && lineNum < lines.Length)
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

        public void reset(){}

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            if (control.timePeriod == TimePeriod.Present)
            renderer.Draw(sheet.image, position, sheet.getSprite(1), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.Background);  
            else
            renderer.Draw(sheet.image, position, sheet.getSprite(0), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.Background);       
        }

        public Rectangle getBounds()
        {
            return new Rectangle(X, Y, 35, 30); 
        }

        public bool isSolid()
        {
            return false;
        }

        public void Interact()
        {
            Boy player = control.player;
            player.state = Boy.BoyState.TimeTravel;
            player.X = X;
        }

        public string saveData()
        {
            return "StartPortrait\nx:" + X + "\ny:" + Y + "\nEndPortrait";
        }
    }
}
