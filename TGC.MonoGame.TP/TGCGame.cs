using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.TP.Models;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.State;
using TGC.MonoGame.TP.Scene;
using TGC.MonoGame.TP.Geometries.Textures;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const string ContentFolder = "Content";

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public const int CarsCount = 1;

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = ContentFolder;
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        public Camera GameCamera { get; set; }
        private CombatVehicle PlayerVehicle { get; set; }
        private PlayerInput PlayerInput { get; set; }
        private StateManager StateManager;

        private GameScene Scene { get; set; }

        protected override void Initialize()
        {

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            GameCamera = new IsometricCamera(
                GraphicsDevice.Viewport.AspectRatio,
                1000f
            );

            PlayerVehicle = new CombatVehicle(Content);
            PlayerInput = new PlayerInput(PlayerVehicle);

            Components.Add(new GameScene(this, GameCamera));

            Components.Add(StateManager = new StateManager(this));
            Services.AddService(typeof(IStateService), StateManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerVehicle.Load(Vector3.Zero);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Escape))
                Exit();
            
            var time = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            PlayerInput.Update(elapsedTime, kbState);
            PlayerVehicle.Update(elapsedTime);

            // Camera gameCamera = Services.GetService<Camera>();
            GameCamera.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            PlayerVehicle.Draw(GameCamera.View, GameCamera.Projection);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}