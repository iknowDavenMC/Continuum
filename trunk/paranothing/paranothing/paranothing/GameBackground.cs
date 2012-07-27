using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace paranothing
{
    class GameBackground
    {
        # region Attributes

        private Texture2D backgroundTexture;
        private Rectangle backgroundRect;

        # endregion

        # region Constructor

        public GameBackground(Texture2D inTexture, Rectangle inRect)
        {
            backgroundTexture = inTexture;
            backgroundRect = inRect;
        }

        # endregion

        # region Methods

        //Accessor
        public Rectangle BackgoundRectangle
        {
            get
            {
                return backgroundRect;
            }
        }

        //Update
        public virtual void Update(Game1 game, KeyboardState keys)
        {
            //Presss enter to continue
            if (keys.IsKeyDown(Keys.Space))
            {
                game.GameState = GameLevel.Level;
           }
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundRect, Color.White);
        }

        # endregion
    }
}
