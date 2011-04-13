using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rob.Phone.Asteroids
{
    public class Ship
    {
        private readonly Game _game;
        private readonly Texture2D _texture;
        private readonly Vector2 _position;
        private readonly Vector2 _scale;
        private float _rotation = 0f;
        private float width;
        private float height;

        public Ship(Game game, Texture2D texture)
        {
            _game = game;
            _texture = texture;
            _position = new Vector2(_game.GraphicsDevice.Viewport.Width / 2, _game.GraphicsDevice.Viewport.Height / 2);
            _scale = new Vector2(0.25f, 0.25f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float originX = _position.X - (_texture.Width*_scale.X);
            float originY = _position.Y - (_texture.Height * _scale.Y);
            Vector2 origin = new Vector2(originX, originY);
            spriteBatch.Draw(_texture, _position, null, Color.Tomato, _rotation, origin, _scale, SpriteEffects.FlipVertically, 0f);
        }

        public void Update()
        {
            
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public float Radius
        {
            get
            {
                return _texture.Width*_scale.X;
            }
        }

        public void RotateTowards(Vector2 target)
        {
            float dx = _position.X - target.X;
            float dy = _position.Y - target.Y;
            _rotation = (float)Math.Atan2(dx, dy);
        }
    }
}