using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace TestMovement
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ship;
        Texture2D control;
        Texture2D controlPressed;
        Figure shipFigure;
        bool alive = true;

        Texture2D enemyRed;
        Texture2D enemyPink;
        List<Enemy> enemies = new List<Enemy>();
        Figure enemyFigure;

        Texture2D blueBullet;
        List<Bullet> enemyBullets = new List<Bullet>();

        Texture2D background;
        Vector2 backgroundPosition;
        Texture2D sky;
        Vector2 skyPosition;

        Texture2D explode;

        float shipSpeed;

        Texture2D bullet1;
        List<Bullet> myBullets = new List<Bullet>();
        int myBullet_DelayMS = 300;
        int myBullet_LastMS = 0;

        float currentRotation = 0;
        bool pressed = false;

        Vector2 origin;

        Song back_music;
        float musicVolume = 1.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Viewport screen = GraphicsDevice.Viewport;

            back_music = Content.Load<Song>("on-the-run-2");
            MediaPlayer.Play(back_music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = musicVolume;

            ship = Content.Load<Texture2D>("kspaceduel");
            control = Content.Load<Texture2D>("control2");
            controlPressed = Content.Load<Texture2D>("control4");
            bullet1 = Content.Load<Texture2D>("bullet3");
            background = Content.Load<Texture2D>("background2");
            sky = Content.Load<Texture2D>("sky2");
            enemyRed = Content.Load<Texture2D>("enemy1_2");
            enemyPink = Content.Load<Texture2D>("enemy1_1");
            explode = Content.Load<Texture2D>("explosion07");
            blueBullet = Content.Load<Texture2D>("blueBullet15");

            origin = new Vector2(screen.Width / 2, screen.Height - screen.Width / 2 - 100);

            backgroundPosition = new Vector2((background.Width - screen.Width) / 2, (background.Height - screen.Width) / 2);
            skyPosition = new Vector2((sky.Width - screen.Width) / 2, (sky.Height - screen.Width) / 2);
            shipSpeed = 0.1f;

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(0.0f, -29.0f);
            points[1] = new Vector2(22.0f, -1.0f);
            points[2] = new Vector2(0.0f, 29.0f);
            points[3] = new Vector2(-22.0f, -1.0f);
            shipFigure = new Figure(points);
            points = new Vector2[4];
            points[0] = new Vector2(0.0f, -24.0f);
            points[1] = new Vector2(19.0f, -10.0f);
            points[2] = new Vector2(0.0f, 24.0f);
            points[3] = new Vector2(-19.0f, -10.0f);
            enemyFigure = new Figure(points);

            enemies.Add(new Enemy(new Vector2(120, 700), 0.07f, 0.0f, 0.0f, enemyFigure));
            enemies.Add(new Enemy(new Vector2(360, 700), 0.08f, 0.0f, 0.0f, enemyFigure));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed )
                this.Exit();

            Vector2 globalShift = Vector2.Zero;
            globalShift.X = shipSpeed * (float)Math.Sin(-2 * Math.PI * currentRotation) * (float)gameTime.ElapsedGameTime.Milliseconds;
            globalShift.Y = shipSpeed * (float)Math.Cos(2 * Math.PI * currentRotation) * (float)gameTime.ElapsedGameTime.Milliseconds;
            skyPosition.X = skyPosition.X + globalShift.X;
            if (skyPosition.X > sky.Width - 480) {skyPosition.X = skyPosition.X - sky.Width + 480;}
            if (skyPosition.X < 0) { skyPosition.X = skyPosition.X + sky.Width - 480; }
            skyPosition.Y = skyPosition.Y + globalShift.Y;
            if (skyPosition.Y > sky.Height - 800) { skyPosition.Y = skyPosition.Y - sky.Height + 800; }
            if (skyPosition.Y < 0 ) { skyPosition.Y = skyPosition.Y + sky.Height - 800; }
            backgroundPosition.X = backgroundPosition.X + 0.5f * globalShift.X;
            if (backgroundPosition.X > background.Width - 480) { backgroundPosition.X = backgroundPosition.X - background.Width + 480; }
            if (backgroundPosition.X < 0) { backgroundPosition.X = backgroundPosition.X + background.Width - 770; }
            backgroundPosition.Y = backgroundPosition.Y + 0.5f * globalShift.Y;
            if (backgroundPosition.Y > background.Height - 800) { backgroundPosition.Y = backgroundPosition.Y - background.Height + 800; }
            if (backgroundPosition.Y < 0) { backgroundPosition.Y = background.Height - 800 + backgroundPosition.Y; }

            myBullet_LastMS = myBullet_LastMS + gameTime.ElapsedGameTime.Milliseconds;

            Figure shipF = new Figure(shipFigure.points, currentRotation, origin);
            //(float)gameTime.ElapsedGameTime.Milliseconds / 1000
            List<Bullet> bulletsToDelete = new List<Bullet>();
            foreach (Bullet bullet in myBullets)
            {
                bullet.Update(gameTime.ElapsedGameTime.Milliseconds, globalShift);
                if (!bullet.alive || bullet.position.X < 0 || bullet.position.Y < 0 || bullet.position.X > 480 || bullet.position.Y > 800) 
                { 
                    bulletsToDelete.Add(bullet); 
                }
            }
            foreach (Bullet bullet in bulletsToDelete)
            {
                myBullets.Remove(bullet);
            }

            while (myBullet_LastMS >= myBullet_DelayMS)
            {
                myBullet_LastMS = myBullet_LastMS - myBullet_DelayMS;
                Bullet bullet1 = new Bullet(origin, 0.15f, currentRotation, 42.0f); // 100/1sec = 0.1
                bullet1.Update(myBullet_LastMS);
                myBullets.Add(bullet1);
                Bullet bullet2 = new Bullet(origin, 0.15f, 0.5f+currentRotation, 42.0f); // 100/1sec = 0.1
                bullet2.Update(myBullet_LastMS);
                myBullets.Add(bullet2);
            }

            List<Enemy> enemyToDelete = new List<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.dead)
                {
                    enemy.Update(gameTime.ElapsedGameTime.Milliseconds, globalShift);
                    //if (bullet.position.X < 0 || bullet.position.Y < 0 || bullet.position.X > 480 || bullet.position.Y > 800) { bulletsToDelete.Add(bullet); }
                    if (enemy.alive && !(enemy.position.X < 0 || enemy.position.Y < 0 || enemy.position.X > 480 || enemy.position.Y > 800))
                    {
                        while (enemy.bullet_LastMS >= enemy.bullet_DelayMS)
                        {
                            enemy.bullet_LastMS = enemy.bullet_LastMS - enemy.bullet_DelayMS;
                            Bullet bullet1 = new Bullet(enemy.position, 0.15f, enemy.direction, 30.0f);
                            bullet1.Update(enemy.bullet_LastMS);
                            enemyBullets.Add(bullet1);
                        }
                    }
                }
                else
                {
                    enemyToDelete.Add(enemy);
                }
            }
            foreach (Enemy enemy in enemyToDelete)
            {
                enemies.Remove(enemy);
            }

            List<Bullet> enemyBulletsToDelete = new List<Bullet>();
            foreach (Bullet bullet in enemyBullets)
            {
                bullet.Update(gameTime.ElapsedGameTime.Milliseconds, globalShift);
                Figure f = bullet.getFigure();
                foreach (Bullet myBullet in myBullets)
                {
                    if (myBullet.alive)
                    {
                        if (f.Intersects(myBullet.getFigure()))
                        {
                            bullet.alive = false;
                            myBullet.alive = false;
                            break;
                        }
                    }
                }
                if (f.Intersects(shipF,true))
                {
                    bullet.alive = false;
                    alive = false;
                }
                if (!bullet.alive || bullet.position.X < 0 || bullet.position.Y < 0 || bullet.position.X > 480 || bullet.position.Y > 800) { enemyBulletsToDelete.Add(bullet); }
            }
            foreach (Bullet bullet in enemyBulletsToDelete)
            {
                enemyBullets.Remove(bullet);
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemy.alive)
                {
                    Figure f = enemy.getFigure();
                    foreach (Bullet myBullet in myBullets)
                    {
                        if (myBullet.alive)
                        {
                            if (f.Intersects(myBullet.getFigure()))
                            {
                                enemy.alive = false;
                                myBullet.alive = false;
                                break;
                            }
                        }
                    }
                    if (f.Intersects(shipF, true))
                    {
                        enemy.alive = false;
                        alive = false;
                    }
                }
            }

            Vector2 touchPosition = Vector2.Zero;
            /*if (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                {
                    touchPosition = new Vector2(gesture.Position.X, gesture.Position.Y);
                }
            }*/
            TouchCollection touchInput = TouchPanel.GetState();
            //look at all touch points (usually 1)
            bool pressed = false;
            this.pressed = false;
            foreach (TouchLocation touch in touchInput)
            {
                if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved) pressed = true;
                touchPosition = new Vector2(touch.Position.X, touch.Position.Y);
            }

            if (touchPosition != Vector2.Zero)
            {
                float y = origin.Y - touchPosition.Y;
                float x = touchPosition.X - origin.X;
                if (y*y + x*x > 120.0f * 120.0f) {
                    this.pressed = pressed;
                    if ((y > x * 2) && (y > -x * 2)) { currentRotation = 0.5f; }
                    else if ((y < x * 2) && (y > x / 2)) { currentRotation = 0.625f; }
                    else if ((y < x / 2) && (y > -x / 2)) { currentRotation = 0.75f; }
                    else if ((y < -x / 2) && (y > -x * 2)) { currentRotation = 0.875f; }
                    else if ((y < -x * 2) && (y < x * 2)) { currentRotation = 0.0f; }
                    else if ((y > x * 2) && (y < x / 2)) { currentRotation = 0.125f; }
                    else if ((y > x / 2) && (y < -x / 2)) { currentRotation = 0.25f; }
                    else if ((y > -x / 2) && (y < -x * 2)) { currentRotation = 0.375f; }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //currentRotation = currentRotation + (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            //spriteBatch.Draw(background, new Rectangle(0, 0, 480, 800), new Rectangle(background.Width / 2 - 240, background.Height / 2 - 400, 480, 800), Color.White);
            spriteBatch.Draw(background, new Rectangle(0, 0, 480, 800), new Rectangle((int)backgroundPosition.X, (int)backgroundPosition.Y, 480, 800), Color.White);
            spriteBatch.Draw(sky, new Rectangle(0, 0, 480, 800), new Rectangle((int)skyPosition.X, (int)skyPosition.Y, 480, 800), Color.White);
            
            foreach (Bullet bullet in myBullets)
            {
                if (bullet.alive) spriteBatch.Draw(bullet1, bullet.position, bullet1.Bounds, Color.White, bullet.direction * 2 * (float)Math.PI, new Vector2(bullet1.Width / 2, bullet1.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            }
            foreach (Bullet bullet in enemyBullets)
            {
                if (bullet.alive) spriteBatch.Draw(blueBullet, bullet.position, blueBullet.Bounds, Color.White, bullet.direction * 2 * (float)Math.PI, new Vector2(blueBullet.Width / 2, blueBullet.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.visible) spriteBatch.Draw(enemyRed, enemy.position, enemyRed.Bounds, Color.White, enemy.direction * 2 * (float)Math.PI, new Vector2(enemyRed.Width / 2, enemyRed.Height / 2), 1.0f, SpriteEffects.FlipVertically, 0.0f);
                if (!enemy.alive) spriteBatch.Draw(explode, enemy.position, new Rectangle(45 * (int) (enemy.animationPos % 4), 45 * (int) (enemy.animationPos / 4), 45, 45), Color.White, 0.0f, new Vector2(22.5f, 22.5f), 1.0f, SpriteEffects.None, 0.0f);
            }
            spriteBatch.Draw(ship, origin, ship.Bounds, Color.White, currentRotation*2*(float)Math.PI, new Vector2(ship.Width / 2, ship.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(control, origin, control.Bounds, Color.White, 0.0f, new Vector2(control.Width / 2, control.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            if (pressed) spriteBatch.Draw(controlPressed, origin, controlPressed.Bounds, Color.White, currentRotation * 2 * (float)Math.PI, new Vector2(controlPressed.Width / 2, controlPressed.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            
            //spriteBatch.Draw(explode, origin, explode.Bounds, Color.White, currentRotation*2*(float)Math.PI, new Vector2(explode.Width / 2, explode.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
