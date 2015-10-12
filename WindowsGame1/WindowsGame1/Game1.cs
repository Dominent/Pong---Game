#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion
namespace PONG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont georgia;
           

        //EndGame
        int loose = 0;
        int win = 0;
                
        //Game World
        Texture2D ballTexture;
        Rectangle ballRectangle;
        int ballCenter;

        int ballWidth = 30;
        int ballHeight = 30;

        Texture2D playerBattTexture;
        Rectangle playerBattRectangle;

        Texture2D enemyBattTexture;
        Rectangle enemyBattRectangle;
        int enemyCenter;

        Vector2 velocity;
        Vector2 scoreP1Pos;
        Vector2 scoreP2Pos;

        Random myRandom = new Random();

        //Screen Parameteres
        int screenWidth;
        int screenHeight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("Sprites/aqua-ball");
            ballRectangle = new Rectangle(300, 300, ballWidth, ballHeight);

            playerBattTexture = Content.Load<Texture2D>("Sprites/Batt");
            playerBattRectangle = new Rectangle(350, 450, 100, 22);

            enemyBattTexture = Content.Load<Texture2D>("Sprites/EnemyBatt");
            enemyBattRectangle = new Rectangle(350, 10, 100, 22);

            //Fonts
            georgia = Content.Load<SpriteFont>("Sprites/georgia");


            //Text possition
            scoreP1Pos.X = 10;
            scoreP1Pos.Y = 50;
            scoreP2Pos.X = 670;
            scoreP2Pos.Y = 50;

            //Ball Velocity
            velocity.X = 3f;
            velocity.Y = 3f;

            RandomLoad();

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Bat Movement
            #region BatMovement
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                playerBattRectangle.X -= 6;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                playerBattRectangle.X += 6;

            //Bat and Ball Interact
            if (ballRectangle.Intersects(playerBattRectangle))
                velocity.Y = -velocity.Y;

            #endregion

            ballRectangle.X = ballRectangle.X + (int)velocity.X;
            ballRectangle.Y = ballRectangle.Y + (int)velocity.Y;

            if(Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ballRectangle.X = (screenWidth / 2) - (ballWidth / 2);
                ballRectangle.Y = (screenHeight / 2) - (ballHeight / 2);
                RandomLoad();
            }

            if (ballRectangle.X <= 0) velocity.X = -velocity.X;

            if (ballRectangle.X + ballWidth >= screenWidth)
                velocity.X = -velocity.X;

           
            //Centers
            ballCenter = (ballRectangle.X + (ballWidth/2));
            enemyCenter = enemyBattRectangle.X + 50;

            EnemyMovement();

            EndGame();

            base.Update(gameTime);
        }
        
        void RandomLoad()
        {
            int random = myRandom.Next(0, 4);

            //Down - Rgiht
            if(random == 0) 
            {
                velocity.X = 3f;
                velocity.Y = 3f;
            }
            //Down - Left
            if(random == 1) 
            {
                velocity.X = -3f;
                velocity.Y = 3f;
            }
            //Up - Left
            if(random == 2) 
            {
                velocity.X = -3f;
                velocity.Y = -3f;
            }
            //Up - Right
            if(random == 3) 
            {
                velocity.X = 3f;
                velocity.Y = -3f;
            }
        }

        void EnemyMovement()
        {
            if(ballCenter >enemyCenter)
            enemyBattRectangle.X += 3;
            if(ballCenter < enemyCenter)
            enemyBattRectangle.X -=3;

            if (ballRectangle.Intersects(enemyBattRectangle))
                velocity.Y = - velocity.Y;
        }

        void EndGame()
        {
            if (ballRectangle.Y + ballHeight >= screenHeight)
            {
                ++loose;
                ballRectangle.X = (screenWidth / 2) - (ballWidth / 2);
                ballRectangle.Y = (screenHeight / 2) - (ballHeight / 2);
                RandomLoad();
            }
            else if (ballRectangle.Y <= 0)
            {
                ++win;
                ballRectangle.X = (screenWidth / 2) - (ballWidth / 2);
                ballRectangle.Y = (screenHeight / 2) - (ballHeight / 2);
                RandomLoad();

            }
            if (loose == 3 || win == 3) this.Exit();
        } //Fully Implemented

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(ballTexture, ballRectangle, Color.White);
            spriteBatch.Draw(playerBattTexture, playerBattRectangle, Color.White);
            spriteBatch.Draw(enemyBattTexture, enemyBattRectangle, Color.White);
            //Draw Scores
            spriteBatch.DrawString(georgia, "Player : " + win.ToString(), scoreP1Pos, Color.Green);
            spriteBatch.DrawString(georgia, "Computer : " + loose.ToString(), scoreP2Pos, Color.Red);

            spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
