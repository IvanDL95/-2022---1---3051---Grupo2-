#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Cameras
{
    public class IsometricCamera : StaticCamera
    {
        /// <summary>
        ///     Camera looking at a particular direction with an isometric view, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="position">The position of the camera.</param>
        /// <param name="frontDirection">The direction where the camera is pointing.</param>
        public IsometricCamera(float aspectRatio, float distance) : base(
            aspectRatio, Vector3.One * distance, -Vector3.Normalize(Vector3.One))
        {
            //
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
        
        }
    }
}