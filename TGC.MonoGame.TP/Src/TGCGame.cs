using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.FinalEntities;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP
{
    internal class TGCGame : Game
    {
        internal static Content GameContent;
        internal static readonly PhysicSimulation PhysicsSimulation = new PhysicSimulation();

        private readonly GraphicsDeviceManager Graphics;

        private Camera Camera;
        private Vehicle Vehicle;
        private Floor Floor;
        private WoodenBox[] WoodenBox;
        private PowerUpBox[] PowerUpBox;
        //private Tub Tub;
        internal TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            base.Content.RootDirectory = "Content";
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

            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            WoodenBox = new WoodenBox[25];
            PowerUpBox = new PowerUpBox[5];
            Floor = new Floor(5000f);
            Vehicle = new Vehicle();
            //Tub = new Tub();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameContent = new Content(base.Content, GraphicsDevice);

            //Tub.Instantiate(Vector3.Zero);
            Vehicle.Instantiate(Vector3.Zero);
            Floor.Instantiate(Vector3.UnitY * -5);

            Vector3[] boxesPositions = {
                new Vector3(500f, 50f, -70f),
                new Vector3(1000f, 50f, 40f),
                new Vector3(-2100f, 50f, 30f),
                new Vector3(3970f, 50f, 1500f),
                new Vector3(4720f, 50f, 50f),
                new Vector3(1250f, 50f, 3200f),
                new Vector3(-4300f, 50f, 50f),
                new Vector3(-1570f, 50f, 1500),
                new Vector3(500f, 50f, -2550f),
                new Vector3(-1500f, 50f, 3750f),
                new Vector3(-540f, 50f, -4750f),
                new Vector3(-5700f, 50f, 250f),
                new Vector3(-2700f, 50f, -4450f)
            };

            for (int i = 0; i < 13; i++)
            {
                WoodenBox[i] = new WoodenBox();
                WoodenBox[i].Instantiate(boxesPositions[i]);
            }

            for (int i = 0; i < 5; i++)
            {
                PowerUpBox[i] = new PowerUpBox();
                PowerUpBox[i].Instantiate(new Vector3((i+1)*-400, 100f, (i+1)*600));
            }

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            Vehicle.Update(dTime, keyboardState);
            for (int i = 0; i < 5; i++)
                if(!PowerUpBox[i].Destroyed)
                    PowerUpBox[i].Update(dTime);  

            PhysicsSimulation.Update();

            Camera.Update(gameTime, Vehicle.World());

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GameContent.E_BasicShader.Parameters["ViewProjection"].SetValue(Camera.ViewProjection);
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            GraphicsDevice.Clear(Color.Black);

            Vehicle.Draw();
            Floor.Draw();
            //Tub.Draw();

            for (int i = 0; i < 13; i++)
                WoodenBox[i].Draw();

            for (int i = 0; i < 5; i++)
                if(!PowerUpBox[i].Destroyed)
                    PowerUpBox[i].Draw();
            
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            PhysicsSimulation.Dispose();
            base.Content.Unload();
            base.UnloadContent();
        }
    }
}