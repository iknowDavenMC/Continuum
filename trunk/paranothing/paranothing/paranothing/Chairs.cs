using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class Chairs : Collideable, Audible, Updatable, Drawable, Interactive, Saveable
    {
        # region Attributes

        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        //Collidable
        private Vector2 position;
        private int tX = 0, tY = 0;
        private int speed = 3;
        private Rectangle bounds;
        //Audible
        private Cue crCue;
        //Drawable
        private SpriteSheet sheet;
        private int frameTime;
        private int frameLength;
        private int frame;
        private string animName;
        private List<int> animFrames;
        private enum ChairsState { Down, Up, Moving }
        private ChairsState state;

        # endregion

        # region Constructor

        public Chairs(int x, int y, int width, int height, int frameLength, bool startLocked)
        {
            this.sheet = sheetMan.getSheet("chair");
            position = new Vector2(x, y);
            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            this.frameLength = frameLength;
        }

        public Chairs(string saveString)
        {
            this.sheet = sheetMan.getSheet("chair");
            string[] lines = saveString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            X = 0;
            Y = 0;
            int lineNum = 0;
            string line = "";
            while (!line.StartsWith("EndChairs") && lineNum < lines.Length)
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

        # endregion

        # region Methods

        //Accessors & Mutators
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

        //Collideable
        public Rectangle getBounds()
        {
            return bounds;
        }
        public bool isSolid()
        {
            return true;
        }

        //Audible
        public Cue getCue()
        {
            return crCue;
        }

        public void setCue(Cue cue)
        {
            crCue = cue;
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
            if (control.timePeriod == TimePeriod.Present)
            renderer.Draw(sheet.image, position, sheet.getSprite(1), tint, 0f, new Vector2(), 1f, SpriteEffects.None, 0.3f);
            else
            renderer.Draw(sheet.image, position, sheet.getSprite(0), tint, 0f, new Vector2(), 1f, SpriteEffects.None, 0.3f);
        }

        //Updatable
        public void update(GameTime time)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;
            switch (state)
            {
                case ChairsState.Idle:
                    break;
                case ChairsState.Up:
                    Animation = "chairup";
                    break;
                case ChairsState.Moving:
                    X += tX * speed / elapsed;
                    Y += tY * speed / elapsed;
                    break;
            }
            if (frameTime >= frameLength)
            {
                frameTime = 0;
                frame = (frame + 1) % animFrames.Count;
            }
        }

        public void move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    tY = -1;
                    break;
                case Direction.Down:
                    tY = 1;
                    break;
                case Direction.Left:
                    tX = -1;
                    break;
                case Direction.Right:
                    tX = 1;
                    break;
            }
        }

        public void drop()
        {
            state = ChairsState.Falling;
        }

        //Interactive
        public void Interact()
        {
        }

        public string saveData()
        {
            return "StartPortrait\nx:" + X + "\ny:" + Y + "\nEndPortrait";
        }

        # endregion
    }
}
