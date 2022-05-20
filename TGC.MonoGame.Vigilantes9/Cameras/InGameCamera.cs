using Microsoft.Xna.Framework;
using System;
using TGC.MonoGame.Vigilantes9.Models;

namespace TGC.MonoGame.Vigilantes9.Cameras
{
    public class InGameCamera : Camera
    {
        private const float AxisDistanceToTarget = 1000f;

        private const float AngleFollowSpeed = 0.015f;

        private const float AngleThreshold = 0.85f;
        
        private Vector3 CurrentRightVector { get; set; } = Vector3.Right;

        private float RightVectorInterpolator { get; set; } = 0f;

        private Vector3 PastRightVector { get; set; } = Vector3.Right;

        private Player FollowedPlayer;

        /// <summary>
        ///     Camera looking at a particular direction with an isometric view, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="followedPlayer">The player to follow. Pass as reference.</param>
        public InGameCamera(float aspectRatio, Player followedPlayer) : base(
            aspectRatio, DefaultNearPlaneDistance, 100000f, MathF.PI / 3f
        )
        {
            FollowedPlayer = followedPlayer;
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            var followedPosition = FollowedPlayer.World.Translation;
            var followedRight = -FollowedPlayer.World.Forward;

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

            FrontDirection = (followedPosition - offsetedPosition);
            FrontDirection.Normalize();

            RightDirection = Vector3.Cross(FrontDirection, Vector3.Up);
            UpDirection = Vector3.Cross(RightDirection, FrontDirection);

            BuildView(offsetedPosition, followedPosition);
        }
    }
}
