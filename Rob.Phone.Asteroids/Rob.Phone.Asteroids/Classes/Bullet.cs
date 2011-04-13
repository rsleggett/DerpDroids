using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rob.Phone.Asteroids.Classes;

namespace Rob.Phone.Asteroids
{
    internal class Bullet
    {
        private Vector2 _velocity;
        private readonly Game _game;
        private readonly Vector2 _origin;
        private readonly Vector2 _target;
        private readonly Texture2D _texture;
        private Vector2 _topLeft;
        private Vector2 _scale;
        private float _rotation;

        public Bullet(Game game, Texture2D texture, Vector2 origin, Vector2 target)
        {
            _game = game;
            _texture = texture;
            _origin = origin;
            _target = target;

            _topLeft = origin;
            _scale = new Vector2(0.1f, 0.1f);

            CalculateVelocity();
            RotateTowards(target);
        }

        private void CalculateVelocity()
        {
            int bulletSpeed = 10;
            Vector2 enemyVec = Vector2.Subtract(_target, _origin);
            float dist = Vector2.Distance(_target, _origin);

            var bulletTrajectory = Vector2.Multiply(Vector2.Normalize(enemyVec), bulletSpeed);
            float deltaX = bulletTrajectory.X;
            float deltaY = bulletTrajectory.Y;
            _velocity = new Vector2(deltaX, deltaY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float originX = _topLeft.X - (_texture.Width * _scale.X);
            float originY = _topLeft.Y - (_texture.Height * _scale.Y);
            Vector2 origin = new Vector2(originX, originY);
            spriteBatch.Draw(_texture, _topLeft, null, Color.Yellow, _rotation, origin, _scale, SpriteEffects.None, 0f);   
        }

        public void Update(Asteroid asteroid)
        {
            _topLeft += _velocity;

            if(asteroid.Hitbox.Contains(new Point((int)_topLeft.X,(int)_topLeft.Y)))
            {
                asteroid.Destroy();
            }
        }

        private void RotateTowards(Vector2 target)
        {
            float dx = _topLeft.X - target.X;
            float dy = _topLeft.Y - target.Y;
            _rotation = (float)Math.Atan2(dx, dy);
        }
    }
}