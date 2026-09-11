using Microsoft.Xna.Framework;
using System;

namespace ShootShapesUp
{
    struct SpawnProfile
    {
        public float InverseStart;
        public float InverseMin;
        public float Ramp;
        public int SeekRolls;
        public int WandererRolls;
        public int EliteRolls;
        public float SeekAccel;
        public float EliteChanceScale;
    }

    static class EnemySpawner
    {
        static readonly Random rand = new Random();
        static float inverseSpawnChance = 60;
        static SpawnProfile profile;

        public static void BeginLevel(SpawnProfile spawnProfile)
        {
            profile = spawnProfile;
            inverseSpawnChance = spawnProfile.InverseStart;
        }

        public static void Reset()
        {
            inverseSpawnChance = profile.InverseStart > 0 ? profile.InverseStart : 60;
        }

        public static void Update()
        {
            if (PlayerShip.Instance.IsDead)
                return;

            if (EntityManager.Count < GameConfig.MaxEntities)
            {
                int chance = SpawnChance();
                for (int i = 0; i < profile.SeekRolls; i++)
                {
                    if (rand.Next(chance) == 0)
                        EntityManager.Add(Enemy.CreateSeek(GetSpawnPosition(), profile.SeekAccel));
                }

                for (int i = 0; i < profile.WandererRolls; i++)
                {
                    if (rand.Next(chance) == 0)
                        EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));
                }

                if (profile.EliteRolls > 0)
                {
                    int eliteChance = (int)(chance * profile.EliteChanceScale);
                    if (eliteChance < 1)
                        eliteChance = 1;
                    for (int i = 0; i < profile.EliteRolls; i++)
                    {
                        if (rand.Next(eliteChance) == 0)
                            EntityManager.Add(Enemy.CreateElite(GetSpawnPosition()));
                    }
                }
            }

            if (inverseSpawnChance > profile.InverseMin)
                inverseSpawnChance -= profile.Ramp;
        }

        static int SpawnChance()
        {
            int chance = (int)inverseSpawnChance;
            return chance < 1 ? 1 : chance;
        }

        private static Vector2 GetSpawnPosition()
        {
            float safeRadiusSq = GameConfig.SpawnSafeRadius * GameConfig.SpawnSafeRadius;
            Vector2 pos = Vector2.Zero;

            for (int i = 0; i < GameConfig.SpawnAttempts; i++)
            {
                pos = new Vector2(rand.Next((int)Game1.ScreenSize.X), rand.Next((int)Game1.ScreenSize.Y));
                if (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) >= safeRadiusSq)
                    return pos;
            }

            return EdgeSpawn();
        }

        static Vector2 EdgeSpawn()
        {
            float margin = 40f;
            switch (rand.Next(4))
            {
                case 0:
                    return new Vector2(rand.NextFloat(0, Game1.ScreenSize.X), margin);
                case 1:
                    return new Vector2(rand.NextFloat(0, Game1.ScreenSize.X), Game1.ScreenSize.Y - margin);
                case 2:
                    return new Vector2(margin, rand.NextFloat(0, Game1.ScreenSize.Y));
                default:
                    return new Vector2(Game1.ScreenSize.X - margin, rand.NextFloat(0, Game1.ScreenSize.Y));
            }
        }
    }
}
