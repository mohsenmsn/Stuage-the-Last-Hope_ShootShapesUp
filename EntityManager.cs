using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ShootShapesUp
{
    static class EntityManager
    {
        static readonly List<Entity> entities = new List<Entity>(256);
        static readonly List<Enemy> enemies = new List<Enemy>(128);
        static readonly List<Bullet> bullets = new List<Bullet>(128);
        static readonly List<Entity> addedEntities = new List<Entity>(32);

        static bool isUpdating;

        public static int Count { get { return entities.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet bullet)
                bullets.Add(bullet);
            else if (entity is Enemy enemy)
                enemies.Add(enemy);
        }

        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();

            for (int i = 0; i < entities.Count; i++)
                entities[i].Update();

            isUpdating = false;

            for (int i = 0; i < addedEntities.Count; i++)
                AddEntity(addedEntities[i]);
            addedEntities.Clear();

            RemoveExpired(entities);
            RemoveExpired(bullets);
            RemoveExpired(enemies);
        }

        static void RemoveExpired<T>(List<T> list) where T : Entity
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].IsExpired)
                    list.RemoveAt(i);
            }
        }

        static void HandleCollisions()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy a = enemies[i];
                if (a.IsExpired)
                    continue;

                for (int j = i + 1; j < enemies.Count; j++)
                {
                    Enemy b = enemies[j];
                    if (b.IsExpired)
                        continue;

                    if (IsColliding(a, b))
                    {
                        a.HandleCollision(b);
                        b.HandleCollision(a);
                    }
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                if (enemy.IsExpired)
                    continue;

                for (int j = 0; j < bullets.Count; j++)
                {
                    Bullet bullet = bullets[j];
                    if (bullet.IsExpired)
                        continue;

                    if (IsColliding(enemy, bullet))
                    {
                        enemy.WasShot();
                        bullet.IsExpired = true;
                    }
                }
            }

            PlayerShip player = PlayerShip.Instance;
            if (player.IsInvulnerable)
                return;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                if (!enemy.IsActive || enemy.IsExpired)
                    continue;

                if (IsColliding(player, enemy))
                {
                    player.Kill();
                    break;
                }
            }
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void ExpireEnemiesNear(Vector2 position, float radius)
        {
            float r2 = radius * radius;
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                if (!enemy.IsExpired && Vector2.DistanceSquared(enemy.Position, position) <= r2)
                    enemy.ExpireSilent();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch);
        }

        public static void Clear()
        {
            entities.Clear();
            enemies.Clear();
            bullets.Clear();
            addedEntities.Clear();
        }

        public static void ClearCombatEntities()
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                if (!(entities[i] is PlayerShip))
                    entities.RemoveAt(i);
            }

            enemies.Clear();
            bullets.Clear();
            addedEntities.Clear();
        }

        public static void EnsurePlayer()
        {
            if (!entities.Contains(PlayerShip.Instance))
                Add(PlayerShip.Instance);
        }
    }
}
