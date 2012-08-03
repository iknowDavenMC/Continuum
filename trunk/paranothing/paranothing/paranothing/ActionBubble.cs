using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paranothing
{
    class ActionBubble : Drawable//, Updatable
    {
        public enum BubbleAction { None, Wardrobe, Push, Portrait };
        private BubbleAction action;
        private bool negated;
        private bool visible;
        private SpriteSheet sheet;
        private Boy player;
        public Boy Player
        {
            set { player = value; }
        }
        
        private string animName;
        private int animIndex;
        public string Animation
        {
            get { return animName; }
            set
            {
               
                if (sheet.hasAnimation(value) && animName != value)
                {
                    animName = value;
                    animIndex = sheet.getAnimation(animName).First();
                }
            }
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
            switch (action)
            {
                case BubbleAction.Wardrobe:
                    Animation = "wardrobe";
                    break;
                case BubbleAction.Portrait:
                    Animation = "portrait";
                    break;
                case BubbleAction.Push:
                    Animation = "push";
                    break;
                default:
                    Animation = "negate";
                    break;
            }
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
                renderer.Draw(sheet.image, drawPos, sheet.getSprite(animIndex), tint);
                if (negated)
                    renderer.Draw(sheet.image, drawPos, sheet.getSprite(4), tint);
            }
        }

        //public void update(GameTime time, GameController control)
        //{

        //}
    }
}
