using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace paranothing
{
    public enum GameState { Title, Description, Game }
    public enum Direction { Left, Right, Up, Down }
    public enum TimePeriod { FarPast, Past, Present };
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        # region Attributes

        Effect greyScale;

        Texture2D boyTex;
        SpriteSheet boySheet;

        Texture2D actionTex;
        SpriteSheet actionSheet;

        Texture2D floorTex;
        SpriteSheet floorSheet;

        Texture2D wallTex;
        SpriteSheet wallSheet;

        Texture2D wallpaperTex;
        SpriteSheet wallpaperSheet;

        Texture2D wardrobeTex;
        SpriteSheet wardrobeSheet;

        Texture2D portraitTex;
        SpriteSheet portraitSheet;

        Texture2D stairTex;
        SpriteSheet stairSheet;

        GameController control;
        float scale = 2.0f;
        int ScreenWidth = 640;
        int ScreenHeight = 360;

        Boy player;
        ActionBubble actionBubble;

        Wardrobe leftWR;
        Wardrobe rightWR;

        Portrait lowerPortrait;
        Portrait upperPortrait;

        Stairs stairs;
        Floor f1;
        Floor f2;

        Wall leftWall;
        Wall rightWallTop;
        Wall rightWallBottom;
        Wall exitWall;
        Wall obstacleWall;

        //Fonts
        private SpriteFont gameFont;
        private SpriteFont titleFont;
        //Title
        private GameTitle title;
        private Vector2 startPosition;
        //Description
        private GameBackground description;


        # endregion

        # region Methods

        public GameState GameState
        {
            get
            {
                return control.state;
            }
            set
            {
                control.state = value;
            }
        }

        /// <summary>
        /// Draws text on the screen
        /// </summary>
        /// <param name="text">text to write</param>
        /// <param name="textColor">color of text</param>
        /// <param name="x">left hand edge of text</param>
        /// <param name="y">top of text</param>
        private void drawText(string text, SpriteFont font, Color textColor, float x, float y)
        {
            int layer;
            Vector2 vectorText = new Vector2(x, y);

            //solid
            Color backColor = new Color(190, 190, 190);
            for (layer = 0; layer < 3; layer++)
            {
                spriteBatch.DrawString(font, text, vectorText, backColor);
                vectorText.X++;
                vectorText.Y++;
            }

            //top of character
            spriteBatch.DrawString(font, text, vectorText, textColor);
        }

        //Title
        private void loadTitleContents()
        {
            titleFont = Content.Load<SpriteFont>("TitleFont");
            gameFont = Content.Load<SpriteFont>("GameFont");
            title = new GameTitle(Content.Load<Texture2D>("screenshot"), new Rectangle(0, 0, (int)(ScreenWidth * scale), (int)(ScreenHeight * scale)));
            title.setBottomTextRectangle(gameFont.MeasureString("Press 'Enter' to start"));
            startPosition = new Vector2(title.BottomTextRectangle.X, title.BottomTextRectangle.Y);
        }
        private void drawTitleText()
        {
            title.setTopTextRectangle(titleFont.MeasureString("Welcome to Paranothing"));
            drawText("Welcome to Paranothing", titleFont, Color.WhiteSmoke, title.TopTextRectangle.X, title.TopTextRectangle.Y);
            spriteBatch.DrawString(gameFont, "Press 'Enter' to start", startPosition, Color.DarkMagenta);
        }

        //Description
        private void drawDescriptionText()
        {
            spriteBatch.DrawString(gameFont, "Press 'Space Bar' to continue", startPosition, Color.Black);
        }

        # endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            greyScale = Content.Load<Effect>("Greyscale");

            wallpaperTex = Content.Load<Texture2D>("Sprites/Wallpaper");
            wallpaperSheet = new SpriteSheet(wallpaperTex);
            wallpaperSheet.addSprite(0, 0, wallpaperTex.Width / 2, wallpaperTex.Height);
            wallpaperSheet.addSprite(wallpaperTex.Width / 2, 0, wallpaperTex.Width / 2, wallpaperTex.Height);

            wardrobeTex = Content.Load<Texture2D>("Sprites/wardrobe");
            wardrobeSheet = new SpriteSheet(wardrobeTex);
            wardrobeSheet.splitSheet(1, 5);
            wardrobeSheet.addAnimation("wardrobeclosed", new int[] { 0 });
            wardrobeSheet.addAnimation("wardrobeopening", new int[] { 1, 2, 3 });
            wardrobeSheet.addAnimation("wardrobeopen", new int[] { 4 });

            portraitTex = Content.Load<Texture2D>("Sprites/portrait");
            portraitSheet = new SpriteSheet(portraitTex);
            portraitSheet.addSprite(0, 0, 35, 30);

            actionTex = Content.Load<Texture2D>("Sprites/actions");
            actionSheet = new SpriteSheet(actionTex);
            actionSheet.splitSheet(2, 3);
            actionSheet.addAnimation("bubble", new int[] { 0 });
            actionSheet.addAnimation("wardrobe", new int[] { 1 });
            actionSheet.addAnimation("push", new int[] { 2 });
            actionSheet.addAnimation("portrait", new int[] { 3 });
            actionSheet.addAnimation("negate", new int[] { 4 });

            boyTex = Content.Load<Texture2D>("Sprites/BruceSheet");
            boySheet = new SpriteSheet(boyTex);
            boySheet.splitSheet(7, 9, 0, 0, 58);
            boySheet.addAnimation("walk", new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            boySheet.addAnimation("stand", new int[] { 8 });
            boySheet.addAnimation("leavewardrobe", new int[] { 9, 10, 11, 12, 13, 14, 15, 16 });
            boySheet.addAnimation("enterwardrobe", new int[] { 18, 19, 20, 21, 22, 23, 24, 25 });
            boySheet.addAnimation("enterportrait", new int[] { 27, 28, 29, 30, 31, 32, 33, 34 });
            boySheet.addAnimation("leaveportrait", new int[] { 34, 33, 32, 31, 30, 29, 28, 27 });
            boySheet.addAnimation("startpush", new int[] { 36, 37, 38, 39 });
            boySheet.addAnimation("endpush", new int[] { 39, 38, 37, 36 });
            boySheet.addAnimation("push", new int[] { 41, 42, 43, 44, 45, 46, 47, 48 });
            boySheet.addAnimation("pushstill", new int[] { 49 });
            boySheet.addAnimation("disappear", new int[] { 50, 51, 52, 53, 54, 55, 56, 57 });

            actionBubble = new ActionBubble(actionSheet);
            player = new Boy(254, 240, actionBubble, boySheet);

            leftWR = new Wardrobe(12, 126, wardrobeSheet);
            rightWR = new Wardrobe(210, 126, wardrobeSheet);
            leftWR.setLinkedWR(rightWR);
            rightWR.setLinkedWR(leftWR);

            floorTex = Content.Load<Texture2D>("Sprites/floor");
            floorSheet = new SpriteSheet(floorTex);
            floorSheet.addSprite(0, 0, floorTex.Width, floorTex.Height);

            f1 = new Floor(0, 208, 400, 8, floorSheet);
            f2 = new Floor(0, 298, 400, 8, floorSheet);

            wallTex = Content.Load<Texture2D>("Sprites/wall");
            wallSheet = new SpriteSheet(wallTex);
            wallSheet.addSprite(0, 0, 16, 32);

            leftWall = new Wall(-8, 0, 16, 300, wallSheet);
            rightWallTop = new Wall(392, 0, 16, 126, wallSheet);
            rightWallBottom = new Wall(392, 216, 16, 84, wallSheet);
            exitWall = new Wall(392, 126, 16, 84, wallSheet, false);
            obstacleWall = new Wall(170, 0, 16, 208, wallSheet);

            lowerPortrait = new Portrait(256, 233, portraitSheet);
            upperPortrait = new Portrait(310, 143, portraitSheet);

            stairTex = Content.Load<Texture2D>("Sprites/Staircase");
            stairSheet = new SpriteSheet(stairTex);
            stairSheet.addSprite(0, 0, 146, 112);
            stairSheet.addSprite(146, 0, 146, 112);

            stairs = new Stairs(100, 186, Direction.Left, stairSheet, false);

            control = GameController.getInstance();
            control.setPlayer(player);
            control.addObject(actionBubble);
            control.addObject(lowerPortrait);
            control.addObject(upperPortrait);
            control.addObject(leftWR);
            control.addObject(rightWR);
            control.addObject(f1);
            control.addObject(f2);
            control.addObject(leftWall);
            control.addObject(rightWallTop);
            control.addObject(exitWall);
            control.addObject(rightWallBottom);
            control.addObject(obstacleWall);
            control.addObject(stairs);
            
            // TODO: use this.Content to load your game content here
            loadTitleContents();
            description = new GameBackground(Content.Load<Texture2D>("GameThumbnail"), new Rectangle(0, 0, ScreenWidth, ScreenHeight));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            graphics.PreferredBackBufferWidth = (int)(ScreenWidth * scale);
            graphics.PreferredBackBufferHeight = (int)(ScreenHeight * scale);
            graphics.ApplyChanges();

            // TODO: Add your update logic here
            switch (control.state)
            {
                case GameState.Title:
                    title.Update(this, Keyboard.GetState());
                    break;
                case GameState.Description:
                    description.Update(this, Keyboard.GetState());
                    break;
                case GameState.Game:
                    control.updateObjs(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (control.state)
            {
                case GameState.Title:
                    spriteBatch.Begin();
                    title.Draw(spriteBatch);
                    drawTitleText();
                    spriteBatch.End();
                    break;
                case GameState.Description:
                    spriteBatch.Begin();
                    description.Draw(spriteBatch);
                    drawDescriptionText();
                    spriteBatch.End();
                    break;
                case GameState.Game:
                    drawWallpaper(spriteBatch, wallpaperSheet);
                    Effect pastEffect = null;
                    if (control.timePeriod == TimePeriod.Past)
                        pastEffect = greyScale;
                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, pastEffect, Matrix.CreateScale(scale));
                    control.drawObjs(spriteBatch);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
        protected void drawWallpaper(SpriteBatch spriteBatch, SpriteSheet wallpaper)
        {

            Effect paperEffect = null;
            if (control.timePeriod == TimePeriod.Past)
                paperEffect = greyScale;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, paperEffect, Matrix.CreateScale(scale));

            Rectangle paperBounds = wallpaper.getSprite(0);
            Rectangle dest = new Rectangle(0,0, ScreenWidth/2, ScreenHeight/2);
            Color paperColor = Color.White;
            if (control.timePeriod == TimePeriod.Past)
                paperColor.A = 16;
            for (int drawX = 0; drawX < ScreenWidth; drawX += paperBounds.Width)
            {
                for (int drawY = 0; drawY < ScreenHeight; drawY += paperBounds.Height)
                {
                    spriteBatch.Draw(wallpaper.image, new Vector2(drawX, drawY), paperBounds, paperColor);
                }
            }
            if (control.timePeriod == TimePeriod.Present)
            {
                paperBounds = wallpaper.getSprite(1);
                dest = new Rectangle(0, 0, ScreenWidth / 2, ScreenHeight / 2);
                for (int drawX = 0; drawX < ScreenWidth; drawX += paperBounds.Width)
                {
                    for (int drawY = 0; drawY < ScreenHeight; drawY += paperBounds.Height)
                    {
                        spriteBatch.Draw(wallpaper.image, new Vector2(drawX, drawY), paperBounds, Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
