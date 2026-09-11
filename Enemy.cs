using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ShootShapesUp
{
    class Enemy : Entity
    {
        public static readonly Random rand = new Random();
        private readonly List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        private int timeUntilStart = 60;
        private float friction = 0.8f;

        public bool IsActive { get { return timeUntilStart <= 0; } }
        public int PointValue { get; private set; }

        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
            PointValue = 1;
        }

        public static Enemy CreateSeek(Vector2 position, float acceleration)
        {
            var enemy = new Enemy(Art.Seek, position);
            enemy.AddBehaviour(enemy.FollowPlayer(acceleration));
            enemy.PointValue = 2;
            enemy.friction = 0.8f;
            return enemy;
        }

        public static Enemy CreateWanderer(Vector2 position)
        {
            var enemy = new Enemy(Art.Wanderer, position);
            enemy.AddBehaviour(enemy.MoveRandomly());
            enemy.PointValue = 1;
            enemy.friction = 0.8f;
            return enemy;
        }

        public static Enemy CreateElite(Vector2 position)
        {
            var enemy = new Enemy(Art.Seekto, position);
            enemy.AddBehaviour(enemy.FollowPlayer(0.9f));
            enemy.PointValue = 3;
            enemy.friction = 0.88f;
            enemy.Radius *= 0.9f;
            return enemy;
        }

        public override void Update()
        {
            if (timeUntilStart <= 0)
                ApplyBehaviours();
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);
            Velocity *= friction;
        }

        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        public void WasShot()
        {
            if (IsExpired)
                return;

            IsExpired = true;
            PlayerStatus.AddPoints(PointValue);
            PlayerStatus.IncreaseMultiplier();
            Game1.PlayExplosion();
            Game1.OnEnemyKilled();
        }

        public void ExpireSilent()
        {
            if (IsExpired)
                return;

            IsExpired = true;
            Game1.PlayExplosion();
        }

        IEnumerable<int> FollowPlayer(float acceleration)
        {
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)
                {
                    Vector2 offset = PlayerShip.Instance.Position - Position;
                    float length = offset.Length();
                    if (length > 0.001f)
                        Velocity += offset * (acceleration / length);
                }

                if (Velocity.LengthSquared() > 0)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

        IEnumerable<int> MoveRandomly()
        {
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);
                    Orientation -= 0.05f;

                    var bounds = Game1.Viewport.Bounds;
                    bounds.Inflate(-image.Width, -image.Height);

                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (Game1.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }
    }
}
