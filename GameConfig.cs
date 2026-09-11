namespace ShootShapesUp
{
    static class GameConfig
    {
        public const int MaxEntities = 200;
        public const int StartingLives = 4;
        public const int ExtraLifeInterval = 3000;
        public const int MaxMultiplier = 25;
        public const float MultiplierExpiry = 0.8f;

        public const float Level1Duration = 20f;
        public const float Level2Duration = 30f;
        public const float Level3Duration = 40f;

        public const float PlayerSpeed = 4.5f;
        public const float BulletSpeed = 12f;
        public const int FireCooldownFrames = 7;
        public const int RespawnFrames = 90;
        public const int InvulnFrames = 90;
        public const int LevelStartInvulnFrames = 60;
        public const float PlayerRadius = 10f;
        public const float BulletRadius = 8f;
        public const float DualBarrelOffset = 14f;
        public const float MuzzleOffset = 36f;
        public const float FireSpread = 0.04f;

        public const float SpawnSafeRadius = 220f;
        public const int SpawnAttempts = 16;
        public const int MaxShotSounds = 2;
        public const int MaxExplosionSounds = 4;

        public const float DeathClearRadius = 180f;
    }
}
