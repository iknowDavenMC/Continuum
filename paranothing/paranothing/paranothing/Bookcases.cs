using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Bookcases : Collideable, Audible, Updatable, Drawable, Interactive, Lockable, Saveable
    {
        # region Attributes

        private static Dictionary<string, Bookcases> bookcasesDict = new Dictionary<string, Bookcases>();
        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        //Collidable
        private Vector2 position;
        private string name;
        private string keyName = "";
        public int X
        {
            get
            {
                return (int)position.X;
            }
            set
            {
                position.X = value;
            }
        }
        public int Y
        {
            get
            {
                return (int)position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public Rectangle enterBox
        {
            get { return new Rectangle(X + 24, Y + 9, 23, 73); }
        }

        public Rectangle pushBox
        {
            get { return new Rectangle(X + 2, Y + 2, 65, 78); }
        }

        //Audible
        private Cue bcCue;
        //Drawable
        private SpriteSheet sheet;
        private string linkedName;
        private bool locked;
        private bool startLocked;
        private int frameTime;
        private int frameLength;
        private int frame;
        private string animName;
        private List<int> animFrames;
        private enum BookcasesState { Closed, Opening, Open }
        private BookcasesState state;
        public string Animation
        {
            get { return animName; }
            set
            {

                if (sheet.hasAnimation(value) && animName != value)
                {
                    animName = value;
                    animFrames = sheet.getAnimation(animName);
                    frame = 0;
                    frameTime = 0;
                }
            }
        }

        # endregion

        # region Constructors

        public Bookcases(int x, int y, string name, bool startLocked = false)
        {
            this.sheet = sheetMan.getSheet("bookcases");
            position = new Vector2(x, y);
            this.startLocked = startLocked;
            locked = startLocked;
            if (startLocked)
            {
                Animation = "bookcasesclosed";
                state = BookcasesState.Closed;
            }
            else
            {
                Animation = "bookcasesopening";
                state = BookcasesState.Open;
            }
            frameLength = 160;
            this.name = name;
            if (bookcasesDict.ContainsKey(name))
                bookcasesDict.Remove(name);
            bookcasesDict.Add(name, this);
        }

        public Bookcases(string saveString)
        {
            this.sheet = sheetMan.getSheet("bookcases");
            int x = 0;
            int y = 0;
            startLocked = false;
            name = "BC";
            string link = "BC";
            string[] lines = saveString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int lineNum = 0;
            string line = "";
            while (!line.StartsWith("EndBookcases") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { x = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("name:"))
                {
                    name = line.Substring(8).Trim();
                }
                if (line.StartsWith("link:"))
                {
                    link = line.Substring(8).Trim();
                }
                lineNum++;
            }
            locked = startLocked;
            if (startLocked)
            {
                Animation = "bookcasesclosed";
                state = BookcasesState.Closed;
            }
            else
            {
                Animation = "bookcasesopening";
                state = BookcasesState.Open;
            }
            position = new Vector2(x, y);
            if (bookcasesDict.ContainsKey(name))
                bookcasesDict.Remove(name);
            bookcasesDict.Add(name, this);
            setLinkedBC(link);
        }

        # endregion

        # region Methods

        //Collideable
        public Rectangle getBounds()
        {
            return pushBox;
        }
        public bool isSolid()
        {
            return false;
        }

        //Audible
        public Cue getCue()
        {
            return bcCue;
        }

        public void setCue(Cue cue)
        {
            bcCue = cue;
        }

        public void Play()
        {

        }

        //Drawable
        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            Rectangle sprite = sheet.getSprite(animFrames.ElementAt(frame));
            renderer.Draw(sheet.image, new Vector2(X, Y), sprite, tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.Wardrobe);
        }

        //Updatable
        public void update(GameTime time)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;
            frameTime += elapsed;

            if (keyName != "")
            {
                DoorKeys k = DoorKeys.getKey(keyName);
                if (k != null)
                {
                    if (k.pickedUp && state == BookcasesState.Closed)
                    {
                        state = BookcasesState.Opening;
                    }
                }
            }

            switch (state)
            {
                case BookcasesState.Open:
                    Animation = "bookasesopen";
                    break;
                case BookcasesState.Opening:
                    frameLength = 100;
                    if (frame == 2)
                    {
                        Animation = "bookcasesopen";
                        state = BookcasesState.Open;
                        unlockObj();
                    }
                    else
                        Animation = "bookcasesopening";
                    break;
                case BookcasesState.Closed:
                    Animation = "bookcasesclosed";
                    break;
            }
            if (frameTime >= frameLength)
            {
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        public void setLinkedBC(string linkedName)
        {
            this.linkedName = linkedName;
        }

        public Bookcases getLinkedBC()
        {
            Bookcases w;
            if (bookcasesDict.ContainsKey(linkedName))
                bookcasesDict.TryGetValue(linkedName, out w);
            else
                w = null;
            return w;
        }

        public void lockObj()
        {
            locked = true;
        }

        public void unlockObj()
        {
            locked = false;
        }

        public bool isLocked()
        {
            return locked;
        }

        public void Interact()
        {
            Boy player = control.player;
            if (Rectangle.Intersect(player.getBounds(), enterBox).Width != 0)
            {
                Bookcases linkedBC = getLinkedBC();
                if (!locked && linkedBC != null && !linkedBC.isLocked() && !control.collidingWithSolid(linkedBC.enterBox))
                {
                    player.state = Boy.BoyState.Teleport;
                    player.X = X + 16;
                }
            }
            else
            {
                if (control.collidingWithSolid(pushBox, false))
                    player.state = Boy.BoyState.PushingStill;
                else
                    player.state = Boy.BoyState.PushWalk;
                if (player.X > X)
                {
                    player.X = X + 67;
                    player.direction = Direction.Left;
                }
                else
                {
                    player.X = X - 36;
                    player.direction = Direction.Right;
                }
            }
        }

        public string saveData()
        {
            return "StartBookcases\nx:" + X + "\ny:" + Y + "\nname:" + name + "\nlocked:" + startLocked + "\nlink:" + linkedName + "\nkeyName:" + keyName + "\nEndBookcases";
        }

        public void setKeyName(string keyName)
        {
            this.keyName = keyName;
        }

        # endregion
    }
}
