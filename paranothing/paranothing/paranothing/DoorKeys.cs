using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class DoorKeys : Drawable, Collideable, Saveable, Interactive
    {
        # region Attributes
        private static Dictionary<string, DoorKeys> keyDict = new Dictionary<string, DoorKeys>();
        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        //Collideable
        private Vector2 position;
        private Rectangle bounds { get { return new Rectangle(X, Y, 16, 9); } }
        //Drawable
        private SpriteSheet sheet;
        private Lockable lockedObj;
        public bool pickedUp;
        private string name;

        # endregion

        # region Constructor

        public DoorKeys(int X, int Y, string name)
        {
            this.sheet = sheetMan.getSheet("key");
            position = new Vector2(X, Y);
            pickedUp = false;
            this.name = name;

            if (keyDict.ContainsKey(name))
                keyDict.Remove(name);
            keyDict.Add(name, this);
        }

        public DoorKeys(string saveString)
        {
            this.sheet = sheetMan.getSheet("key");
            pickedUp = false;
            X = 0;
            Y = 0;
            name = "WR";
            string[] lines = saveString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int lineNum = 0;
            string line = "";
            while (!line.StartsWith("EndKey") && lineNum < lines.Length)
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
                if (line.StartsWith("name:"))
                {
                    name = line.Substring(5).Trim();
                }
                lineNum++;
            }

            if (keyDict.ContainsKey(name))
                keyDict.Remove(name);
            keyDict.Add(name, this);
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
            if (!pickedUp)
            {
                if (control.timePeriod == TimePeriod.Present)
                    renderer.Draw(sheet.image, bounds, sheet.getSprite(1), tint, 0f, new Vector2(), SpriteEffects.None, DrawLayer.Key);
                else
                    renderer.Draw(sheet.image, bounds, sheet.getSprite(0), tint, 0f, new Vector2(), SpriteEffects.None, DrawLayer.Key);
            }
        }

        //Interactive
        public void Interact()
        {
        }

        public static DoorKeys getKey(string name)
        {
            DoorKeys k;
            if (keyDict.ContainsKey(name))
                keyDict.TryGetValue(name, out k);
            else
                k = null;
            return k;
        }

        #endregion

        public string saveData()
        {
            return "StartKey\nx:" + X + "\ny:" + Y + "\nname:" + name + "\nEndKey"; 
        }
    }
}
