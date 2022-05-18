using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Scene;
using TGC.MonoGame.TP.Models;
using TGC.MonoGame.TP.Models.Vehicles;

using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }
        private InGameCamera Camera { get; set; }
        private Vehicle Vehicle;
        private GameScene Scene { get; set; }
        private BoundingBox[] Colliders { get; set; }

        private Box[] Box { get; set; }

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

            Scene = new GameScene();
            Vehicle = new SimpleVehicle(this);
            Camera = new InGameCamera(GraphicsDevice.Viewport.AspectRatio, ref Vehicle);
            Box = new Box[25];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scene.Load(this);
            Vehicle.LoadContent();

            Box[0] = new Box(Content, new Vector3(500f, 50f, -70f), 2f);
            Box[1] = new Box(Content, new Vector3(1000f, 50f, 40f), 2f);
            Box[2] = new Box(Content, new Vector3(-2100f, 50f, 30f), 2f);
            Box[3] = new Box(Content, new Vector3(3970f, 50f, 1500f), 2f);
            Box[4] = new Box(Content, new Vector3(4720f, 50f, 50f), 2f);
            Box[5] = new Box(Content, new Vector3(1250f, 50f, 3200f), 2f);
            Box[6] = new Box(Content, new Vector3(-4300f, 50f, 50f), 2f);
            Box[7] = new Box(Content, new Vector3(-1570f, 50f, 1500f), 2f);
            Box[8] = new Box(Content, new Vector3(500f, 50f, -2550f), 2f);
            Box[9] = new Box(Content, new Vector3(-1500f, 50f, 3750f), 2f);
            Box[10] = new Box(Content, new Vector3(-540f, 50f, -4750f), 2f);
            Box[11] = new Box(Content, new Vector3(-5700f, 50f, 250f), 2f);
            Box[12] = new Box(Content, new Vector3(-2700f, 50f, -4450f), 2f);
            Box[13] = new Box(Content, new Vector3(570f, 50f, -415f), 2f);
            Box[14] = new Box(Content, new Vector3(90f, 50f, 1220f), 2f);
            Box[15] = new Box(Content, new Vector3(110f, 50f, -4820f), 2f);
            Box[16] = new Box(Content, new Vector3(20f, 50f, 3520f), 2f);
            Box[17] = new Box(Content, new Vector3(-500f, 50f, 90f), 2f);
            Box[18] = new Box(Content, new Vector3(400f, 50f, 780f), 2f);
            Box[19] = new Box(Content, new Vector3(-3517f, 50f, -2120f), 2f);
            Box[20] = new Box(Content, new Vector3(3917f, 50f, 520f), 2f);
            Box[21] = new Box(Content, new Vector3(-3517f, 50f, 420f), 2f);
            Box[22] = new Box(Content, new Vector3(-4000f, 50f, -2340f), 2f);
            Box[23] = new Box(Content, new Vector3(3517f, 50f, 3020f), 2f);
            Box[24] = new Box(Content, new Vector3(-3517f, 50f, 4220f), 2f);


            Colliders = new BoundingBox[25];//Scene.Colliders;
            Colliders[0] = Box[0].Collider;
            Colliders[1] = Box[1].Collider;
            Colliders[2] = Box[2].Collider;
            Colliders[3] = Box[3].Collider;
            Colliders[4] = Box[4].Collider;
            Colliders[5] = Box[5].Collider;
            Colliders[6] = Box[6].Collider;
            Colliders[7] = Box[7].Collider;
            Colliders[8] = Box[8].Collider;
            Colliders[9] = Box[9].Collider;
            Colliders[10] =Box[10].Collider;
            Colliders[11] =Box[11].Collider;
            Colliders[12] =Box[12].Collider;
            Colliders[13] =Box[13].Collider;
            Colliders[14] =Box[14].Collider;
            Colliders[15] =Box[15].Collider;
            Colliders[16] =Box[16].Collider;
            Colliders[17] =Box[17].Collider;
            Colliders[18] =Box[18].Collider;
            Colliders[19] =Box[19].Collider;
            Colliders[20] =Box[20].Collider;
            Colliders[21] =Box[21].Collider;
            Colliders[22] =Box[22].Collider;
            Colliders[23] =Box[23].Collider;
            Colliders[24] =Box[24].Collider;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            Camera.Update(gameTime);
            Vehicle.Update(dTime, Colliders, keyboardState);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.Black);
            Vehicle.Draw(dTime, Camera.View, Camera.Projection);
            Scene.Draw(gameTime, Camera);

            foreach (var box in Box)
            {
                box.Draw(Camera.View, Camera.Projection);
            }

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}