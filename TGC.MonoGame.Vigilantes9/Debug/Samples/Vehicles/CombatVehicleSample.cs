using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.Vigilantes9.Cameras;
using TGC.MonoGame.Vigilantes9.Models;
using TGC.MonoGame.Vigilantes9.Geometries.Textures;
using TGC.MonoGame.Vigilantes9.Models.Vehicles;

namespace TGC.MonoGame.Vigilantes9.Debug.Samples.Vehicles
{
    public class CombatVehicleSample : TGCSample
    {
        /// <inheritdoc />
        public CombatVehicleSample(TGCDebug game) : base(game)
        {
            Category = TGCSampleCategory.Vehicle;
            Name = "Combat Vehicle";
            Description =
                "The vehicle the player will use.";
        }

        // The Model of the Vehicle to draw
        private VehicleModel Model { get; set; }

        // The Camera
        private Camera Camera { get; set; }

        private QuadPrimitive Quad { get; set; }
        private Effect TillingEffect { get; set; }
        

        public override void Initialize()
        {
            Model = new Models.Vehicles.CombatVehicle(0f,0f,0f);
            Camera = new IsometricCamera(Game.GraphicsDevice.Viewport.AspectRatio, 500f);
            Quad = new QuadPrimitive(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model.Load(Game.Content);
            
            var cobbleTexture = Game.Content.Load<Texture2D>(TGCContent.ContentFolderTextures + "floor/stones");
            TillingEffect = Game.Content.Load<Effect>(TGCContent.ContentFolderEffects + "TextureTiling");
            TillingEffect.Parameters["Texture"].SetValue(cobbleTexture);
            TillingEffect.Parameters["Tiling"].SetValue(new Vector2(20f, 20f));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Game.CurrentKeyboardState;

            var rotationY = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) *
                (Convert.ToSingle(keyboardState.IsKeyDown(Keys.Left)) - Convert.ToSingle(keyboardState.IsKeyDown(Keys.Right)));

            var rotationAcceleration = 0f;
            
            Model.World *= Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(rotationY, rotationAcceleration, 0f));
            // Model.WheelsRotation = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            Model.Update(keyboardState);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.Background = Color.CornflowerBlue;

            var world = Matrix.CreateScale(1000f);
            TillingEffect.Parameters["World"].SetValue(world);
            TillingEffect.Parameters["WorldViewProjection"].SetValue(world * Camera.View * Camera.Projection);
            Quad.Draw(TillingEffect);

            Model.Draw(Camera.View * Camera.Projection);
            base.Draw(gameTime);
        }

    }
}