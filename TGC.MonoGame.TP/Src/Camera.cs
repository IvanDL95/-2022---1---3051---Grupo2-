using Microsoft.Xna.Framework;
using System;

namespace TGC.MonoGame.TP
{
    public class Camera 
    {
        private const float AxisDistanceToTarget = 1000f;

        private const float AngleFollowSpeed = 0.015f;

        private const float AngleThreshold = 0.85f;

        private Matrix Projection;

        private Matrix View;

        internal Matrix ViewProjection;
        
        private Vector3 CurrentRightVector= Vector3.Right;

        private float RightVectorInterpolator = 0f;

        private Vector3 PastRightVector = Vector3.Right;

        public Camera(float aspectRatio)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
        }

        public void Update(GameTime gameTime, Matrix FollowedWorld)
        {

            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            var followedPosition = FollowedWorld.Translation;
            var followedRight = -FollowedWorld.Forward;

            if (Vector3.Dot(followedRight, PastRightVector) > AngleThreshold)
            {
                RightVectorInterpolator += elapsedTime * AngleFollowSpeed;
                RightVectorInterpolator = MathF.Min(RightVectorInterpolator, 1f);
                CurrentRightVector = Vector3.Lerp(CurrentRightVector, followedRight, RightVectorInterpolator * RightVectorInterpolator);
            }
            else
                RightVectorInterpolator = 0f;

            PastRightVector = followedRight;

            var offsetedPosition = followedPosition 
                + CurrentRightVector * AxisDistanceToTarget
                + Vector3.Up * AxisDistanceToTarget;

            var forward = (followedPosition - offsetedPosition);
            forward.Normalize();

            var right = Vector3.Cross(forward, Vector3.Up);
            var cameraCorrectUp = Vector3.Cross(right, forward);

            View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);

            ViewProjection = View * Projection;
        }
    }
}