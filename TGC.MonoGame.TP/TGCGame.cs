using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Scene;

using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        private BoundingBox[] Colliders { get; set; }

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = TGCContent.ContentFolder;
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        public InGameCamera Camera { get; set; }
        private Vehicle Vehicle { get; set; }
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

            Camera = new InGameCamera(GraphicsDevice.Viewport.AspectRatio);
            Scene = new GameScene(this, GraphicsDevice);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scene.Load();
            Vehicle = new Vehicle(Content);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            Colliders = new BoundingBox[0];

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            Camera.Update(gameTime, Vehicle.World);
            Vehicle.Update(dTime, Colliders, keyboardState);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Vehicle.Draw(Camera.View, Camera.Projection);
            Scene.Draw(gameTime, Camera);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}