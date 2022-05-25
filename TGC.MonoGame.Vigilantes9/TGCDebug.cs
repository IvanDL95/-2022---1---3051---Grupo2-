using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.Vigilantes9.Models;
using TGC.MonoGame.Vigilantes9.Models.Vehicles;
using TGC.MonoGame.Vigilantes9.Gameplay;
using TGC.MonoGame.Vigilantes9.Debug;

using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Vigilantes9
{
    public class TGCDebug : Game
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="TGCDebug" /> class.
        ///     The main game constructor is used to initialize the starting variables.
        /// </summary>
        public TGCDebug()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = TGCContent.ContentFolder;
            IsMouseVisible = true;

            // Gizmos = new Gizmos.Gizmos();
    
            Model = new DebugtViewer(this);
            Model.LoadTreeSamples();
        }

        /// <summary>
        ///     Handles the configuration and management of the graphics device.
        /// </summary>
        public GraphicsDeviceManager Graphics { get; }

        /// <summary>
        ///     Gizmos are used to debug and visualize boundaries and vectors
        /// </summary>
        // public Gizmos.Gizmos Gizmos { get; }

        /// <summary>
        ///     Enables a group of sprites to be drawn using the same settings.
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        ///     Represents a state of keystrokes recorded by a keyboard input device.
        /// </summary>
        public KeyboardState CurrentKeyboardState { get; set; }

        /// <summary>
        ///     Represents the state of the Mouse input device.
        /// </summary>
        public MouseState CurrentMouseState { get; set; }

        /// <summary>
        ///     Sample background color.
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        ///     The model has the logic for the creation of the sample explorer.
        /// </summary>
        private DebugtViewer Model { get; }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 75;
            Graphics.ApplyChanges();

            // Components.Add(Player);
            Model.LoadWelcomeSample();

            Background = Color.CornflowerBlue;

            base.Initialize();
        }
        
        /// <summary>
        ///     This method is used to load your game content.
        ///     It is called only once per game, after Initialize method, but before the main game loop methods.
        /// </summary>
        protected override void LoadContent()
        {
            //TODO: use this.Content to load your game content here
            // Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, ContentFolder));

            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }
        /// <summary>
        ///     This method is called multiple times per second, and is used to update your game state (updating the world,
        ///     checking for collisions, gathering input, playing audio, etc.).
        /// </summary>
        /// <param name="gameTime">Holds the time state of a <see cref="Game" />.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            HandleInput();

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        ///     Similar to the Update method, it is also called multiple times per second.
        /// </summary>
        /// <param name="gameTime">Holds the time state of a <see cref="Game" />.</param>
        protected override void Draw(GameTime gameTime)
        {
            //TODO: Add your drawing code here

            GraphicsDevice.Clear(Background);

            base.Draw(gameTime);

            // Gizmos.Draw();
        }

        int sampleChangedTicks = 0;
        /// <summary>
        ///     Handles input for quitting the game.
        /// </summary>
        private void HandleInput()
        {
            CurrentKeyboardState = Keyboard.GetState();

            CurrentMouseState = Mouse.GetState();

            // Check for exit.
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape)) Exit();

            bool changeSample = CurrentKeyboardState.IsKeyDown(Keys.Tab);
            if(CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                if (changeSample && sampleChangedTicks <= 0){
                    Model.LoadPreviousSample();
                    sampleChangedTicks = 18;
                }
            } else 
            {
                if (changeSample && sampleChangedTicks <= 0){
                    Model.LoadNextSample();
                    sampleChangedTicks = 18;
                }
            }
            if(sampleChangedTicks > 0) sampleChangedTicks--;
        }
    }
}