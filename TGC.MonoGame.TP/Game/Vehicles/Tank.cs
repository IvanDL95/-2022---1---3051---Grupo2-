using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Models;

namespace TGC.MonoGame.TP.TGCGameGame
{
    public class Tank : DrawableGameComponent
    {
        public const string VehiclesFolder = "vehicles/";

        private Camera Camera { get; set; }
        private TankModel TankModel { get; set; }
        private Matrix TankWorld { get; set; }

        /// <summary>
        ///     The viewer where the example is shown.
        /// </summary>
        protected TGCGame BaseGame { get; }

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="game">The game.</param>
        public Tank(TGCGame game) : base(game)
        {
            BaseGame = game;
        }

        /// <summary>
        ///     Initialize the game settings here.
        /// </summary>
        public override void Initialize()
        {
            Camera = new IsometricCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 250f, -Vector3.Normalize(Vector3.One), Vector3.Up);
            LoadContent();
            base.Initialize();
        }

        /// <summary>
        ///     Load all content here.
        /// </summary>
        protected override void LoadContent()
        {
            // Load the tank model from the ContentManager.
            TankModel = new TankModel();
            var model = Game.Content.Load<Model>(TGCGame.ContentFolder3D + VehiclesFolder + "Tank/tank");
            TankModel.Load(model);

            TankWorld = Matrix.Identity;
    
            base.LoadContent();
        }

        /// <summary>
        ///     Updates the game.
        /// </summary>
        /// <param name="gameTime">Holds the time state of a <see cref="Game" />.</param>
        public override void Update(GameTime gameTime)
        {
            var time = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            // Update the animation properties on the tank object. In a real game you would probably take this data from user inputs
            // or the physics system, rather than just making everything rotate like this!
            TankModel.WheelRotation = time * 5;
            TankModel.SteerRotation = (float) Math.Sin(time * 0.75f) * 0.5f;
            TankModel.TurretRotation = (float) Math.Sin(time * 0.333f) * 1.25f;
            TankModel.CannonRotation = (float) Math.Sin(time * 0.25f) * 0.333f - 0.333f;
            TankModel.HatchRotation = MathHelper.Clamp((float) Math.Sin(time * 2) * 2, -1, 0);

            /* Updates the View and Projection matrices. */
            // Game.Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);

            base.Update(gameTime);
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            // BaseGame.Background = Color.CornflowerBlue;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Calculate the camera matrices.
            var time = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            // Draw the tank model.
            TankModel.Draw(TankWorld * Matrix.CreateRotationY(time * 0.1f), Camera.View, Camera.Projection);

            base.Draw(gameTime);
        }
    }
}