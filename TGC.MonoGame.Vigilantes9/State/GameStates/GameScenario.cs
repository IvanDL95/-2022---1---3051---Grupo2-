using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using TGC.MonoGame.Vigilantes9;
using TGC.MonoGame.Vigilantes9.Cameras;

namespace TGC.MonoGame.Vigilantes9.State
{
    public class GameScenario : GameState
    {
        #region Constants

        protected const float CameraDistance = 1000f;

        #endregion Constants

        public Camera Camera { get; protected set; }

        public GameScenario(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            Camera = new IsometricCamera(
                Game.GraphicsDevice.Viewport.AspectRatio,
                CameraDistance
            );

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
            }
        }
    }
}