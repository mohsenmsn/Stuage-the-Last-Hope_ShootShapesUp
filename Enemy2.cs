using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{

    class Enemy2 : Entity
    {
        public static Random rand = new Random();

        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        private int timeUntilStart = 60;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        public int PointValue { get; private set; }

        public Enemy2(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
            PointValue = 1;
        }

        public static Enemy2 CreateSeekto(Vector2 position)
        {
            var enemy = new Enemy2(Art.Seekto, position);
            enemy.AddBehaviour(enemy.FollowPlayer3());
            enemy.PointValue = 3;

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

            Velocity *= 0.9f;
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

        public void HandleCollision(Enemy2 other)
        {
            var d = Position - other.Position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }



        public void WasShot()
        {

            IsExpired = true;
            PlayerStatus.AddPoints(PointValue);
            PlayerStatus.IncreaseMultiplier();
            Game1.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);

        }

        #region Behaviours
        IEnumerable<int> FollowPlayer3(float acceleration = 0.8f)
        {
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)
                    Velocity += (PlayerShip.Instance.Position - Position) * (acceleration / (PlayerShip.Instance.Position - Position).Length());

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }
        #endregion
    }
}

