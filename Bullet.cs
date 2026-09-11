using Microsoft.Xna.Framework;

namespace ShootShapesUp
{
    class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.FBullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = GameConfig.BulletRadius;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            if (!Game1.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}
