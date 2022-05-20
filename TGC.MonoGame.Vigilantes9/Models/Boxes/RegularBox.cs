#region Using Statements

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Vigilantes9.Geometries.Textures;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Models.Boxes
{
    public class RegularBox : BoxModel
    {
        #region Constants

        protected const float DefaultBoxSize = 2f;

        #endregion Constants

        public RegularBox(Vector3 position) : this(position, DefaultBoxSize)
        {
        }

        public RegularBox(Vector3 position, float scale) : base(position, Vector3.One * scale)
        {
            // World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        }

    }
}
