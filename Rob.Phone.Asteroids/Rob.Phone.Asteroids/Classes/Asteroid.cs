using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rob.Phone.Asteroids.Classes
{
    public class Asteroid
    {
        private readonly float _radius;
        private readonly Texture2D _texture;
        private readonly Color _color;
        private readonly Game _game;
        private readonly Vector2 _velocity;

        private float _scale;
        private Vector2 _topLeft;
        private float _rotation;
        private float _rotationVelocity;
        private bool _destroyed;

        public Asteroid(Game game, Color color, Texture2D texture, Vector2 centre, Vector2 velocity, float radius)
        {
            _game = game;
            _color = color;
            _texture = texture;

            _topLeft = new Vector2(centre.X - radius, centre.Y - radius);
            _velocity = velocity;
            _radius = radius;

            var random = new Random();
            _rotation = random.Next(360);
            _rotationVelocity = (float)random.NextDouble() * 0.1f;

            CalculateScale();
        }

        public bool Destroyed
        {
            get { return _destroyed; }
        }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)_topLeft.X, (int)_topLeft.Y, (int)_radius * 2, (int)_radius * 2);
            }
        }

        private void CalculateScale()
        {
            float width = _texture.Bounds.Width;
            _scale = (_radius*2)/width;
        }

        public void Draw(SpriteBatch batch)
        {
            float originX = _topLeft.X - (_texture.Width * _scale);
            float originY = _topLeft.Y - (_texture.Height * _scale);
            Vector2 origin = new Vector2(originX, originY);
            batch.Draw(_texture, _topLeft, null, _color, _rotation, origin, _scale, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            CheckOffScreen();
            Rotate();
            _topLeft += _velocity;
        }

        private void Rotate()
        {
            _rotation += _rotationVelocity;
            if(_rotation > 360)
            {
                _rotation = 1;
            }
        }

        private void CheckOffScreen()
        {
            Vector2 newTopLeft = _topLeft + _velocity;
            float top = newTopLeft.Y;
            float left = newTopLeft.X;

            if (top - (_radius * 2)  < 0)
            {
                _topLeft = new Vector2(newTopLeft.X, _game.GraphicsDevice.Viewport.Height - (_radius * 2));
            }

            if (left - (_radius * 2) > _game.GraphicsDevice.Viewport.Width)
            {
                _topLeft = new Vector2(0 + (_radius * 2), newTopLeft.Y);
            }

            if (top - (_radius * 2) > _game.GraphicsDevice.Viewport.Height)
            {
                _topLeft = new Vector2(newTopLeft.X, 0 + (_radius * 2));    
            }

            if (left - (_radius * 2) < 0)
            {
                _topLeft = new Vector2(_game.GraphicsDevice.Viewport.Width - (_radius * 2), newTopLeft.Y);
            }
        }

        public void Destroy()
        {
            _destroyed = true;
        }
    }
}