#region Using Statements

using Microsoft.Xna.Framework;

#endregion Using Statements

namespace TGC.MonoGame.TP.Cameras
{
    public struct Frustum
    {
        public Frustum(float fieldOfView, float aspectRatio, float nearPlane, float farPlane)
        {
            FieldOfView = fieldOfView;
            AspectRatio = aspectRatio;
            NearPlane = nearPlane;
            FarPlane = farPlane;
        }

        /// <summary>
        ///     Field of view in the y direction, in radians.
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        ///     Aspect ratio, defined as view space width divided by height.
        /// </summary>
        public float AspectRatio { get; set; }

        /// <summary>
        ///     Distance to the near view plane.
        /// </summary>
        public float NearPlane { get; set; }

        /// <summary>
        ///     Distance to the far view plane.
        /// </summary>
        public float FarPlane { get; set; }   
    }

    public abstract class Camera
    {
        #region Constants

        public const float DefaultFieldOfView = MathHelper.PiOver4;
        public const float DefaultNearPlaneDistance = 0.1f;
        public const float DefaultFarPlaneDistance = 3000;

        #endregion Constants

        public Camera(Frustum frustum) : this(frustum.AspectRatio, frustum.NearPlane, frustum.FarPlane, frustum.FieldOfView)
        {
        }

        public Camera(
            float aspectRatio,
            float nearPlaneDistance = DefaultNearPlaneDistance,
            float farPlaneDistance = DefaultFarPlaneDistance
        ) : this(aspectRatio, nearPlaneDistance, farPlaneDistance, DefaultFieldOfView)
        {
        }

        public Camera(float aspectRatio, float nearPlaneDistance, float farPlaneDistance, float fieldOfViewDegrees)
        {
            ProjectionFrustum = new Frustum(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
            BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
        }

        /// <summary>
        ///     The direction that is "up" from the camera's point of view.
        /// </summary>
        public readonly Vector3 DefaultWorldUpVector = Vector3.Up;

        /// <summary>
        ///     Forward direction of the camera.
        /// </summary>
        public readonly Vector3 DefaultWorldFrontVector = Vector3.Forward;

        /// <summary>
        ///     Build a perspective projection matrix based on a field of view, aspect ratio, and near and far view plane
        ///     distances.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio, defined as view space width divided by height.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <param name="fieldOfView">The field of view in the y direction, in degrees.</param>
        public void BuildProjection(
            float aspectRatio,
            float nearPlaneDistance,
            float farPlaneDistance,
            float fieldOfView
        )
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView,
                aspectRatio,
                nearPlaneDistance,
                farPlaneDistance
            );
        }

        /// <summary>
        ///     Build the camera View matrix using its properties.
        /// </summary>
        public void BuildView(Vector3 position, Vector3 target)
        {
            View = Matrix.CreateLookAt(position, target, UpDirection);
        }

        /// <summary>
        ///     Build the camera View matrix using its properties.
        /// </summary>
        public void BuildView()
        {
            BuildView(Position, Position + FrontDirection);
        }

        /// <summary>
        ///     Allows updating the internal state of the camera if this method is overwritten.
        ///     By default it does not perform any action.
        /// </summary>
        /// <param name="gameTime">Holds the time state of a <see cref="Game" />.</param>
        public abstract void Update(GameTime gameTime);

        #region Fields

        /// <summary>
        ///     The created view matrix.
        /// </summary>
        public Matrix View { get; set; }
        
        /// <summary>
        ///     Position where the camera is located.
        /// </summary>
        public Vector3 Position { get; set; }

        // TODO - Reducir este boilerplate de direcciones con Vector3 de direccones
        // public Vector3 TargetDirection;

        /// <summary>
        ///     Direction where the camera is looking.
        /// </summary>
        public Vector3 FrontDirection { get; set; }

        /// <summary>
        ///     Represents the positive x-axis of the camera space.
        /// </summary>
        public Vector3 DirectionX { get; set; }

        /// <summary>
        ///     Vector up direction (may differ if the camera is reversed).
        /// </summary>
        public Vector3 UpDirection { get; set; }

        /// <summary>
        ///     The perspective projection matrix.
        /// </summary>
        public Matrix Projection { get; set; }

        /// <summary>
        ///     Frustum of the camera.
        /// </summary>
        public Frustum ProjectionFrustum { get; set; }

        #endregion Fields
    }
}