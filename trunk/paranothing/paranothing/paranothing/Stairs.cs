using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace paranothing
{
    class Stairs : Drawable, Collideable, Updatable, Interactive, Saveable
    {
        private GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        private SpriteSheet sheet;
        public Vector2 position;
        public int X { get { return (int)position.X; } set { position.X = value; } }
        public int Y { get { return (int)position.Y; } set { position.Y = value; } }
        private bool startIntact;
        private bool intact;
        public Direction direction;
        public float drawLayer;
        public Texture2D getImage()
        {
            return sheet.image;
        }

        public Stairs(float X, float Y, Direction direction)
        {
            this.sheet = sheetMan.getSheet("stair");
            position = new Vector2(X, Y);
            intact = true;
            this.direction = direction;
            this.startIntact = true;
            drawLayer = DrawLayer.Stairs;
        }

        public Stairs(float X, float Y, Direction direction, bool startIntact)
        {
            this.sheet = sheetMan.getSheet("stair");
            position = new Vector2(X, Y);
            intact = true;
            this.direction = direction;
            this.startIntact = startIntact;
            drawLayer = DrawLayer.Stairs;
        }

        public void update(GameTime time)
        {
            if (control.timePeriod == TimePeriod.Past)
                intact = true;
            else
                intact = startIntact;
            Boy player = control.player;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (direction == Direction.Left)
                flip = SpriteEffects.FlipHorizontally;
            Rectangle sprite;
            if (intact)
                sprite = sheet.getSprite(0);
            else
                sprite = sheet.getSprite(1);
            renderer.Draw(sheet.image, position, sprite, tint, 0f, new Vector2(), 1f, flip, drawLayer);
        }

        public bool isSolid()
        {
            return intact;
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 146, 112);
        }

        public Rectangle getSmallBounds()
        {
            return new Rectangle((int)position.X + (direction == Direction.Left ? 0 : 24), (int)position.Y + 22, 122, 190);
        }

        public void Interact()
        {
            Boy player = control.player;
            if (direction == Direction.Left)
                player.state = Boy.BoyState.StairsLeft;
            else
                player.state = Boy.BoyState.StairsRight;
            if (player.X + 30 >= X && player.X + 8 <= X)
            {
                player.direction = Direction.Right;
                player.X = X - 14;
            }
            else if (player.X + 30 >= X + getBounds().Width && player.X + 8 <= X + getBounds().Width)
            {
                player.direction = Direction.Left;
                player.X = X + getSmallBounds().Width;
            }
        }

        public string saveData()
        {
            return "StartStair\nx:" + X + "\ny:" + Y + "\ndirection:" + direction + "\nintact:" + (startIntact ? "true" : "false") + "\nEndStair";
        }
    }
}
