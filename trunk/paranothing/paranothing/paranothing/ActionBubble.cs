using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class ActionBubble : Drawable, Updatable
    {
        public enum BubbleAction { None, Wardrobe, Portrait };
        private BubbleAction action;
        private bool negated;
        private bool visible;
        private SpriteSheet sheet;
        private Boy player;
        public Boy Player
        {
            set { player = value; }
        }

        public ActionBubble(SpriteSheet sheet)
        {
            this.sheet = sheet;
            this.action = BubbleAction.None;
            visible = false;
            negated = false;
        }

        public bool isVisible()
        {
            return visible;
        }

        public void show()
        {
            visible = true;
        }

        public void hide()
        {
            visible = false;
        }

        public void setAction(BubbleAction action, bool negated)
        {
            this.action = action;
            this.negated = negated;
        }

        public Texture2D getImage()
        {
            return sheet.image;
        }

        public void draw(SpriteBatch renderer, Color tint)
        {
            if (visible)
            {
                Vector2 drawPos = new Vector2(player.X + 8, player.Y - 30);
                renderer.Draw(sheet.image, drawPos, sheet.getSprite(0), tint);
                switch (action)
                {
                    case BubbleAction.Wardrobe:
                        renderer.Draw(sheet.image, drawPos, sheet.getSprite(1), tint);
                        break;
                    case BubbleAction.Portrait:
                        renderer.Draw(sheet.image, drawPos, sheet.getSprite(2), tint);
                        break;
                    default:
                        break;
                }
                if (negated)
                    renderer.Draw(sheet.image, drawPos, sheet.getSprite(3), tint);
            }
        }

        public void update(GameTime time, GameController control)
        {

        }
    }
}
