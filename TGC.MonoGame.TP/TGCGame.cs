using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.TP.Models;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Player;
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
        private Camera GameCamera { get; set; }
        private CombatVehicle PlayerVehicle { get; set; }
        private PlayerInput PlayerInput { get; set; }

        private Floor Floor { get; set; }

        private Effect TilingEffect { get; set; }

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
                Vector3.One * 1000f,
                -Vector3.Normalize(Vector3.One)
            );

            PlayerVehicle = new CombatVehicle(Content);
            PlayerInput = new PlayerInput(PlayerVehicle);
            Floor = new Floor(new QuadPrimitive(GraphicsDevice), 5000f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerVehicle.Load(Vector3.Zero);

            Floor.Load(Content.Load<Texture2D>(ContentFolderTextures + "floor/adoquin-2"));

            TilingEffect = Content.Load<Effect>(ContentFolderEffects + "TextureTiling");
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(600f, 600f));

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

            GameCamera.Update(gameTime);

            Debug.WriteLine("Car World forward: " + PlayerVehicle.World.Forward);
            Debug.WriteLine("Car Speed: " + PlayerVehicle.Speed);
            Debug.WriteLine("Car Rotation: " + PlayerVehicle.Rotation);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            PlayerVehicle.Draw(GameCamera.View, GameCamera.Projection);
            Floor.Draw(TilingEffect, GameCamera.View * GameCamera.Projection);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}