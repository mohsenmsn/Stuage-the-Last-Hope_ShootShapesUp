using System.IO;

namespace ShootShapesUp
{
    static class PlayerStatus
    {
        private const string highScoreFilename = "highscore.txt";

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }
        public static float RemainingTime { get; private set; }
        public static float ExtraLifePulse { get; private set; }
        public static int HighScore { get; private set; }
        public static bool IsGameOver { get { return Lives <= 0; } }

        private static float multiplierTimeLeft;
        private static int scoreForExtraLife;

        private static int LoadHighScore()
        {
            int score;
            return File.Exists(highScoreFilename) && int.TryParse(File.ReadAllText(highScoreFilename), out score) ? score : 0;
        }

        private static void SaveHighScore(int score)
        {
            File.WriteAllText(highScoreFilename, score.ToString());
        }

        static PlayerStatus()
        {
            HighScore = LoadHighScore();
            Reset();
        }

        public static void Reset()
        {
            CommitHighScore();
            Score = 0;
            Multiplier = 1;
            Lives = GameConfig.StartingLives;
            scoreForExtraLife = GameConfig.ExtraLifeInterval;
            multiplierTimeLeft = GameConfig.MultiplierExpiry;
            RemainingTime = GameConfig.Level1Duration;
            ExtraLifePulse = 0;
        }

        public static void BeginLevel(float duration)
        {
            RemainingTime = duration;
        }

        public static void TickTime(float elapsedSeconds)
        {
            RemainingTime -= elapsedSeconds;
            if (RemainingTime < 0)
                RemainingTime = 0;
        }

        public static void CommitHighScore()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);
        }

        public static void Update()
        {
            if (ExtraLifePulse > 0)
            {
                ExtraLifePulse -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds * 2f;
                if (ExtraLifePulse < 0)
                    ExtraLifePulse = 0;
            }

            if (Multiplier > 1)
            {
                multiplierTimeLeft -= (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (multiplierTimeLeft <= 0)
                    ResetMultiplier();
            }
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
                return;

            Score += basePoints * Multiplier;
            while (Score >= scoreForExtraLife)
            {
                scoreForExtraLife += GameConfig.ExtraLifeInterval;
                Lives++;
                ExtraLifePulse = 1f;
            }
        }

        public static void IncreaseMultiplier()
        {
            if (PlayerShip.Instance.IsDead)
                return;

            multiplierTimeLeft = GameConfig.MultiplierExpiry;
            if (Multiplier < GameConfig.MaxMultiplier)
                Multiplier++;
        }

        public static void ResetMultiplier()
        {
            Multiplier = 1;
            multiplierTimeLeft = GameConfig.MultiplierExpiry;
        }

        public static void RemoveLife()
        {
            if (Lives > 0)
                Lives--;
            ResetMultiplier();
        }
    }
}
