using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    public class Chairs : Collideable, Audible, Updatable, Drawable, Interactive, Saveable
    {
        # region Attributes

        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        //Collidable
        private Vector2 position;
        private int tX = 0, tY = 0;
        private int speed = 3;
        private int moveTime = 0;
        private int movelength = 70;
        private Rectangle bounds { get { return new Rectangle(X, Y, 40, 52); } }
        //Audible
        private Cue crCue;
        //Drawable
        private SpriteSheet sheet;
        public enum ChairsState { Idle, Falling, Moving }
        public ChairsState state;
        private ActionBubble bubble = new ActionBubble();

        # endregion

        # region Constructor

        public Chairs(int x, int y, int width, int height, int frameLength, bool startLocked)
        {
            this.sheet = sheetMan.getSheet("chair");
            position = new Vector2(x, y);
            bubble.chair = this;
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
            bubble.chair = this;
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

        //Collideable
        public Rectangle getBounds()
        {
            return bounds;
        }
        public bool isSolid()
        {
            return false;
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
                renderer.Draw(sheet.image, position, sheet.getSprite(1), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.Wardrobe - 0.005f);
            else
                renderer.Draw(sheet.image, position, sheet.getSprite(0), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.Wardrobe - 0.005f);
            bubble.draw(renderer, tint);
        }

        //Updatable
        public void update(GameTime time)
        {
            Boy player = control.player;
            int elapsed = time.ElapsedGameTime.Milliseconds;
            switch (state)
            {
                case ChairsState.Idle:
                    if (player.nearestChair != null && player.nearestChair != this)
                    {
                        if (player.nearestChair.state == ChairsState.Idle)
                        {
                            Vector2 oldDist = new Vector2(player.X - player.nearestChair.X, player.Y - player.nearestChair.Y);
                            Vector2 newDist = new Vector2(player.X - X, player.Y - Y);
                            if (newDist.LengthSquared() < oldDist.LengthSquared())
                            {
                                player.nearestChair = this;
                                bubble.show();
                                bubble.setAction(ActionBubble.BubbleAction.Chair, false);
                            }
                            else
                                bubble.hide();
                        }
                    }
                    else
                    {
                        player.nearestChair = this;
                        bubble.show();
                        bubble.setAction(ActionBubble.BubbleAction.Chair, false);
                    }
                    break;
                case ChairsState.Falling:
                    bubble.hide();
                    Y++;
                    break;
                case ChairsState.Moving:
                    bubble.hide();
                    moveTime += elapsed;
                    if (moveTime >= movelength)
                    {
                        X += tX * speed;
                        Y += tY * speed;
                        moveTime = 0;

                        Rectangle smallerBound = new Rectangle(X + 2, Y + 2, bounds.Width - 4, bounds.Height - 4);
                        if (control.collidingWithSolid(smallerBound, false))
                        {
                            X -= tX * speed;
                            Y -= tY * speed;
                        }
                        tX = 0;
                        tY = 0;
                    }
                    break;
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
            return "StartChairs\nx:" + X + "\ny:" + Y + "\nEndChairs";
        }

        //reset
        public void reset()
        {

        }

        # endregion
    }
}
