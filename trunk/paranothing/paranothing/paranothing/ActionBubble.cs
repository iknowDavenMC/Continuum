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
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        public enum BubbleAction { None, Wardrobe, Push, Portrait, Stair };
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
        private int negateInd;

        public ActionBubble()
        {
            this.sheet = sheetMan.getSheet("action");
            this.action = BubbleAction.None;
            visible = false;
            negated = false;
            if (sheet.hasAnimation("negate"))
            {
                negateInd = sheet.getAnimation("negate").First();
            }
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
                case BubbleAction.Stair:
                    Animation = "stair";
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
                Vector2 drawPos = new Vector2(player.X + 11, player.Y - 27);
                renderer.Draw(sheet.image, drawPos, sheet.getSprite(0), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.ActionBubble);
                renderer.Draw(sheet.image, drawPos, sheet.getSprite(animIndex), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.ActionBubble - 0.001f);
                if (negated)
                    renderer.Draw(sheet.image, drawPos, sheet.getSprite(negateInd), tint, 0f, new Vector2(), 1f, SpriteEffects.None, DrawLayer.ActionBubble - 0.002f);
            }
        }

        //public void update(GameTime time, GameController control)
        //{

        //}
    }
}
