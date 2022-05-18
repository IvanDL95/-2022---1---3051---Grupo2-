#region Using Statements

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries.Textures;

#endregion Using Statements

namespace TGC.MonoGame.TP.Models.Boxes
{
    public class RegularBox : Box
    {
        public RegularBox(Game game, Matrix world) : this(
            game, new BasicEffect(game.GraphicsDevice), world)
        {
        }

        public RegularBox(Game game, Effect effect, Matrix world) : base(
            game, effect)
        {
            World = world;
        }

        public RegularBox(Game game, Effect effect, Vector3 size, Matrix world) : base(game, effect, size)
        {
            World = world;
        }

        #region Fields

        public Matrix World { get; set; }

        #endregion Fields
    }
}
