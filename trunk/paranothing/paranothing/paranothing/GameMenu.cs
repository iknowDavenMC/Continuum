using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paranothing
{
    class GameMenu : GameBackground
    {
        # region Attributes

        private Rectangle gameRect;
        private Rectangle controlRect;
        private Rectangle editLevelRect;
        private Rectangle creditRect;

        # endregion

        # region Constructor

        public GameMenu(Texture2D inTexture, Rectangle inRect)
            : base(inTexture, inRect)
        {
            gameRect = new Rectangle();
            controlRect = new Rectangle();
            editLevelRect = new Rectangle();
            creditRect = new Rectangle();
        }

        # endregion

        # region Methods

        //Accessors
        public Rectangle GameRectangle
        {
            get
            {
                return gameRect;
            }
        }
        public void setGameRectangle(Vector2 inVec)
        {
            gameRect.X = BackgoundRectangle.Center.X - (int)(inVec.X / 2);
            gameRect.Y = BackgoundRectangle.Top;
            gameRect.Width = (int)inVec.X;
            gameRect.Height = (int)inVec.Y;
        }
        public Rectangle ControlRectangle
        {
            get
            {
                return controlRect;
            }
        }
        public void setControlRectangle(Vector2 inVec)
        {
            controlRect.X = BackgoundRectangle.Center.X - (int)(inVec.X / 2);
            controlRect.Y = BackgoundRectangle.Top + gameRect.Y;
            controlRect.Width = (int)inVec.X;
            controlRect.Height = (int)inVec.Y;
        }
        public Rectangle EditLevelRectangle
        {
            get
            {
                return editLevelRect;
            }
        }
        public void setEditLevelRectangle(Vector2 inVec)
        {
            editLevelRect.X = BackgoundRectangle.Center.X - (int)(inVec.X / 2);
            editLevelRect.Y = BackgoundRectangle.Top + gameRect.Y;
            editLevelRect.Width = (int)inVec.X;
            editLevelRect.Height = (int)inVec.Y;
        }
        public Rectangle CreditRectangle
        {
            get
            {
                return creditRect;
            }
        }
        public void setCreditRectangle(Vector2 inVec)
        {
            creditRect.X = BackgoundRectangle.Center.X - (int)(inVec.X / 2);
            creditRect.Y = BackgoundRectangle.Top + gameRect.Y;
            creditRect.Width = (int)inVec.X;
            creditRect.Height = (int)inVec.Y;
        }

        //Update
        public override void Update(Game1 game, KeyboardState keys)
        {
            //Presss enter to start
            if (keys.IsKeyDown(Keys.Enter))
            {
                game.GameState = GameState.Game;
            }
        }

        # endregion
    }
}
