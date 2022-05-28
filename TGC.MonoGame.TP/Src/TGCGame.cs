using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.FinalEntities;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }
        private Camera Camera { get; set; }
        private Vehicle Vehicle;
        private Floor Floor;

        private WoodenBox[] WoodenBox { get; set; }

        
        internal static readonly PhysicSimulation PhysicsSimulation = new PhysicSimulation();

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

            Vehicle = new Vehicle();
            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            WoodenBox = new WoodenBox[25];
            Floor = new Floor(5000f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Vehicle.Load(Content);
            Floor.Load(Content);

            var cantCajas = 13;
            WoodenBox[0] = new WoodenBox(new Vector3(500f, 50f, -70f), 2f);
            WoodenBox[1] = new WoodenBox(new Vector3(1000f, 50f, 40f), 2f);
            WoodenBox[2] = new WoodenBox(new Vector3(-2100f, 50f, 30f), 2f);
            WoodenBox[3] = new WoodenBox(new Vector3(3970f, 50f, 1500f), 2f);
            WoodenBox[4] = new WoodenBox(new Vector3(4720f, 50f, 50f), 2f);
            WoodenBox[5] = new WoodenBox(new Vector3(1250f, 50f, 3200f), 2f);
            WoodenBox[6] = new WoodenBox(new Vector3(-4300f, 50f, 50f), 2f);
            WoodenBox[7] = new WoodenBox(new Vector3(-1570f, 50f, 1500f), 2f);
            WoodenBox[8] = new WoodenBox(new Vector3(500f, 50f, -2550f), 2f);
            WoodenBox[9] = new WoodenBox(new Vector3(-1500f, 50f, 3750f), 2f);
            WoodenBox[10] = new WoodenBox(new Vector3(-540f, 50f, -4750f), 2f);
            WoodenBox[11] = new WoodenBox(new Vector3(-5700f, 50f, 250f), 2f);
            WoodenBox[12] = new WoodenBox(new Vector3(-2700f, 50f, -4450f), 2f);
            
            for (int i = 0; i < cantCajas; i++)
                WoodenBox[i].Load(Content);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            Vehicle.Update(dTime, keyboardState);
            Camera.Update(gameTime, Vehicle.World);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.Black);
            Vehicle.Draw(dTime, Camera.View, Camera.Projection);
            Floor.Draw(Camera.View, Camera.Projection);

              for (int i = 0; i < 13; i++)
                WoodenBox[i].Draw(Camera.View, Camera.Projection);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}