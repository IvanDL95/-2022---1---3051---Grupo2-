using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.Vigilantes9.Cameras;
using TGC.MonoGame.Vigilantes9.Scene;
using TGC.MonoGame.Vigilantes9.Models;
using TGC.MonoGame.Vigilantes9.Models.Vehicles;
using TGC.MonoGame.Vigilantes9.Gameplay;

using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Vigilantes9
{
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }
        private Player Player;
        private Level CurrentLevel;

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

            Player = new Player(this, new SimpleVehicle(this));
            CurrentLevel = new Levels.SimpleLevel(this, Player);

            Components.Add(Player);
            Components.Add(CurrentLevel);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Player.Vehicle.LoadContent();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Player.Vehicle.Update(dTime, CurrentLevel.Colliders.ToArray(), keyboardState);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Player.Vehicle.Draw(dTime, Player.Camera.View, Player.Camera.Projection);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Components.Clear();
            Content.Unload();
            base.UnloadContent();
        }
    }
}