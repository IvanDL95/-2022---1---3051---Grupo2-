using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Drawers;

using TGC.MonoGame.TP.Physics;
using TGC.MonoGame.TP.PhysicsEntities;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP.FinalEntities
{
    internal class Vehicle: DynamicEntity
    {
        protected override Drawer Drawer() => TGCGame.GameContent.D_Vehicle;
        internal Vector3 Forward, RightDirection, UpDirection;
        protected override Vector3 Scale => Vector3.One * 1.5f;
        protected override TypedIndex Shape() => TGCGame.GameContent.SH_Vehicle;
        protected override float Mass => 300f;

        private Vector3 HorizontalVelocity;
        private Vector3 VerticalVelocity;
        
        private BodyReference Body;
        internal readonly float MinSpeed = -200f;
        internal float MaxSpeed = 1000f;
        private const float AccelerationRate = 400f;

        private bool Grounded = false;

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
            Body = getBody();
            UpdateOrientation(Body);
        }

        public virtual void Update(float dTime, KeyboardState keyboardState) {
            Body = getBody();
            Acceleration(dTime, keyboardState);
            Turning(dTime, keyboardState);
            Jumping(keyboardState);
            Console.WriteLine("Auto: Velocity: " + Body.Velocity.Linear);
            UpdateOrientation(Body);     
        }

        //////////////////MOVEMENT//////////////////
        protected float boolToFloat(bool boolean) => boolean ? 1 : 0;
        protected float AccelerationAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.W)) - boolToFloat(keyboardState.IsKeyDown(Keys.S));
        protected float TurningAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.A)) - boolToFloat(keyboardState.IsKeyDown(Keys.D));
        
        private void Acceleration(float dTime, KeyboardState keyboardState)
        {
            float accelerationAxis = AccelerationAxis(keyboardState);
            if(accelerationAxis != 0)
                AddLinearVelocity(accelerationAxis * Forward * AccelerationRate * dTime);
        }

        private void Jumping(KeyboardState keyboardState)
        {
            float jumpPower = 400f;
            if(Grounded && keyboardState.IsKeyDown(Keys.Space))
                AddLinearVelocity(Vector3.UnitY * jumpPower);
            Grounded = false;
        }

        internal void Turning(float dTime, KeyboardState keyboardState)
        {
            float turningAxis = TurningAxis(keyboardState);
            if(turningAxis != 0)
                AddRotation(Quaternion.CreateFromAxisAngle(Vector3.UnitY, turningAxis * SpinningSensibility() * dTime)); 
        }

        private float SpinningSensibility() {
            if(HorizontalVelocity == Vector3.Zero)
                return 0f;
            else
                return MathF.Abs((HorizontalVelocity.Length() / MaxSpeed));
        }

        private void AddRotation(Quaternion rotation)
        {
            Body.Pose.Orientation *= rotation.ToBEPU();
        }

        private void AddLinearVelocity(Vector3 dVelocity)
        {
            Vector3 velocity = Body.Velocity.Linear.ToVector3() + dVelocity;
        
            HorizontalVelocity = velocity;
            HorizontalVelocity.Y = 0;

            VerticalVelocity = velocity;
            VerticalVelocity.X = 0;
            VerticalVelocity.Z = 0;

            Body.Velocity.Linear = (Vector3.Normalize(HorizontalVelocity) * Math.Clamp(HorizontalVelocity.Length(), MinSpeed, MaxSpeed) + 
                                    Vector3.UnitY * Math.Clamp(VerticalVelocity.Y, -400f, 400f)).ToBEPU();
        }

        private void UpdateOrientation(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Forward = -PhysicUtils.Forward(rotation);
            RightDirection = PhysicUtils.Left(rotation);
            UpDirection = PhysicUtils.Up(rotation);
        }

        internal void Turbo()
        {
            MaxSpeed += 200f;
            AddLinearVelocity(Forward * 500f);
        }

        public override bool HandleCollition(ICollitionHandler other)
        {
            if (!Destroyed)
            {
                if (other is Floor _)
                    Grounded = true;
            }
            return true;
        }
    }
}