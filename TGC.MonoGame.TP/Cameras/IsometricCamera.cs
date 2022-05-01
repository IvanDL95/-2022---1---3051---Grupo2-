#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.TP.Cameras
{
    public class IsometricCamera : Camera
    {
        /// <summary>
        ///     Static camera looking at a particular direction, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="position">The position of the camera.</param>
        /// <param name="frontDirection">The direction where the camera is pointing.</param>
        /// <param name="upDirection">The direction that is "up" from the camera's point of view.</param>
        public IsometricCamera(float aspectRatio, Vector3 position, Vector3 frontDirection, Vector3 upDirection) : base(
            aspectRatio)
        {
            Position = position;
            FrontDirection = frontDirection;
            UpDirection = upDirection;
            BuildView();
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            // TODO - Follow car movement
        }

        #region Fields

        /// <summary>
        ///     Value with which the camera is going to move.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        ///     Value with which the camera is going to move the angle.
        /// </summary>
        public float Angle { get; set; }

        #endregion Fields
    }
}