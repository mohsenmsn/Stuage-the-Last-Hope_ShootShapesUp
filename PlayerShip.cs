using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class PlayerShip : Entity
    {
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }

        const int cooldownFrames = 6;
        int cooldownRemaining = 0;

        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }

        static Random rand = new Random();

        private PlayerShip()
        {
            image = Art.SPlayer;
            Position = Game1.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update()
        {
            if (IsDead)
            {
                if (--framesUntilRespawn == 0)
                {
                    if (PlayerStatus.Lives == 0)
                    {
                        PlayerStatus.Reset();
                        Position = Game1.ScreenSize / 2;
                    }
                }
                return;
            }

            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = rand.NextFloat(-0.05f, 0.05f) + rand.NextFloat(-0.05f, 0.05f);
                Vector2 vel = 5f * new Vector2((float)Math.Cos(aimAngle + randomSpread), (float)Math.Sin(aimAngle + randomSpread));
                Vector2 vel1 = 4f * new Vector2((float)Math.Cos(aimAngle + randomSpread), (float)Math.Sin(aimAngle + randomSpread));

                Vector2 offset = Vector2.Transform(new Vector2(50, -0), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));
                Vector2 off = Vector2.Transform(new Vector2(50, -50), aimQuat);
                EntityManager2.Add(new Bullet2(Position + off, vel1));

                Game1.Shot.Play(0.1f, rand.NextFloat(-0.2f, 0.2f), 0);
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

            const float speed = 4; // Reduced from 8 to 4 for slower, more strategic movement
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }

        public void Kill()
        {
            PlayerStatus.RemoveLife();
            framesUntilRespawn = PlayerStatus.IsGameOver ? 300 : 120;
        }
    }
}
