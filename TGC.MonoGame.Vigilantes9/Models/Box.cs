using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Vigilantes9.Geometries.Textures;

namespace TGC.MonoGame.Vigilantes9.Models
{
    public abstract class Box : DrawableGameComponent
    {
        #region Constants

        protected const float DefaultBoxSize = 50f;

        #endregion Constants

        public Box(Game game) : this(game, new BasicEffect(game.GraphicsDevice))
        {
        }

        public Box(Game game, Vector3 size) : this(game, new BasicEffect(game.GraphicsDevice), size)
        {
        }

        public Box(Game game, Effect effect) : this(game, effect, Vector3.One * DefaultBoxSize)
        {
        }

        public Box(Game game, Effect effect, Vector3 size) : base(game)
        {
            Size = size;
            Effect = effect.Clone();
            BoxPrimitive = new BoxPrimitive(Game.GraphicsDevice, Size);
        }

        public override void Initialize()
        {
            BoxPrimitive = new BoxPrimitive(Game.GraphicsDevice, Size);
        }

        public override void Draw(GameTime gameTime)
        {
            BoxPrimitive.Draw(Effect);
        }

        #region Properties

        public Vector3 Size { get; set; }
        public Effect Effect { get; set; }
        protected BoxPrimitive BoxPrimitive { get; set; }

        #endregion Properties
    }
}
