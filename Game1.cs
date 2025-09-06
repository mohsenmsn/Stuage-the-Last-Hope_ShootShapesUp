using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Threading;

namespace ShootShapesUp
{
    // This project was inspired by common MonoGame shoot 'em up tutorial patterns
    // and adapted with original creative elements including the "Stuage the Last Hope" storyline.
    // See REFERENCES.md for detailed source attributions.
   
    public class Game1 : Game
    {
        enum GameState
        {
            MainMenu,
            Break1,
            Level1,
            Break2,
            Level2,
            Level3,
            EndGame,
            GameOver,
            Paused,
            Exit,
        }


        GameState CurState;
        //= GameState.MainMenu;

        // some helpful static properties
        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public static Song Music { get; private set; }
        private static readonly Random rand = new Random();
        private static SoundEffect[] explosions;
        // return a random explosion sound
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }
        private static SoundEffect[] shots;
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }
        private static SoundEffect[] spawns;
        public static SoundEffect Spawn { get { return spawns[rand.Next(spawns.Length)]; } }

        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle vpRect;
        //************************
       




        //*******************

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight =700;
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            
            // Initialize player ship
            EntityManager.Add(PlayerShip.Instance);
            EntityManager2.Add(PlayerShip.Instance);

            // Start background music
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Game1.Music);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);

            Music = Content.Load<Song>("Sound/Music");

            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            shots = Enumerable.Range(1, 4).Select(x => Content.Load<SoundEffect>("Sound/shoot-0" + x)).ToArray();
            spawns = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/spawn-0" + x)).ToArray();
        }
        protected override void UnloadContent()
        {
            // Clean up resources if needed
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            Input.Update();
            
                // Allows the game to exit
                if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                {
                    if (CurState == GameState.Level1 || CurState == GameState.Level2 || CurState == GameState.Level3)
                        CurState = GameState.Paused;
                    else
                        this.Exit();
                }

            EntityManager.Update();
            EnemySpawner.Update();
            PlayerStatus.Update();
            EntityManager2.Update();

            switch (CurState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Break1:
                    UpdateBreak1(gameTime);
                    break;

                case GameState.Level1:
                    UpdateLevel1(gameTime);
                    break;
                    
                case GameState.Break2:
                    UpdateBreak1(gameTime);
                    break;

                case GameState.Level2:
                    UpdateLevel2(gameTime);
                    break;
                case GameState.Level3:
                    UpdateLevel3(gameTime);
                    break;

                case GameState.EndGame:
                    UpdateEndGame(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
                case GameState.Paused:
                    UpdatePaused(gameTime);
                    break;
                case GameState.Exit:
                    Exit();
                    break;
            }
          //  PlayerStatus.timer2 -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
            
        }
        
        void UpdateMainMenu(GameTime gameTime)
        {
            CurState = GameState.MainMenu;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                CurState = GameState.Break1;
        }
        void UpdateBreak1(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                CurState = GameState.Level1;
        }
        void UpdateLevel1(GameTime gameTime)
        {
            if (Input.WasKeyPressed(Keys.P))
                CurState = GameState.Paused;

            EntityManager.Update();
            EnemySpawner.Update();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PlayerStatus.timer -= elapsed;
            if (PlayerShip.Instance.IsDead)
                CurState = GameState.GameOver;
            if (PlayerStatus.timer <= 0)
                CurState = GameState.Level2;
        }
        void UpdateLevel2(GameTime gameTime)
        {
            if (Input.WasKeyPressed(Keys.P))
                CurState = GameState.Paused;

            EntityManager.Update();
            EnemySpawner.Update();

            float secElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PlayerStatus.timer2 -= secElapsed;
            if(PlayerShip.Instance.IsDead)
                CurState = GameState.GameOver;
            if (PlayerStatus.timer2 <= 0)
                CurState = GameState.Level3;
        }
        void UpdateLevel3(GameTime gameTime)
        {
            if (Input.WasKeyPressed(Keys.P))
                CurState = GameState.Paused;

            EntityManager.Update();
            EntityManager2.Update();
            EnemySpawner.Update();

            float thrdElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PlayerStatus.timer3 -= thrdElapsed;
            if (PlayerShip.Instance.IsDead)
                CurState = GameState.GameOver;
            if (PlayerStatus.timer3 <= 0)
                CurState = GameState.EndGame;
        }
        void UpdateGameOver(GameTime gameTime)
        {
            if (Input.WasKeyPressed(Keys.R))
            {
                PlayerStatus.Reset();
                EntityManager.Clear();
                EntityManager2.Clear();
                EnemySpawner.Reset();
                CurState = GameState.MainMenu;
            }
        }
        void UpdateEndGame(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    CurState = GameState.Exit;
        }

        void UpdatePaused(GameTime gameTime)
        {
            if (Input.WasKeyPressed(Keys.P) || Input.WasKeyPressed(Keys.Escape))
            {
                // Return to the previous level
                if (PlayerStatus.timer > 0)
                    CurState = GameState.Level1;
                else if (PlayerStatus.timer2 > 0)
                    CurState = GameState.Level2;
                else if (PlayerStatus.timer3 > 0)
                    CurState = GameState.Level3;
            }
            else if (Input.WasKeyPressed(Keys.R))
            {
                // Restart the game
                PlayerStatus.Reset();
                EntityManager.Clear();
                EntityManager2.Clear();
                EnemySpawner.Reset();
                CurState = GameState.MainMenu;
            }
        }




        protected override void Draw(GameTime gameTime)
        {

            switch (CurState)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.Break1:
                    DrawBreak1(gameTime);
                    break;
                case GameState.Level1:
                    DrawLevel1(gameTime);
                    break;
                case GameState.Break2:
                    DrawLBreak2(gameTime);
                    break;
                case GameState.Level2:
                    DrawLevel2(gameTime);
                    break;
                case GameState.Level3:
                    DrawLevel3(gameTime);
                    break;
                case GameState.EndGame:
                    DrawEndGame(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
                case GameState.Paused:
                    DrawPaused(gameTime);
                    break;
            }
            
            base.Draw(gameTime);
        }

       
        void DrawMainMenu(GameTime deltaTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.MainMenu, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.End();     
        }
        private void DrawBreak1(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.BGalaxy11, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.End();
        }

      /*  void DrawGamePlay(GameTime deltaTime)
        {
            // Draw the background for level 1 
            // Draw enemies // Draw the player 
            // Draw particle effects , etc

        }*/
       
        private void DrawLevel1(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.BGalaxy, vpRect, Color.White);
            vpRect = new Rectangle(0, 0, 
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.End();

            spriteBatch.Begin(0, BlendState.Additive);
            spriteBatch.DrawString(Art.Font1, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);
            spriteBatch.DrawString(Art.Font21, "Time to Survive:" + PlayerStatus.timer.ToString(" 0.00"), new Vector2(400, 5), Color.White);
            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n"
                    + "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.Font1.MeasureString(text);
                spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - textSize / 2, Color.White);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            EntityManager.Draw(spriteBatch);
            
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            spriteBatch.End();

        }
        private void DrawLBreak2(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.BGalaxy12, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.End();
        }

        private void DrawLevel2(GameTime gameTime)
        {
            //######################################2
            
             //#######################################
             spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.BGalaxy21, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.End();

            spriteBatch.Begin(0, BlendState.Additive);
            spriteBatch.DrawString(Art.Font1, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);

            spriteBatch.DrawString(Art.Font21, "Time to Survive:" + PlayerStatus.timer2.ToString(" 0.00"), new Vector2(450, 5), Color.White);
            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n"
                    + "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.Font1.MeasureString(text);
                spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - textSize / 2, Color.White);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            EntityManager.Draw(spriteBatch);
            //EntityManager2.Draw(spriteBatch);
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            spriteBatch.End();
        }
        private void DrawLevel3(GameTime gameTime)
        {
            //######################################3
            
            //#######################################
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.BGalaxy1, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.End();

            spriteBatch.Begin(0, BlendState.Additive);
            spriteBatch.DrawString(Art.Font1, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);

            spriteBatch.DrawString(Art.Font21, "Time to Survive:" + PlayerStatus.timer3.ToString(" 0.00"), new Vector2(450, 5), Color.White);
            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n"
                    + "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.Font1.MeasureString(text);
                spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - textSize / 2, Color.White);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            EntityManager.Draw(spriteBatch);
            EntityManager2.Draw(spriteBatch);
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            spriteBatch.End();
        }
        void DrawEndGame(GameTime deltaTime)
        {

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.EndGame12, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);


            spriteBatch.End();
        }

        void DrawGameOver(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Art.BGalaxy2, vpRect, Color.White);
            vpRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            
            string text = 
                "Your Score: " + PlayerStatus.Score + "\n" +
                "High Score: " + PlayerStatus.HighScore + "\n" +
                "Press ESC to Exit or R to Restart";

            Vector2 textSize = Art.Font1.MeasureString(text);
            spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - textSize / 2, Color.White);

            spriteBatch.End();
        }

        void DrawPaused(GameTime gameTime)
        {
            // Draw the current level in the background
            if (PlayerStatus.timer > 0)
                DrawLevel1(gameTime);
            else if (PlayerStatus.timer2 > 0)
                DrawLevel2(gameTime);
            else if (PlayerStatus.timer3 > 0)
                DrawLevel3(gameTime);

            // Draw pause overlay
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            // Semi-transparent overlay
            spriteBatch.Draw(Art.BGalaxy, vpRect, Color.Black * 0.5f);
            
            string text = "PAUSED\n\nPress P or ESC to Resume\nPress R to Restart";
            Vector2 textSize = Art.Font1.MeasureString(text);
            spriteBatch.DrawString(Art.Font1, text, ScreenSize / 2 - textSize / 2, Color.White);
            
            spriteBatch.End();
        }


        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font1.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font1, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
    }
}
