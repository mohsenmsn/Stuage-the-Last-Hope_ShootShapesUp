using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        int cooldownRemaining;
        int framesUntilRespawn;
        int invulnFrames;
        static readonly Random rand = new Random();

        public bool IsDead { get { return framesUntilRespawn > 0; } }
        public bool IsInvulnerable { get { return invulnFrames > 0 || IsDead; } }

        private PlayerShip()
        {
            image = Art.SPlayer;
            Position = Game1.ScreenSize / 2;
            Radius = GameConfig.PlayerRadius;
        }

        public void Reset()
        {
            framesUntilRespawn = 0;
            cooldownRemaining = 0;
            invulnFrames = 0;
            Velocity = Vector2.Zero;
            Position = Game1.ScreenSize / 2;
            Orientation = 0;
            color = Color.White;
            IsExpired = false;
        }

        public void PrepareForLevel()
        {
            cooldownRemaining = 0;
            Velocity = Vector2.Zero;
            Position = Game1.ScreenSize / 2;
            invulnFrames = GameConfig.LevelStartInvulnFrames;
            if (framesUntilRespawn > 0 && !PlayerStatus.IsGameOver)
                framesUntilRespawn = 0;
            color = Color.White;
        }

        public override void Update()
        {
            if (IsDead)
            {
                framesUntilRespawn--;
                if (framesUntilRespawn <= 0 && !PlayerStatus.IsGameOver)
                    invulnFrames = GameConfig.InvulnFrames;
                return;
            }

            if (invulnFrames > 0)
                invulnFrames--;

            var aim = Input.GetAimDirection();
            bool firing = Input.IsFiring() && aim.LengthSquared() > 0;

            if (firing && cooldownRemaining <= 0)
            {
                Fire(aim);
                cooldownRemaining = GameConfig.FireCooldownFrames;
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

            Velocity = GameConfig.PlayerSpeed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);

            if (firing)
                Orientation = aim.ToAngle();
            else if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
        }

        void Fire(Vector2 aim)
        {
            float aimAngle = aim.ToAngle();
            Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

            FireBarrel(aimAngle, aimQuat, new Vector2(GameConfig.MuzzleOffset, -GameConfig.DualBarrelOffset));
            FireBarrel(aimAngle, aimQuat, new Vector2(GameConfig.MuzzleOffset, GameConfig.DualBarrelOffset));
            Game1.PlayShot();
        }

        void FireBarrel(float aimAngle, Quaternion aimQuat, Vector2 localOffset)
        {
            float spread = rand.NextFloat(-GameConfig.FireSpread, GameConfig.FireSpread) + rand.NextFloat(-GameConfig.FireSpread, GameConfig.FireSpread);
            Vector2 vel = GameConfig.BulletSpeed * new Vector2((float)Math.Cos(aimAngle + spread), (float)Math.Sin(aimAngle + spread));
            Vector2 offset = Vector2.Transform(localOffset, aimQuat);
            EntityManager.Add(new Bullet(Position + offset, vel));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead)
                return;

            if (invulnFrames > 0 && (invulnFrames / 4) % 2 == 0)
                return;

            base.Draw(spriteBatch);
        }

        public void Kill()
        {
            if (IsDead || invulnFrames > 0)
                return;

            PlayerStatus.RemoveLife();
            framesUntilRespawn = PlayerStatus.IsGameOver ? 1 : GameConfig.RespawnFrames;
            EntityManager.ExpireEnemiesNear(Position, GameConfig.DeathClearRadius);
            Game1.OnPlayerHit();
        }
    }
}
