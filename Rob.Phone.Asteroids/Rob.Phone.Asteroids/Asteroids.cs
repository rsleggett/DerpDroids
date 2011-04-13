using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Rob.Phone.Asteroids.Classes;

namespace Rob.Phone.Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Asteroids : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _asteroidTexture;
        private Texture2D _shipTexture;
        private Texture2D _bulletTexture;

        private const int AsteroidRate = 100;
        private const int MaxAsteroids = 5;
        private int _counter = 100;

        private List<Asteroid> _asteroids = new List<Asteroid>();
        private List<Bullet> _bullets = new List<Bullet>();

        private Ship _ship;
        private bool _touching;
        

        public Asteroids()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            _ship = new Ship(this, _shipTexture);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _asteroidTexture = Content.Load<Texture2D>("Asteroid");
            _shipTexture = Content.Load<Texture2D>("Ship");
            _bulletTexture = Content.Load<Texture2D>("Bullet");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            HandleTouches();

            CreateAsteroid();

            UpdateAsteroids();

            UpdateBullets();

            _ship.Update();

            base.Update(gameTime);
        }

        private void UpdateAsteroids()
        {
            _asteroids = _asteroids.Where(x => !x.Destroyed).ToList();
            foreach (var roid in _asteroids)
            {
                roid.Update();
            }
        }

        private void UpdateBullets()
        {
            foreach(var bullet in _bullets)
            {
                foreach(var asteroid in _asteroids)
                {
                    bullet.Update(asteroid);
                }
            }
        }

        private void CreateAsteroid()
        {
            if (_counter++ > AsteroidRate && _asteroids.Count < MaxAsteroids)
            {
                _counter = 0;

                var random = new Random(DateTime.Now.Millisecond);
                var color = new Color(random.Next(255), random.Next(255), random.Next(255));
                var velocity = new Vector2((random.NextDouble() > .5 ? -1 : 1) * random.Next(9), (random.NextDouble() > .5 ? -1 : 1) * random.Next(9)) + Vector2.UnitX + Vector2.UnitY;

                //TODO: this needs to calculate an edge of the viewport
                var centre = new Vector2(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);

                float radius = 25f*(float) random.NextDouble() + 15f;
                _asteroids.Add(new Asteroid(this, color, _asteroidTexture, centre, velocity, radius));
            }
        }

        private void HandleTouches()
        {
            TouchCollection touches = TouchPanel.GetState();
            if (!_touching && touches.Count > 0)
            {
                _touching = true;

                //create a bullet
                //send the bullet to the target
                var touch = touches.First();
                var bullet = new Bullet(this, _bulletTexture, new Vector2(_ship.Position.X - _ship.Radius,_ship.Position.Y), touch.Position);

                _bullets.Add(bullet);

                //Rotate ship towards touch position
                _ship.RotateTowards(touch.Position);
            }
            else if (touches.Count == 0)
            {
                _touching = false;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
            foreach(var roid in _asteroids)
            {
                roid.Draw(_spriteBatch);
            }

            _ship.Draw(_spriteBatch);

            foreach(var bullet in _bullets)
            {
                bullet.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
