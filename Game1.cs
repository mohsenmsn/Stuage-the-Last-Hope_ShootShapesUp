using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace ShootShapesUp
{
    public class Game1 : Game
    {
        enum GameState
        {
            MainMenu,
            Briefing,
            Playing,
            Paused,
            GameOver,
            Victory
        }

        sealed class LevelDef
        {
            public string Title;
            public string Blurb;
            public float Duration;
            public Texture2D Background;
            public Texture2D BriefingArt;
            public SpawnProfile Spawn;
        }

        GameState CurState;
        int levelIndex;
        int menuIndex;
        LevelDef[] levels;

        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public static Song Music { get; private set; }
        private static readonly Random rand = new Random();
        private static SoundEffect[] explosions;
        private static SoundEffect[] shots;
        private static int shotPlays;
        private static int explosionPlays;

        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle vpRect;
        Vector2 pointerOrigin;
        Texture2D pixel;
        float shakeRemaining;
        Vector2 shakeOffset;
        float hitFlash;
        bool musicStarted;

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;
        }

        protected override void Initialize()
        {
            base.Initialize();
            vpRect = GraphicsDevice.Viewport.Bounds;
            EntityManager.Add(PlayerShip.Instance);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);

            pointerOrigin = new Vector2(Art.Pointer.Width / 2f, Art.Pointer.Height / 2f);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            Music = Content.Load<Song>("Sound/Music");
            explosions = LoadSoundRange("Sound/explosion-0", 8);
            shots = LoadSoundRange("Sound/shoot-0", 4);

            BuildLevels();
            StartMusic();
        }

        SoundEffect[] LoadSoundRange(string prefix, int count)
        {
            var loaded = new SoundEffect[count];
            for (int i = 0; i < count; i++)
                loaded[i] = Content.Load<SoundEffect>(prefix + (i + 1));
            return loaded;
        }

        void BuildLevels()
        {
            levels = new[]
            {
                new LevelDef
                {
                    Title = "Level 1: The Initial Assault",
                    Blurb = "Seek and Wanderer buzzards test the line. Survive 20 seconds.",
                    Duration = GameConfig.Level1Duration,
                    Background = Art.BGalaxy,
                    BriefingArt = Art.BGalaxy11,
                    Spawn = new SpawnProfile
                    {
                        InverseStart = 75f,
                        InverseMin = 28f,
                        Ramp = 0.015f,
                        SeekRolls = 1,
                        WandererRolls = 1,
                        EliteRolls = 0,
                        SeekAccel = 0.55f,
                        EliteChanceScale = 2.5f
                    }
                },
                new LevelDef
                {
                    Title = "Level 2: The War Escalates",
                    Blurb = "Faster seekers, denser wanderers. Hold 30 seconds. Elites are still gathering.",
                    Duration = GameConfig.Level2Duration,
                    Background = Art.BGalaxy21,
                    BriefingArt = Art.BGalaxy12,
                    Spawn = new SpawnProfile
                    {
                        InverseStart = 52f,
                        InverseMin = 16f,
                        Ramp = 0.03f,
                        SeekRolls = 1,
                        WandererRolls = 2,
                        EliteRolls = 0,
                        SeekAccel = 0.85f,
                        EliteChanceScale = 2.5f
                    }
                },
                new LevelDef
                {
                    Title = "Level 3: The Final Stand",
                    Blurb = "Elite buzzards join the swarm. Survive 40 seconds and save the sausage planet.",
                    Duration = GameConfig.Level3Duration,
                    Background = Art.BGalaxy1,
                    BriefingArt = Art.BGalaxy11,
                    Spawn = new SpawnProfile
                    {
                        InverseStart = 40f,
                        InverseMin = 10f,
                        Ramp = 0.04f,
                        SeekRolls = 1,
                        WandererRolls = 1,
                        EliteRolls = 1,
                        SeekAccel = 1.0f,
                        EliteChanceScale = 2.4f
                    }
                }
            };
        }

        void StartMusic()
        {
            if (musicStarted || Music == null)
                return;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.45f;
            MediaPlayer.Play(Music);
            musicStarted = true;
        }

        public static void PlayShot()
        {
            if (shotPlays >= GameConfig.MaxShotSounds)
                return;
            shotPlays++;
            Shot.Play(0.12f, rand.NextFloat(-0.2f, 0.2f), 0);
        }

        public static void PlayExplosion()
        {
            if (explosionPlays >= GameConfig.MaxExplosionSounds)
                return;
            explosionPlays++;
            Explosion.Play(0.4f, rand.NextFloat(-0.4f, 0.4f), 0);
        }

        public static void OnPlayerHit()
        {
            Instance.shakeRemaining = 10f;
            Instance.hitFlash = 0.4f;
        }

        public static void OnEnemyKilled()
        {
            if (Instance.shakeRemaining < 3f)
                Instance.shakeRemaining = 3f;
        }

        void RestartToMenu()
        {
            PlayerStatus.Reset();
            PlayerShip.Instance.Reset();
            EntityManager.Clear();
            EnemySpawner.Reset();
            EntityManager.Add(PlayerShip.Instance);
            levelIndex = 0;
            menuIndex = 0;
            CurState = GameState.MainMenu;
            shakeRemaining = 0;
            hitFlash = 0;
        }

        void StartCurrentLevel()
        {
            LevelDef level = levels[levelIndex];
            EntityManager.ClearCombatEntities();
            EntityManager.EnsurePlayer();
            PlayerShip.Instance.PrepareForLevel();
            EnemySpawner.BeginLevel(level.Spawn);
            PlayerStatus.BeginLevel(level.Duration);
            CurState = GameState.Playing;
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            Input.Update();
            shotPlays = 0;
            explosionPlays = 0;
            UpdateFx(gameTime);

            bool hideCursor = CurState == GameState.Playing || CurState == GameState.Paused;
            IsMouseVisible = !hideCursor;

            switch (CurState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu();
                    break;
                case GameState.Briefing:
                    UpdateBriefing();
                    break;
                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case GameState.Paused:
                    UpdatePaused();
                    break;
                case GameState.GameOver:
                    UpdateGameOver();
                    break;
                case GameState.Victory:
                    UpdateVictory();
                    break;
            }

            base.Update(gameTime);
        }

        void UpdateFx(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shakeRemaining > 0)
            {
                shakeRemaining -= dt * 28f;
                if (shakeRemaining < 0)
                    shakeRemaining = 0;
                shakeOffset = new Vector2(
                    rand.NextFloat(-shakeRemaining, shakeRemaining),
                    rand.NextFloat(-shakeRemaining, shakeRemaining));
            }
            else
            {
                shakeOffset = Vector2.Zero;
            }

            if (hitFlash > 0)
            {
                hitFlash -= dt * 1.8f;
                if (hitFlash < 0)
                    hitFlash = 0;
            }
        }

        void UpdateMainMenu()
        {
            if (Input.WasMenuUpPressed() || Input.WasMenuDownPressed())
                menuIndex = 1 - menuIndex;

            if (Input.WasConfirmPressed())
            {
                if (menuIndex == 0)
                {
                    levelIndex = 0;
                    CurState = GameState.Briefing;
                }
                else
                {
                    Exit();
                }
            }
            else if (Input.WasKeyPressed(Keys.Escape) || Input.WasButtonPressed(Buttons.Back))
            {
                if (menuIndex == 1 || Input.WasButtonPressed(Buttons.Back))
                    Exit();
                else
                    menuIndex = 1;
            }
        }

        void UpdateBriefing()
        {
            if (Input.WasConfirmPressed())
                StartCurrentLevel();
            else if (Input.WasKeyPressed(Keys.Escape))
                RestartToMenu();
        }

        void UpdatePlaying(GameTime gameTime)
        {
            if (Input.WasKeyPressed(Keys.P) || Input.WasKeyPressed(Keys.Escape) || Input.WasButtonPressed(Buttons.Back))
            {
                CurState = GameState.Paused;
                return;
            }

            EntityManager.Update();
            EnemySpawner.Update();
            PlayerStatus.Update();
            PlayerStatus.TickTime((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (PlayerStatus.IsGameOver)
            {
                PlayerStatus.CommitHighScore();
                CurState = GameState.GameOver;
            }
            else if (PlayerStatus.RemainingTime <= 0)
            {
                EntityManager.ClearCombatEntities();
                levelIndex++;
                if (levelIndex >= levels.Length)
                {
                    PlayerStatus.CommitHighScore();
                    CurState = GameState.Victory;
                }
                else
                {
                    CurState = GameState.Briefing;
                }
            }
        }

        void UpdatePaused()
        {
            if (Input.WasKeyPressed(Keys.P) || Input.WasKeyPressed(Keys.Escape))
                CurState = GameState.Playing;
            else if (Input.WasKeyPressed(Keys.R))
                RestartToMenu();
        }

        void UpdateGameOver()
        {
            if (Input.WasKeyPressed(Keys.R) || Input.WasConfirmPressed() || Input.WasKeyPressed(Keys.Escape))
                RestartToMenu();
        }

        void UpdateVictory()
        {
            if (Input.WasKeyPressed(Keys.R) || Input.WasConfirmPressed() || Input.WasKeyPressed(Keys.Escape))
                RestartToMenu();
        }

        protected override void Draw(GameTime gameTime)
        {
            vpRect = GraphicsDevice.Viewport.Bounds;

            switch (CurState)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;
                case GameState.Briefing:
                    DrawBriefing();
                    break;
                case GameState.Playing:
                    DrawPlaying();
                    break;
                case GameState.Paused:
                    DrawPaused();
                    break;
                case GameState.GameOver:
                    DrawGameOver();
                    break;
                case GameState.Victory:
                    DrawVictory();
                    break;
            }

            base.Draw(gameTime);
        }

        void DrawFullscreen(Texture2D texture)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
            spriteBatch.Draw(texture, vpRect, Color.White);
            spriteBatch.End();
        }

        void DrawPlaying()
        {
            DrawFullscreen(levels[levelIndex].Background);
            DrawHud();
            DrawWorld();
            DrawHitFlash();
        }

        void DrawHud()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Color lifeColor = PlayerStatus.ExtraLifePulse > 0 ? Color.Gold : Color.White;
            spriteBatch.DrawString(Art.Font1, "Lives: " + PlayerStatus.Lives, new Vector2(8, 6), lifeColor);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 6);
            DrawRightAlignedString("Multiplier: x" + PlayerStatus.Multiplier, 32);
            string time = "Time " + PlayerStatus.RemainingTime.ToString("0.0");
            Vector2 timeSize = Art.Font21.MeasureString(time);
            spriteBatch.DrawString(Art.Font21, time, new Vector2((ScreenSize.X - timeSize.X) / 2f, 6), Color.White);
            spriteBatch.End();
        }

        void DrawWorld()
        {
            Matrix shake = Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, shake);
            EntityManager.Draw(spriteBatch);
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, null, Color.White, 0f, pointerOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        void DrawHitFlash()
        {
            if (hitFlash <= 0)
                return;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.Draw(pixel, vpRect, Color.White * hitFlash);
            spriteBatch.End();
        }

        void DrawMainMenu()
        {
            DrawFullscreen(Art.MainMenu);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            string start = menuIndex == 0 ? "> Start" : "  Start";
            string exit = menuIndex == 1 ? "> Exit" : "  Exit";
            string text = start + "\n" + exit + "\n\nWASD / Arrows move   Mouse aim   Left click fire\nSpace / Enter confirm   P pause";
            Vector2 size = Art.Font1.MeasureString(text);
            spriteBatch.DrawString(Art.Font1, text, new Vector2((ScreenSize.X - size.X) / 2f, ScreenSize.Y - size.Y - 28), Color.White);
            spriteBatch.End();
        }

        void DrawBriefing()
        {
            LevelDef level = levels[levelIndex];
            DrawFullscreen(level.BriefingArt);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            string text = level.Title + "\n" + level.Blurb + "\n\nPress Space or Enter to deploy";
            Vector2 size = Art.Font1.MeasureString(text);
            spriteBatch.DrawString(Art.Font1, text, new Vector2((ScreenSize.X - size.X) / 2f, ScreenSize.Y - size.Y - 28), Color.White);
            spriteBatch.End();
        }

        void DrawPaused()
        {
            DrawPlaying();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(pixel, vpRect, Color.Black * 0.55f);
            string text = "PAUSED\n\nPress P or ESC to Resume\nPress R to Restart";
            Vector2 size = Art.Font1.MeasureString(text);
            spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - size / 2, Color.White);
            spriteBatch.End();
        }

        void DrawGameOver()
        {
            DrawFullscreen(Art.BGalaxy2);
            DrawCenteredBlock(
                "Game Over\n" +
                "Your Score: " + PlayerStatus.Score + "\n" +
                "High Score: " + PlayerStatus.HighScore + "\n\n" +
                "Press R or Space to Restart");
        }

        void DrawVictory()
        {
            DrawFullscreen(Art.EndGame12);
            DrawCenteredBlock(
                "The sausage planet is saved\n" +
                "Your Score: " + PlayerStatus.Score + "\n" +
                "High Score: " + PlayerStatus.HighScore + "\n\n" +
                "Press Space or R to play again");
        }

        void DrawCenteredBlock(string text)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Vector2 size = Art.Font1.MeasureString(text);
            spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - size / 2, Color.White);
            spriteBatch.End();
        }

        void DrawRightAlignedString(string text, float y)
        {
            float textWidth = Art.Font1.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font1, text, new Vector2(ScreenSize.X - textWidth - 8, y), Color.White);
        }
    }
}
