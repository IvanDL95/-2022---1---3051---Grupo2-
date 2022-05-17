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
        private GraphicsDeviceManager Graphics { get; }
        private InGameCamera Camera { get; set; }
        private Vehicle Vehicle { get; set; }
        private GameScene Scene { get; set; }
        private BoundingBox[] Colliders { get; set; }

        private Box Box { get; set; }

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = TGCContent.ContentFolder;
            IsMouseVisible = true;
        }

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
            Scene = new GameScene();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scene.Load(this);
            Vehicle = new Vehicle(Content);
            Box = new Box(Content, new Vector3(500f, 50f, 20f), 2f);

            Colliders = new BoundingBox[1];//Scene.Colliders;
            Colliders[0] = Box.Collider;

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
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.Black);
            Vehicle.Draw(dTime, Camera.View, Camera.Projection);
            Scene.Draw(gameTime, Camera);
            Box.Draw(Camera.View, Camera.Projection);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}