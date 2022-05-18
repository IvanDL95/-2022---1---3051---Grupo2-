#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Cameras
{
    public class StaticCamera : Camera
    {
        /// <summary>
        ///     Camera looking at a particular direction with an isometric view, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="position">The position of the camera.</param>
        /// <param name="frontDirection">The direction where the camera is pointing.</param>
        public StaticCamera(float aspectRatio, Vector3 position, Vector3 frontDirection) : base(
            aspectRatio)
        {
            Position = position;
            FrontDirection = frontDirection;
            UpDirection = DefaultWorldUpVector;
            BuildView();
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
        
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