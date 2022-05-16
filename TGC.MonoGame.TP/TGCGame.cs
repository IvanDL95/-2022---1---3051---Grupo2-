using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Scene;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    class Vehicle
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private Model Model { get; set; }
        private Effect Effect { get; set; }
        private Texture2D Texture { get; set; }
        public Matrix World { get; set; }
        private Matrix Scale { get; set; }
        private Matrix Rotation { get; set; }
        private Matrix Translation { get; set; }

        private float SpinningSensibility { get; set; } = 0f;
        private float MaxSpinSensibility { get; set; } = 3f;
        
        private float Acceleration { get; set; } = 9f;
        private float Friction { get; set; } = 5f;
        private float HorizontalVelocity { get; set; } = 0f;
        private float MaxHorizontalVelocity { get; set; } = 700f;
        private float MinHorizontalVelocity { get; set; } = -200f;

        public Vehicle(ContentManager content)
        {
            Model = content.Load<Model>(ContentFolder3D + "vehicles/Car/car");
            Effect = content.Load<Effect>(ContentFolderEffects + "CarShader");
            //Texture = content.Load<Texture2D>(ContentFolderTextures + "car/pallette");
            //Effect.Parameters["ModelTexture"].SetValue(Texture);

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            Scale = Matrix.CreateScale(1f);
            Rotation = Matrix.Identity;
            Translation = Matrix.Identity;
            World = Scale * Rotation * Translation;
        }

        public void Update(float dTime, KeyboardState keyboardState) {
            VelocityUpdate(dTime, keyboardState);
            SpinSensibilityUpdate();

            Rotation *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitY, YAxis(keyboardState) * SpinningSensibility * dTime));
            Translation *= Matrix.CreateTranslation(World.Forward * HorizontalVelocity * dTime);
            World = Scale * Rotation * Translation;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["ViewProjection"].SetValue(view * projection);
            //Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in Model.Meshes)
            {
                var worldMesh = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"].SetValue(worldMesh);
                mesh.Draw();
            }
        }

        //////////////////MOVEMENT//////////////////
        private float boolToFloat(bool boolean) => boolean ? 1 : 0;

        private float AccelerationAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.W)) - boolToFloat(keyboardState.IsKeyDown(Keys.S));

        private float YAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.A)) - boolToFloat(keyboardState.IsKeyDown(Keys.D));

        private void VelocityUpdate(float dTime, KeyboardState keyboardState) {
            if(HorizontalVelocity > 0)
                HorizontalVelocity = MathHelper.Clamp(HorizontalVelocity - Friction, 0, MaxHorizontalVelocity); 
            else
                HorizontalVelocity = MathHelper.Clamp(HorizontalVelocity + Friction, MinHorizontalVelocity, 0); 
            HorizontalVelocity = MathHelper.Clamp(HorizontalVelocity + AccelerationAxis(keyboardState) * Acceleration, MinHorizontalVelocity, MaxHorizontalVelocity); 
        }

        private void SpinSensibilityUpdate() {
            if(HorizontalVelocity == 0)
                SpinningSensibility = 0f;
            else
                SpinningSensibility = MathF.Abs(MaxSpinSensibility * (HorizontalVelocity / MaxHorizontalVelocity));
        }
    }

    ////////////////////////////////////////GAME/////////////////////////////////////////////////////////////

    public class TGCGame : Game
    {
        public const string ContentFolder = "Content";
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = ContentFolder;
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        public InGameCamera Camera { get; set; }
        private Vehicle Vehicle { get; set; }
        //private StateManager StateManager;
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

            /*
            Components.Add(Scene);
            Components.Add(StateManager = new StateManager(this));
            Services.AddService(typeof(IStateService), StateManager);
            */
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scene.Load();
            Vehicle = new Vehicle(Content);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            Camera.Update(gameTime, Vehicle.World);
            Vehicle.Update(dTime, keyboardState);

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