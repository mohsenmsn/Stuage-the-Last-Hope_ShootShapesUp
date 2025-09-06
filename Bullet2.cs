using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class Bullet2 : Entity
    {
        public Bullet2(Vector2 position, Vector2 velocity)
        {
            image = Art.FBullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 10)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            // delete bullets that go off-screen
            if (!Game1.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }


    }
}


