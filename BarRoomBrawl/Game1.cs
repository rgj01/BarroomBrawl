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

namespace BarRoomBrawl
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Server Server;


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
            // TODO: Add your initialization logic here

            m_map = new Map(this);
            m_player = new Player(this, "Player", Vector2.Zero, 0.0f, GameObject.Directions.E, 0);
            m_camera = new Camera(Vector2.Zero, new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2));
            m_state = new GameState();
            Server = new Server();
            Server.Start(4444);
            base.Initialize();
        }

        Map m_map;
        Player m_player;
        Camera m_camera;
        GameState m_state;


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            m_map.LoadContent();
            m_player.LoadContent();
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
            KeyboardState state = Keyboard.GetState();
            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            m_player.Speed = 0.2f;

            if (state.IsKeyDown(Keys.D))
            {
                if (state.IsKeyDown(Keys.W))
                {
                    m_player.Direction = GameObject.Directions.SE;
                }
                else if (state.IsKeyDown(Keys.S))
                {
                    m_player.Direction = GameObject.Directions.NE;
                }
                else
                {
                    m_player.Direction = GameObject.Directions.E;
                }
            }
            else if (state.IsKeyDown(Keys.A))
            {
                if (state.IsKeyDown(Keys.W))
                {
                    m_player.Direction = GameObject.Directions.SW;
                }
                else if (state.IsKeyDown(Keys.S))
                {
                    m_player.Direction = GameObject.Directions.NW;
                }
                else
                {
                    m_player.Direction = GameObject.Directions.W;
                }
            }
            else if (state.IsKeyDown(Keys.W))
            {
                m_player.Direction = GameObject.Directions.S;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                m_player.Direction = GameObject.Directions.N;
            }
            else
            {
                m_player.Speed = 0.0f;
            }

            m_player.Update(gameTime);
            m_camera.Update(m_player.Location);
            m_state.Update();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, m_camera.TransformMatrix);
            m_map.Draw(spriteBatch);
            foreach (GameObject obj in m_state.GameObjects)
            {
                obj.Draw(spriteBatch);
            }
            m_player.Draw(spriteBatch);
            
            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
