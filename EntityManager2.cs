using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EntityManager2
    {
        public static int score;
        static List<Entity> entities = new List<Entity>();
        static List<Bullet2> bullets2 = new List<Bullet2>();
        static List<Enemy2> enemies2 = new List<Enemy2>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        
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
            if (entity is Bullet2)
                    bullets2.Add(entity as Bullet2);
            else if (entity is Enemy2)
                enemies2.Add(entity as Enemy2);

        }

        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets2 = bullets2.Where(x => !x.IsExpired).ToList();
            enemies2 = enemies2.Where(x => !x.IsExpired).ToList();
         
    
        }

        static void HandleCollisions()
        {
            for (int i = 0; i < enemies2.Count; i++)
                for (int j = i + 1; j < enemies2.Count; j++)
                {
                    if (IsColliding(enemies2[i], enemies2[j]))
                    {
                        enemies2[i].HandleCollision(enemies2[j]);
                        enemies2[j].HandleCollision(enemies2[i]);
                        score++;
                    }
                }

            //Bullets and Enemy
            for (int i = 0; i < enemies2.Count; i++)
                for (int j = 0; j < bullets2.Count; j++)
                {
                    if (IsColliding(enemies2[i], bullets2[j]))
                    {
                        enemies2[i].WasShot();
                        bullets2[j].IsExpired = true;
                    }
                }

            for (int i = 0; i < enemies2.Count; i++)
                for (int j = 0; j < bullets2.Count; j++)
                {
                    if (IsColliding(enemies2[i], bullets2[j]))
                    {
                        enemies2[i].WasShot();
                        bullets2[j].IsExpired = true;
                    }
                }

            //Enemy and Player
            for (int i = 0; i < enemies2.Count; i++)
            {
                if (enemies2[i].IsActive && IsColliding(PlayerShip.Instance, enemies2[i]))
                {
                    PlayerShip.Instance.Kill();
                    enemies2.ForEach(x => x.WasShot());
                    EnemySpawner.Reset();
                    break;
                }
            }

           
            

        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }

        public static void Clear()
        {
            entities.Clear();
            enemies2.Clear();
            bullets2.Clear();
            addedEntities.Clear();
        }
    }
}
