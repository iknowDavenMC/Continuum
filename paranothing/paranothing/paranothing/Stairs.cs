using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace paranothing
{
    class Stairs : Drawable, Collideable, Updatable
    {

        private SpriteSheet sheet;
        public Vector2 position;
        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }
        private bool startIntact;
        private bool intact;
        public Direction direction;

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public Stairs(float X, float Y, Direction direction, SpriteSheet sheet, bool startIntact = true)
        {
            this.sheet = sheet;
            position = new Vector2(X, Y);
            intact = true;
            this.direction = direction;
            this.startIntact = startIntact;
        }

        public void update(GameTime time, GameController control)
        {
            if (control.timePeriod == TimePeriod.Past)
                intact = true;
            else
                intact = startIntact;
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
            renderer.Draw(sheet.image, position, sprite, Color.White, 0f, new Vector2(), 1f, flip, 0f);
        }

        public bool isSolid()
        {
            return intact;
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 124, 86);
        }

    }
}
