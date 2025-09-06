using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Radius = 8;
        }

        public bool IsExpired2 { get; internal set; }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            // delete bullets that go off-screen
            if (!Game1.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}
