using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace paranothing
{
    public enum GameState { MainMenu, Controls, Game, Dialogue, Pause, EndGame, LevelEditor, Credits }
    public enum Direction { Left, Right, Up, Down }
    public enum TimePeriod { FarPast, Past, Present };
    public struct DrawLayer {
        public static float ActionBubble = 0.01f;
        public static float Chairs = 0.015f;
        public static float Player = 0.02f;
        public static float Rubble = 0.03f;
        public static float Key = 0.035f;
        public static float Wardrobe = 0.04f;
        public static float Floor = 0.05f;
        public static float Stairs = 0.06f;
        public static float PlayerBehindStairs = 0.07f;
        public static float Background = 0.08f;
        public static float WallpaperTears = 0.09f;
        public static float Wallpaper = 0.10f;
    };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch spriteBatch2;

        # region Attributes
        Effect greyScale;

        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;
        Song bgMusic;

        Texture2D boyTex;
        SpriteSheet boySheet;

        Texture2D shadowTex;
        SpriteSheet shadowSheet;

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

        //Portrait
        Texture2D portraitTex;
        SpriteSheet portraitSheet;

        Texture2D rubbleTex;
        SpriteSheet rubbleSheet;

        Texture2D stairTex;
        SpriteSheet stairSheet;

        Texture2D doorTex;
        SpriteSheet doorSheet;

        Texture2D oldPortraitTex;
        SpriteSheet oldPortraitSheet;

        Texture2D keyTex;
        SpriteSheet keySheet;

        Texture2D chairTex;
        SpriteSheet chairSheet;

        Texture2D finalDoorTex;
        SpriteSheet finalDoorSheet;

        Texture2D buttonTex;
        SpriteSheet buttonSheet;

        Texture2D controller;

        GameController control = GameController.getInstance();
        private SpriteSheetManager sheetMan = SpriteSheetManager.getInstance();
        SoundManager soundMan = SoundManager.getInstance();
        
        int ScreenWidth = 1280;
        int ScreenHeight = 720;

        Boy player;
        ActionBubble actionBubble;

        //public static GameStorage gameStorage;

        //Wardrobe leftWR;
        //Wardrobe rightWR;

        //Portrait lowerPortrait;
        //Portrait upperPortrait;

        //Stairs stairs;
        //Floor f1;
        //Floor f2;

        //Wall leftWall;
        //Wall rightWallTop;
        //Wall rightWallBottom;
        //Wall exitWall;
        //Wall obstacleWall;

        //Fonts
        public static SpriteFont gameFont;
        public static SpriteFont titleFont;
        public static SpriteFont menuFont;
        //Title
        private GameTitle title;
        private Vector2 startPosition;
        //Description
        private GameBackground description;

        private Level tutorial, level1, level2, level3, level4;

        public bool gameInProgress = false;
        public static bool endGame = false;
        private float fadeOpacity;
        private float opacityPerSecond = 0.02f;
        private Stopwatch stopwatch;
        private Texture2D white;

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
            menuFont = Content.Load<SpriteFont>("MenuFont");
            title = new GameTitle(Content.Load<Texture2D>("screenshot_for_menu"), new Rectangle(0, 0, (int)(ScreenWidth), (int)(ScreenHeight)));
            title.setBottomTextRectangle(gameFont.MeasureString("Press Start"));
            startPosition = new Vector2(title.BottomTextRectangle.X, title.BottomTextRectangle.Y);
        }
        private void drawTitleText()
        {
            title.setTopTextRectangle(titleFont.MeasureString("Continuum"));
            drawText("Continuum", titleFont, Color.WhiteSmoke, title.TopTextRectangle.X, title.TopTextRectangle.Y);
            if (title.titleState == GameTitle.TitleState.Title)
                spriteBatch.DrawString(gameFont, "Press Start", startPosition, Color.White);
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
            control.state = paranothing.GameState.MainMenu;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteBatch2 = new SpriteBatch(GraphicsDevice);

            //Stuff for fade
            stopwatch = new Stopwatch();
            white = Content.Load<Texture2D>("white");

            audioEngine = new AudioEngine(@"Content/Sounds/sounds.xgs");
            waveBank = new WaveBank(audioEngine, @"Content/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content/Sounds/Sound Bank.xsb");
            soundMan.setSoundBank(ref soundBank);
            bgMusic = Content.Load<Song>("Sounds/Soundtrack");
            MediaPlayer.Play(bgMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;

            greyScale = Content.Load<Effect>("Greyscale");

            wallpaperTex = Content.Load<Texture2D>("Sprites/Wallpaper");
            wallpaperSheet = new SpriteSheet(wallpaperTex);
            wallpaperSheet.splitSheet(1, 2);

            wardrobeTex = Content.Load<Texture2D>("Sprites/wardrobe");
            wardrobeSheet = new SpriteSheet(wardrobeTex);
            wardrobeSheet.splitSheet(1, 5);
            wardrobeSheet.addAnimation("wardrobeclosed", new int[] { 0 });
            wardrobeSheet.addAnimation("wardrobeopening", new int[] { 1, 2, 3 });
            wardrobeSheet.addAnimation("wardrobeopen", new int[] { 4 });

            //Portrait
            portraitTex = Content.Load<Texture2D>("Sprites/portrait");
            portraitSheet = new SpriteSheet(portraitTex);
            portraitSheet.splitSheet(2, 1);

            rubbleTex = Content.Load<Texture2D>("Sprites/rubble");
            rubbleSheet = new SpriteSheet(rubbleTex);
            rubbleSheet.addSprite(0, 0, 37, 28);

            actionTex = Content.Load<Texture2D>("Sprites/actions");
            actionSheet = new SpriteSheet(actionTex);
            actionSheet.splitSheet(3, 3);
            actionSheet.addAnimation("bubble", new int[] { 0 });
            actionSheet.addAnimation("wardrobe", new int[] { 1 });
            actionSheet.addAnimation("push", new int[] { 2 });
            actionSheet.addAnimation("chair", new int[] { 3 });
            actionSheet.addAnimation("stair", new int[] { 4 });
            actionSheet.addAnimation("portrait", new int[] { 5 });
            actionSheet.addAnimation("oldportrait", new int[] { 6 });
            actionSheet.addAnimation("bookcase", new int[] { 7 });
            actionSheet.addAnimation("negate", new int[] { 8 });

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
            boySheet.addAnimation("controlstart", new int[] { 50, 51, 52 });
            boySheet.addAnimation("control", new int[] { 53 });
            boySheet.addAnimation("controlend", new int[] { 52, 51, 50 });
            boySheet.addAnimation("disappear", new int[] { 50, 51, 52, 53, 54, 55, 56, 57 });

            shadowTex = Content.Load<Texture2D>("Sprites/Shadow");
            shadowSheet = new SpriteSheet(shadowTex);
            shadowSheet.splitSheet(1, 4);
            shadowSheet.addAnimation("walk", new int[] { 0, 1, 2 });
            shadowSheet.addAnimation("stopwalk", new int[] { 2, 1, 0 });
            shadowSheet.addAnimation("stand", new int[] { 3 });

            floorTex = Content.Load<Texture2D>("Sprites/floor");
            floorSheet = new SpriteSheet(floorTex);
            floorSheet.splitSheet(2, 1);

            wallTex = Content.Load<Texture2D>("Sprites/wall");
            wallSheet = new SpriteSheet(wallTex);
            wallSheet.splitSheet(1, 2);

            stairTex = Content.Load<Texture2D>("Sprites/Staircase");
            stairSheet = new SpriteSheet(stairTex);
            stairSheet.splitSheet(1, 2);

            doorTex = Content.Load<Texture2D>("Sprites/door");
            doorSheet = new SpriteSheet(doorTex);
            doorSheet.splitSheet(2, 3);
            doorSheet.addAnimation("doorclosedpast", new int[] { 0 });
            doorSheet.addAnimation("dooropeningpast", new int[] { 1 });
            doorSheet.addAnimation("dooropenpast", new int[] { 2 });
            doorSheet.addAnimation("doorclosedpresent", new int[] { 3 });
            doorSheet.addAnimation("dooropeningpresent", new int[] { 4 });
            doorSheet.addAnimation("dooropenpresent", new int[] { 5 });

            //Old Portrait
            oldPortraitTex = Content.Load<Texture2D>("Sprites/PortraitWoman");
            oldPortraitSheet = new SpriteSheet(oldPortraitTex);
            oldPortraitSheet.splitSheet(2, 1);

            keyTex = Content.Load<Texture2D>("Sprites/Key");
            keySheet = new SpriteSheet(keyTex);
            keySheet.splitSheet(2, 1);

            chairTex = Content.Load<Texture2D>("Sprites/chair");
            chairSheet = new SpriteSheet(chairTex);
            chairSheet.splitSheet(1, 2);

            finalDoorTex = Content.Load<Texture2D>("Sprites/door_final");
            finalDoorSheet = new SpriteSheet(finalDoorTex);
            finalDoorSheet.splitSheet(1, 7);
            finalDoorSheet.addAnimation("bookcaseclosed", new int[] { 0 });
            finalDoorSheet.addAnimation("bookcaseopening", new int[] { 1, 2, 3, 4, 5 });
            finalDoorSheet.addAnimation("bookcaseclosing", new int[] { 5, 4, 3, 2, 1 });
            finalDoorSheet.addAnimation("bookcaseopen", new int[] { 6 });

            buttonTex = Content.Load<Texture2D>("Sprites/button");
            buttonSheet = new SpriteSheet(buttonTex);
            buttonSheet.splitSheet(1, 2);

            sheetMan.addSheet("wallpaper", wallpaperSheet);
            sheetMan.addSheet("wardrobe", wardrobeSheet);
            sheetMan.addSheet("portrait", portraitSheet);
            sheetMan.addSheet("rubble", rubbleSheet);
            sheetMan.addSheet("action", actionSheet);
            sheetMan.addSheet("boy", boySheet);
            sheetMan.addSheet("floor", floorSheet);
            sheetMan.addSheet("wall", wallSheet);
            sheetMan.addSheet("stair", stairSheet);
            sheetMan.addSheet("door", doorSheet);
            sheetMan.addSheet("oldportrait", oldPortraitSheet);
            sheetMan.addSheet("key", keySheet);
            sheetMan.addSheet("chair", chairSheet);
            sheetMan.addSheet("bookcase", finalDoorSheet);
            sheetMan.addSheet("button", buttonSheet);
            sheetMan.addSheet("shadow", shadowSheet);

            actionBubble = new ActionBubble();
            player = new Boy(254, 240, actionBubble);
            Camera camera = new Camera(0, 360, 1280, 720, 2.0f);
            tutorial = new Level("levels/tutorial.lvl");
            level1 = new Level("levels/level1.lvl");
            level2 = new Level("levels/level2.lvl");
            level3 = new Level("levels/level3.lvl");
            level4 = new Level("levels/level4.lvl");
            control.addLevel(tutorial);
            control.addLevel(level1);
            control.addLevel(level2);
            control.addLevel(level3);
            control.addLevel(level4);
            control.goToLevel("Tutorial");

            GameTitle.levelName = "Tutorial";

            control.setPlayer(player);
            control.setCamera(camera);
            control.initLevel(false);

            controller = Content.Load<Texture2D>("controller");

            // TODO: use this.Content to load your game content here
            loadTitleContents();
            description = new GameBackground(Content.Load<Texture2D>("GameThumbnail"), new Rectangle(0, 0, (int)(ScreenWidth), (int)(ScreenHeight)));
        }

        public void ResetGame()
        {
            gameInProgress = false;
            
            actionBubble = new ActionBubble();
            player = new Boy(254, 240, actionBubble);
            //control.goToLevel("Tutorial");

            //GameTitle.levelName = "Tutorial";

            Camera camera = new Camera(0, 360, 1280, 720, 2.0f);

            //control.level = l;
            control.setPlayer(player);
            control.setCamera(camera);
            control.initLevel(false);

            //loadTitleContents();
            description = new GameBackground(Content.Load<Texture2D>("GameThumbnail"), new Rectangle(0, 0, (int)(ScreenWidth), (int)(ScreenHeight)));

            fadeOpacity = 0;

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

            if ((Keyboard.GetState().IsKeyDown(Keys.Pause) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)) && control.state == GameState.Game)
            {       
                control.state = GameState.MainMenu;
                title.titleState = GameTitle.TitleState.Pause;
                title.menuSize = 4;
            }
            // TODO: Add your update logic here

            if (!endGame)
            {

                switch (control.state)
                {
                    case GameState.MainMenu:
                        endGame = false;
                        stopwatch.Reset();
                    
                        title.Update(this, Keyboard.GetState());
                        break;
                    case GameState.Game:

                        gameInProgress = true;

                        control.updateObjs(gameTime);
                        break;
                }
            }

            //FADE OUT UPDATE
            else
            {

                if (!(stopwatch.IsRunning))
                    stopwatch.Start();

                if (fadeOpacity < 1)
                    fadeOpacity = (stopwatch.ElapsedMilliseconds/100) * opacityPerSecond;

                if (fadeOpacity == 1 && stopwatch.ElapsedMilliseconds >= 15000)
                {
                    endGame = false;
                    stopwatch.Reset();
                    control.state = GameState.MainMenu;
                    title.titleState = GameTitle.TitleState.Menu;
                    title.menuSize = 5;
                    gameInProgress = false;
                }

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20,20,20));

            switch (control.state)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    title.Draw(spriteBatch);
                    drawTitleText();

                    if (title.titleState == GameTitle.TitleState.Controls)
                        spriteBatch.Draw(controller, new Rectangle(200, 180, 500, 500), Color.White);

                    //spriteBatch.End();
                    break;
                //case GameState.Description:
                //    spriteBatch.Begin();
                //    description.Draw(spriteBatch);
                //    drawDescriptionText();
                //    spriteBatch.End();
                //    break;
                case GameState.Game:
                    Effect pastEffect = null;
                    if (control.timePeriod != TimePeriod.Present)
                        pastEffect = greyScale;
                    Matrix transform = Matrix.Identity;
                    transform *= Matrix.CreateTranslation(-control.camera.X, -control.camera.Y, 0);
                    transform *= Matrix.CreateScale(control.camera.scale);
                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointWrap, null, null, pastEffect, transform);
                    drawWallpaper(spriteBatch, wallpaperSheet);
                    control.drawObjs(spriteBatch);

                    //spriteBatch.End();
                    break;
            }

            spriteBatch.End();

            spriteBatch2.Begin();


            if (endGame)
            {
                spriteBatch2.Draw(white, new Vector2(0, 0), null, Color.White * fadeOpacity, 0f, Vector2.Zero, new Vector2(ScreenWidth, ScreenHeight), SpriteEffects.None, 0f);

                if (fadeOpacity >= 1)
                    spriteBatch2.DrawString(Game1.menuFont, "Bruce... you're safe now.", new Vector2(280, 300), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            }

            spriteBatch2.End();


            base.Draw(gameTime);
        }
        protected void drawWallpaper(SpriteBatch spriteBatch, SpriteSheet wallpaper)
        {

            Effect paperEffect = null;
            if (control.timePeriod == TimePeriod.Past)
                paperEffect = greyScale;
            Matrix transform = Matrix.CreateScale(control.camera.scale);
            transform *= Matrix.CreateTranslation(-control.camera.X * control.camera.scale, -control.camera.Y * control.camera.scale, 0);

            Rectangle paperBounds = wallpaper.getSprite(0);
            Rectangle dest = new Rectangle(0,0, ScreenWidth/2, ScreenHeight/2);
            Color paperColor = control.level.wallpaperColor;
            if (control.timePeriod == TimePeriod.Past)
                paperColor.A = 16;
            int startX = -paperBounds.Width;
            int xCount = control.level.Width / paperBounds.Height + 2;
            int startY = (int)(Math.Floor((float)-control.camera.Y / paperBounds.Height)) * paperBounds.Height;
            int yCount = control.level.Height / paperBounds.Height + 1;
               // float minZ = (float)Math.Floor(ball.Z / 10) * 10.0f - 10;
            for (int drawX = 0; drawX < xCount; drawX++)
            {
                for (int drawY = 0; drawY < yCount; drawY++)
                {
                    Rectangle drawRect = new Rectangle(drawX * paperBounds.Width + startX, drawY * paperBounds.Height + startY, paperBounds.Width, paperBounds.Height);
                    Rectangle srcRect = new Rectangle(paperBounds.X, paperBounds.Y, paperBounds.Width, paperBounds.Height);
                    if ((drawY + 1) * paperBounds.Height + startY > control.level.Height)
                    {
                        drawRect.Height = control.level.Height - (drawY * paperBounds.Height + startY);
                        srcRect.Height = drawRect.Height;
                    }
                    spriteBatch.Draw(wallpaper.image, drawRect, srcRect, paperColor, 0f, new Vector2(),SpriteEffects.None, DrawLayer.Wallpaper);
                }
            }
            if (control.timePeriod == TimePeriod.Present)
            {
                paperBounds = wallpaper.getSprite(1);
                dest = new Rectangle(0, 0, ScreenWidth / 2, ScreenHeight / 2);
                for (int drawX = 0; drawX < xCount; drawX++)
                {
                    for (int drawY = 0; drawY < yCount; drawY++)
                    {
                        Rectangle drawRect = new Rectangle(drawX * paperBounds.Width + startX, drawY * paperBounds.Height + startY, paperBounds.Width, paperBounds.Height);
                        Rectangle srcRect = new Rectangle(paperBounds.X, paperBounds.Y, paperBounds.Width, paperBounds.Height);
                        if ((drawY + 1) * paperBounds.Height + startY > control.level.Height)
                        {
                            drawRect.Height = control.level.Height - (drawY * paperBounds.Height + startY);
                            srcRect.Height = drawRect.Height;
                        }
                        spriteBatch.Draw(wallpaper.image, drawRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, DrawLayer.WallpaperTears);
                    }
                }
            }
        }
    }
}
