using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shmup
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Shmup : Game
    {
        GraphicsDeviceManager graphics;
        Background background;
        SpriteFont font;
        SpriteFont fontMenu;
        SpriteFont fontTitle;
        SpriteFont fontTitle2;
        SpriteBatch spriteBatch;
        PlayerShip playerShip;
        PlayerShipBullet playerShipBullet;
        KeyboardState state = Keyboard.GetState();
        Level level;

        GreenBullet gBullet;
        BlueBullet bBullet;
        RedBullet rBullet;
        YellowBullet yBullet;

        GreenEnemy gEnemy;
        BlueEnemy bEnemy;
        RedEnemy rEnemy;
        YellowEnemy yEnemy;

        ParticleManager particle;
        List<Enemies> enemies;
        MinionSpawner minionSpawner;

        Dictionary<string, Animation> animation; //Dictionnaire contenant les différentes animations qui seront utilisés

        Song songGame;
        Song songMenu;

        Vector2 positionCredits;
        Vector2 positionScore = new Vector2(500, 500);

        static int width;
        static int height;
        static bool isInside = true;
        bool isPlaying = false;
        static int score = 0;
        static int highScore = 0;
        static int multiplicateur = 1;
        static int timeResetMultiplicateur = 0;

        //pour le menu
        MouseState mouseState, previousMouseState;
        KeyboardState ks;
        Button playButton, scoreButton, creditsButton;
        const int menu = 0, play = 1, credits = 2, hscore = 3;
        static int currentScreen = menu;


        public static int Multiplicateur { get => multiplicateur; set => multiplicateur = value; }
        public static int TimeResetMultiplicateur { get => timeResetMultiplicateur; set => timeResetMultiplicateur = value; }
        public static int CurrentScreen { get => currentScreen; set => currentScreen = value; }
        public static int Width { get => width; set => width = value; }
        public static int Height { get => height; set => height = value; }
        public static int HighScore { get => highScore; set => highScore = value; }
        public static int Score { get => score; set => score = value; }
        public static bool IsInside { get => isInside; set => isInside = value; }

        public Shmup()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            width = graphics.PreferredBackBufferWidth;
            height = graphics.PreferredBackBufferHeight;
            IsMouseVisible = true;
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
            playerShip = new PlayerShip();
            playerShipBullet = new PlayerShipBullet();

            enemies = new List<Enemies>();
            gEnemy = new GreenEnemy(137, 139, Enemies.Sprites.Green);
            yEnemy = new YellowEnemy(94, 95, Enemies.Sprites.Yellow);
            rEnemy = new RedEnemy(105, 99, Enemies.Sprites.Red);
            bEnemy = new BlueEnemy(104, 100, Enemies.Sprites.Blue);

            minionSpawner = new MinionSpawner();

            enemies.Add(rEnemy);
            enemies.Add(gEnemy);
            enemies.Add(yEnemy);
            enemies.Add(bEnemy);
            playerShipBullet = new PlayerShipBullet();
            gBullet = new GreenBullet(14, 14);
            bBullet = new BlueBullet(10, 39);
            yBullet = new YellowBullet(16, 17);
            rBullet = new RedBullet(10, 17);
            particle = new ParticleManager(100, 1, 300 * 15);

            level = new Level(playerShip, rEnemy, bEnemy, gEnemy, yEnemy);

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
            // TODO: use this.Content to load your game content here

            level.Background = Content.Load<Texture2D>("bg");
            level.Bordure = Content.Load<Texture2D>("line");

            background = new Background(graphics, Content.Load<Texture2D>("bg"));

            playerShip.Texture = Content.Load<Texture2D>("ship");

            playerShipBullet.Texture = Content.Load<Texture2D>("ship_bullets");
            playerShipBullet.SoundEffect = Content.Load<SoundEffect>("laserShip");

            rEnemy.Texture = Content.Load<Texture2D>("redEnemy");
            rEnemy.InitialisationHitbox();

            gEnemy.Texture = Content.Load<Texture2D>("greenEnemy");
            gEnemy.InitialisationHitbox();

            yEnemy.Texture = Content.Load<Texture2D>("yellowEnemy");
            yEnemy.InitialisationHitbox();

            bEnemy.Texture = Content.Load<Texture2D>("blueEnemy");
            bEnemy.InitialisationHitbox();

            gBullet.Texture = Content.Load<Texture2D>("ennemies_bullets");
            bBullet.Texture = Content.Load<Texture2D>("ennemies_bullets");
            rBullet.Texture = Content.Load<Texture2D>("ennemies_bullets");
            yBullet.Texture = Content.Load<Texture2D>("ennemies_bullets");

            animation = new Dictionary<String, Animation>()
            {
                {"blue", new Animation(Content.Load<Texture2D>("blueEnemy"), 104, 100, 37, 25, 1.0f, false) },
                {"green", new Animation(Content.Load<Texture2D>("greenEnemy"), 137, 139, 16, 10, 1.0f, false) },
                {"red", new Animation(Content.Load<Texture2D>("redEnemy"), 106, 100, 12, 10, 1.0f, false) },
                {"yellow", new Animation(Content.Load<Texture2D>("yellowEnemy"), 94, 95, 13, 10, 1.0f, false) }
            };

            particle.Particle.Texture = Content.Load<Texture2D>("Laser");
            font = Content.Load<SpriteFont>("score");
            fontMenu = Content.Load<SpriteFont>("MenuFont");
            fontTitle = Content.Load<SpriteFont>("FontTitle");
            fontTitle2 = Content.Load<SpriteFont>("FontTitle2");

            songGame = Content.Load<Song>("song");
            songMenu = Content.Load<Song>("Menu");


            playerShipBullet.SoundEffect = Content.Load<SoundEffect>("laserShip");
            playerShip.SoundEffectExplosion = Content.Load<SoundEffect>("explosion-04");

            rEnemy.SoundEffectExplosion = Content.Load<SoundEffect>("explosion-02");
            rBullet.SoundEffect = Content.Load<SoundEffect>("redLaser");

            gBullet.SoundEffect = Content.Load<SoundEffect>("greenLaser");

            gEnemy.SoundEffectExplosion = Content.Load<SoundEffect>("explosion-02");

            yEnemy.SoundEffectExplosion = Content.Load<SoundEffect>("explosion-02");
            yBullet.SoundEffect = Content.Load<SoundEffect>("yellowLaser");

            bBullet.SoundEffect = Content.Load<SoundEffect>("blueLaser");
            bBullet.SoundEffectSpecial = Content.Load<SoundEffect>("beam");

            bEnemy.SoundEffectExplosion = Content.Load<SoundEffect>("explosion-02");

            BlueBullet bullet = new BlueBullet(13, Level.Height)
            {
                Texture = Content.Load<Texture2D>("bluebullet_special"),
                SoundEffectSpecial = Content.Load<SoundEffect>("beam")
            };
            bEnemy.InitialisationLaser(bullet);


            minionSpawner.Texture = Content.Load<Texture2D>("minion");
            minionSpawner.SoundEffectExplosion = Content.Load<SoundEffect>("explosion-05");

            //pour le menu
            playButton = new Button(new Vector2((width / 2) - (width / 30), (height / 2) - fontMenu.MeasureString("Score").Y - 50), false);
            playButton.Load("Play", fontMenu);
            scoreButton = new Button(new Vector2((width / 2) - (width / 30), playButton.Position.Y + fontMenu.MeasureString("Play").Y + 30), false);
            scoreButton.Load("Score", fontMenu);
            creditsButton = new Button(new Vector2((width / 2) - (width / 30), scoreButton.Position.Y + fontMenu.MeasureString("Credits").Y + 30), false);
            creditsButton.Load("Credits", fontMenu);
            positionCredits = new Vector2(width / 2 - fontMenu.MeasureString("Un jeu realise par :").X / 2, height);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            //pour le menu
            mouseState = Mouse.GetState();
            ks = Keyboard.GetState();
            switch (currentScreen)
            {
                case menu:
                    if (!isPlaying)
                    {
                        MediaPlayer.Volume = 0.3f;
                        MediaPlayer.Play(songMenu);
                        MediaPlayer.IsRepeating = true;
                        isPlaying = true;
                    }
                    UpdateMenu(gameTime);

                    break;
                case play:
                    if (!isPlaying)
                    {
                        MediaPlayer.Volume = 0.3f;
                        MediaPlayer.Play(songGame);
                        MediaPlayer.IsRepeating = true;
                        isPlaying = true;
                    }
                    Pause.WantToPause();
                    if (!Pause.HasPaused)
                    {
                        UpdateGamePlay(gameTime);
                        MediaPlayer.Resume();
                    }
                    else
                        MediaPlayer.Pause();
                    break;
                case credits:
                    background.Update(gameTime.ElapsedGameTime.TotalSeconds * 20);
                    positionCredits.Y -= 1f;
                    RetournerMenu(gameTime);
                    if (positionCredits.Y == -400)
                        currentScreen = menu;
                    break;
                case hscore:
                    background.Update(gameTime.ElapsedGameTime.TotalSeconds * 20);
                    RetournerMenu(gameTime);
                    break;
            }

        }

        /// <summary>
        /// Retourne sur le menu après un clic gauche
        /// </summary>
        /// <param name="gameTime"></param>

        public void RetournerMenu(GameTime gameTime)
        {
            if (mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                currentScreen = menu;

            previousMouseState = mouseState;
        }

        /// <summary>
        /// Update le menu si l'on clique sur un des boutons
        /// </summary>
        /// <param name="gameTime"></param>

        protected void UpdateMenu(GameTime gameTime)
        {
            background.Update(gameTime.ElapsedGameTime.TotalSeconds * 20);
            switch (currentScreen)
            {
                case menu:
                    //pour aller dans le jeux
                    if (playButton.Update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        currentScreen = play;
                        isPlaying = false;
                    }

                    //pour aller dans les scores
                    if (scoreButton.Update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                        currentScreen = hscore;

                    if (creditsButton.Update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                        currentScreen = credits;
                    break;

                case hscore:
                    if (ks.IsKeyDown(Keys.A))
                        currentScreen = menu;
                    break;

                case play:
                    if (ks.IsKeyDown(Keys.A))
                        currentScreen = menu;
                    break;

            }
            previousMouseState = mouseState;
        }

        /// <summary>
        /// Update lorsque l'on est en jeu
        /// </summary>
        /// <param name="gameTime"></param>

        public void UpdateGamePlay(GameTime gameTime)
        {

            level.Update(gameTime, playerShip);

            playerShip.Update(gameTime, playerShipBullet, particle, level);


            gEnemy.Update(playerShip, animation["green"], gameTime, gBullet, particle);
            yEnemy.Update(playerShip, animation["yellow"], yBullet, gameTime, particle);
            rEnemy.Update(playerShip, animation["red"], gameTime, rBullet, particle);
            bEnemy.Update(playerShip, animation["blue"], bBullet, gameTime, particle);

            minionSpawner.Update(gameTime, playerShip, gEnemy, yEnemy, rEnemy, bEnemy);

            Camera.Update(playerShip);
            particle.DeleteParticles();


            if (GraphicsDevice.Viewport.Bounds.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                isInside = true;
            else
                isInside = false;


            if (!playerShip.IsAlive)
            {
                MediaPlayer.Stop();
                if (playerShip.TimeRespawn >= 3)
                {
                    CurrentScreen = menu;
                    playerShip.TimeRespawn = 0;
                    score = 0;
                    isPlaying = false;
                    level.ResetLevel(playerShip, rEnemy, bEnemy, gEnemy, yEnemy, minionSpawner, particle);
                }
            }

            if (!gEnemy.IsAlive && !yEnemy.IsAlive && !bEnemy.IsAlive && !rEnemy.IsAlive)
            {
                playerShip.TimeRespawn += gameTime.ElapsedGameTime.TotalSeconds;
                if (playerShip.TimeRespawn >= 3)
                {
                    level.ResetLevel(playerShip, rEnemy, bEnemy, gEnemy, yEnemy, minionSpawner, particle);
                    playerShip.TimeRespawn = 0;
                }
            }

            if (multiplicateur > 1)
            {
                timeResetMultiplicateur++;
                if (timeResetMultiplicateur == 60 * 5)
                {
                    multiplicateur = 1;
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
            switch (currentScreen)
            {
                case menu:
                    DrawMenu();
                    break;
                case play:
                    DrawGamePlay(gameTime);
                    break;
                case credits:
                    DrawCredits();
                    break;
                case hscore:
                    DrawScores();
                    break;

            }
            
            base.Draw(gameTime);
        }

        /// <summary>
        /// Affichage du menu
        /// </summary>

        public void DrawMenu()
        {
            spriteBatch.Begin();
            background.Draw(spriteBatch);
            spriteBatch.DrawString(fontTitle, "COLOR SWITCH", new Vector2(width / 2 - 1.5f * fontTitle.MeasureString("COLOR SWITCH").X + 350, 100), Color.White);
            spriteBatch.DrawString(fontTitle2, "Shmup Edition", new Vector2(width / 2 - fontTitle2.MeasureString("Shmup Edition").X + 120, 160), Color.White);
            playButton.Draw(spriteBatch);
            scoreButton.Draw(spriteBatch);
            creditsButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Affichage du gameplay
        /// </summary>
        /// <param name="gameTime"></param>

        public void DrawGamePlay(GameTime gameTime)
        {           

            spriteBatch.Begin(transformMatrix: Camera.World);

            GraphicsDevice.Clear(Color.Black);


            level.Draw(spriteBatch);            

            gEnemy.Draw(spriteBatch, animation["green"], particle);
            rEnemy.Draw(spriteBatch, animation["red"], particle);
            yEnemy.Draw(spriteBatch, animation["yellow"], particle);
            bEnemy.Draw(spriteBatch, animation["blue"], particle);
            minionSpawner.Draw(spriteBatch);
            playerShip.Draw(spriteBatch, particle);

            spriteBatch.End();


           

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score : " + score, new Vector2(1000, 50), Color.White);
            spriteBatch.DrawString(font, "Multiplicateur : x" + multiplicateur, new Vector2(1000, 80), Color.White);

            if (!gEnemy.IsAlive && !yEnemy.IsAlive && !bEnemy.IsAlive && !rEnemy.IsAlive)
            {
                if (playerShip.TimeRespawn < 3)
                {
                    spriteBatch.DrawString(font, "CONGRATULATIONS ! ", new Vector2(width / 2 - font.MeasureString("CONGRATULATIONS").X / 2, height / 2), Color.White);
                    spriteBatch.DrawString(font, "Now, starting a new game... ", new Vector2(width / 2 - font.MeasureString("Now, starting a new game... ").X / 2, height / 2 + 50), Color.White);
                }                   
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Affichage des crédits
        /// </summary>

        public void DrawCredits()
        {
            spriteBatch.Begin();
            background.Draw(spriteBatch);
            spriteBatch.DrawString(fontMenu, "Un jeu realise par :", positionCredits, Color.White);
            spriteBatch.DrawString(fontMenu, "Ahmonkou Zabsonre", positionCredits + new Vector2(0, 100), Color.White);
            spriteBatch.DrawString(fontMenu, "Maxime Bardiot", positionCredits + new Vector2(0, 200), Color.White);
            spriteBatch.DrawString(fontMenu, "Samir Belfaquir", positionCredits + new Vector2(0, 300), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Affichage du score
        /// </summary>

        public void DrawScores()
        {
            String path = @"../scores.txt";

                spriteBatch.Begin();
                background.Draw(spriteBatch);
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        String line = sr.ReadToEnd();
                        spriteBatch.DrawString(fontMenu, "HIGHSCORES :", positionScore - new Vector2(0, 190), Color.White);
                        spriteBatch.DrawString(fontMenu, line, positionScore - new Vector2(0, 100), Color.White);
                    }
                }
                else
                {
                    spriteBatch.DrawString(fontMenu, "Vous n'avez pas encore lance une seule partie.", new Vector2(width / 2 - fontMenu.MeasureString("Vous n'avez pas encore lance une seule partie.").X / 2, height / 2 - 50), Color.White);
                    spriteBatch.DrawString(fontMenu, "N'hesitez pas a jouer !", new Vector2(width / 2 - font.MeasureString("N'hesitez pas a jouer !").X / 2, height / 2 + 20), Color.White);
                }
            spriteBatch.End();
        }
    }
}
