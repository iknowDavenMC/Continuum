using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace paranothing
{
    class GameTitle : GameBackground
    {
        # region Attribute

        private Rectangle topTextRect;
        private Rectangle bottomTextRect;

        # endregion

        # region Constructor

        public GameTitle(Texture2D inTexture, Rectangle inRect)
            : base(inTexture, inRect)
        {
            topTextRect = new Rectangle();
            bottomTextRect = new Rectangle();
        }

        # endregion

        # region Methods

        //Accessors
        public Rectangle TopTextRectangle
        {
            get
            {
                return topTextRect;
            }
        }
        public void setTopTextRectangle(Vector2 inVec)
        {
            topTextRect.X = BackgoundRectangle.Center.X - (int)(inVec.X / 2);
            topTextRect.Y = BackgoundRectangle.Top;
            topTextRect.Width = (int)inVec.X;
            topTextRect.Height = (int)inVec.Y;
        }
        public Rectangle BottomTextRectangle
        {
            get
            {
                return bottomTextRect;
            }
        }
        public void setBottomTextRectangle(Vector2 inVec)
        {
            bottomTextRect.X = BackgoundRectangle.Center.X - (int)(inVec.X / 2);
            bottomTextRect.Y = BackgoundRectangle.Bottom - (int)inVec.Y;
            bottomTextRect.Width = (int)inVec.X;
            bottomTextRect.Height = (int)inVec.Y;
        }

        //Update
        public override void Update(Game1 game, KeyboardState keys)
        {
            //Presss enter to start
            if (keys.IsKeyDown(Keys.Enter))
            {
                game.GameState = GameLevel.Description;
            }
        }

        # endregion
    }
}
