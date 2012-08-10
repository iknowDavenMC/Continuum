using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace paranothing
{
    class GameTitle : GameBackground
    {
        # region Attribute

        private Rectangle topTextRect;
        private Rectangle bottomTextRect;

        private int menuIndex;
        private int menuSize = 5;
        private GamePadState prevPad;
        private KeyboardState prevKeys;
        private bool toggleSound = true;
        private bool toggleMusic = true;
        private String soundText = "ON";
        private String musicText = "ON";
        private bool pause = false;

        private Color[] colors = new Color[5]{Color.Yellow, Color.White, Color.White, Color.White, Color.White};

        private enum TitleState
        {
            Title,
            Menu,
            Options,
            Controls,
            Credits,
            Pause
        }

        private TitleState titleState = TitleState.Title;

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

            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            //Presss enter to start
            if ((keys.IsKeyDown(Keys.Enter) || padState.IsButtonDown(Buttons.Start)) && titleState == TitleState.Title)
            {
                //game.GameState = GameState.Game;
                titleState = TitleState.Menu;

            }

            //Selecting menu options
            else if ((padState.IsButtonDown(Buttons.LeftThumbstickDown) || keys.IsKeyDown(Keys.Down)) && !(prevPad.IsButtonDown(Buttons.LeftThumbstickDown) || prevKeys.IsKeyDown(Keys.Down)) && menuIndex < menuSize)
            {

               

                colors[menuIndex] = Color.White;

                menuIndex++;

                colors[menuIndex] = Color.Yellow;

            }

            else if ((padState.IsButtonDown(Buttons.LeftThumbstickUp) || keys.IsKeyDown(Keys.Up)) && !(prevPad.IsButtonDown(Buttons.LeftThumbstickUp) || prevKeys.IsKeyDown(Keys.Up)) && menuIndex > 0)
            {

                

                colors[menuIndex] = Color.White;

                menuIndex--;

                colors[menuIndex] = Color.Yellow;

            }

            else if (titleState == TitleState.Menu && (keys.IsKeyDown(Keys.Enter) || padState.IsButtonDown(Buttons.A)) && !(prevKeys.IsKeyDown(Keys.Enter) || prevPad.IsButtonDown(Buttons.A)))
            {
                if (menuIndex == 0)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    colors[menuIndex] = Color.Yellow;
                    game.GameState = GameState.Game;
                }
                else if (menuIndex == 1) //TODO: ADD RESUME GAME
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    colors[menuIndex] = Color.Yellow;
                    game.GameState = GameState.Game;
                }
                else if (menuIndex == 2) 
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    colors[menuIndex] = Color.Yellow;
                    menuSize = 3;
                    titleState = TitleState.Options;
                }
                else if (menuIndex == 3)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 0;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Controls;
                }
                else if (menuIndex == 4)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 0;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Credits;
                }

            }

            else if (titleState == TitleState.Options && (keys.IsKeyDown(Keys.Enter) || padState.IsButtonDown(Buttons.A)) && !(prevKeys.IsKeyDown(Keys.Enter) || prevPad.IsButtonDown(Buttons.A)))
            {

                if (menuIndex == 0 && toggleSound)
                {
                    soundText = "OFF";
                    toggleSound = false;
                }

                else if (menuIndex == 0 && !toggleSound)
                {
                    soundText = "ON";
                    toggleSound = true;
                }

                else if (menuIndex == 1 && toggleMusic)
                {
                    musicText = "OFF";
                    toggleMusic = false;
                    MediaPlayer.Pause();
                }

                else if (menuIndex == 1 && !toggleMusic)
                {
                    musicText = "ON";
                    toggleMusic = true;
                    MediaPlayer.Resume();
                }

                else if (menuIndex == 2 && !game.gameInProgress)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 5;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Menu;
                }

                else if (menuIndex == 2 && game.gameInProgress)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 4;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Pause;
                }

            }

            else if (titleState == TitleState.Controls && (keys.IsKeyDown(Keys.Enter) || padState.IsButtonDown(Buttons.A)) && !(prevKeys.IsKeyDown(Keys.Enter) || prevPad.IsButtonDown(Buttons.A)))
            {

                if (menuIndex == 0 && !game.gameInProgress)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 5;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Menu;
                }

                else if (menuIndex == 0 && game.gameInProgress)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 4;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Pause;
                }

            }

            else if (titleState == TitleState.Credits && (keys.IsKeyDown(Keys.Enter) || padState.IsButtonDown(Buttons.A)) && !(prevKeys.IsKeyDown(Keys.Enter) || prevPad.IsButtonDown(Buttons.A)))
            {

                if (menuIndex == 0 && !game.gameInProgress)
                {
                    colors[menuIndex] = Color.White;
                    menuIndex = 0;
                    menuSize = 5;
                    colors[menuIndex] = Color.Yellow;
                    titleState = TitleState.Menu;
                }

            }


            prevKeys = keys;
            prevPad = padState;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            if (titleState == TitleState.Menu)
            {
                spriteBatch.DrawString(Game1.menuFont, "New Game", new Vector2(0, 150), colors[0]);
                spriteBatch.DrawString(Game1.menuFont, "Continue", new Vector2(0, 190), colors[1]);
                spriteBatch.DrawString(Game1.menuFont, "Options", new Vector2(0, 230), colors[2]);
                spriteBatch.DrawString(Game1.menuFont, "Controls", new Vector2(0, 270), colors[3]);
                spriteBatch.DrawString(Game1.menuFont, "Credits", new Vector2(0, 310), colors[4]);
            }

            if (titleState == TitleState.Options)
            {
            
                //TODO: FIX OPTIONS
                
                spriteBatch.DrawString(Game1.menuFont, "Toggle Sound: " + soundText, new Vector2(0, 150), colors[0]);
                spriteBatch.DrawString(Game1.menuFont, "Toggle Music: " + musicText, new Vector2(0, 190), colors[1]);
                spriteBatch.DrawString(Game1.menuFont, "Back", new Vector2(0, 230), colors[2]);

            }

            if (titleState == TitleState.Controls)
            {

                spriteBatch.DrawString(Game1.menuFont, "Back", new Vector2(0, 150), colors[0]);
                //TODO: ADD CONTROLS

            }

            if (titleState == TitleState.Credits)
            {

                spriteBatch.DrawString(Game1.menuFont, "Back", new Vector2(0, 150), colors[0]);
                //TODO: ADD CREDITS

            }


        }

        # endregion
    }
}
