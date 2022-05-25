#region Using Statements

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.Vigilantes9.Cameras;
using TGC.MonoGame.Vigilantes9.Models;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9
{
    public sealed class Player : GameComponent
    {        
        public Player(Game game, Vehicle vehicle) : base(game)
        {
            Vehicle = vehicle;
        }

        #region Fields

        public Camera Camera;
        public Vehicle Vehicle;

        #endregion Fields

        public override void Initialize()
        {   
            Camera = new InGameCamera(Game.GraphicsDevice.Viewport.AspectRatio, this);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            base.Update(gameTime);
        }

        #region Properties

        public Matrix Perspective => Camera.View * Camera.Projection;
        public Matrix World => Vehicle.World;

        #endregion Properties
    }
}