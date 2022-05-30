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
        protected override float Mass => 50f;

        private Vector3 CurrentVelocity = Vector3.Zero;
        internal readonly float MinSpeed = -100f;
        internal readonly float MaxSpeed = 400f;
        private const float AccelerationRate = 180f; // Acceleration Refence 3: * 60 FPS = 180 Final Acceleration

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
            UpdateOrientation(Body());
        }

        public virtual void Update(float dTime, KeyboardState keyboardState) {
            BodyReference body = Body();
            Acceleration(dTime, keyboardState, body);
            Turning(dTime, keyboardState, body);   
            UpdateOrientation(body);     
        }

        //////////////////MOVEMENT//////////////////
        protected float boolToFloat(bool boolean) => boolean ? 1 : 0;
        protected float AccelerationAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.W)) - boolToFloat(keyboardState.IsKeyDown(Keys.S));
        protected float TurningAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.A)) - boolToFloat(keyboardState.IsKeyDown(Keys.D));
        private void Acceleration(float dTime, KeyboardState keyboardState, BodyReference body)
        {
            float accelerationAxis = AccelerationAxis(keyboardState);
            if(accelerationAxis != 0)
                AddLinearVelocity(body, accelerationAxis * Forward * AccelerationRate * dTime);
        }

        private float SpinningSensibility() {
            if(CurrentVelocity == Vector3.Zero)
                return 0f;
            else
                return MathF.Abs(10 * (CurrentVelocity.Length() / MaxSpeed));
        }


        internal void Turning(float dTime, KeyboardState keyboardState, BodyReference body)
        {
            float turningAxis = TurningAxis(keyboardState);
            if(turningAxis != 0)
                AddRotation(body, Quaternion.CreateFromAxisAngle(Vector3.UnitY, turningAxis * dTime)); // * SpinningSensibility() * dTime)); //dTime
        }


        private void AddRotation(BodyReference body, Quaternion rotation)
        {
            body.Pose.Orientation *= rotation.ToBEPU();
        }

        private void AddLinearVelocity(BodyReference body, Vector3 dVelocity)
        {
            Vector3 velocity = body.Velocity.Linear.ToVector3() + dVelocity;
        
            Vector3 velocityDirection = Vector3.Normalize(velocity);
            float speed = Math.Clamp(velocity.Length(), MinSpeed, MaxSpeed);

            Vector3 limitedVelocity = speed * velocityDirection;
            CurrentVelocity = velocity;

            body.Velocity.Linear = limitedVelocity.ToBEPU();
        }

        private void UpdateOrientation(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Forward = -PhysicUtils.Forward(rotation);
            RightDirection = PhysicUtils.Left(rotation);
            UpDirection = PhysicUtils.Up(rotation);
        }

        public override bool HandleCollition(ICollitionHandler other)
        {
            if (!Destroyed)
            {
                //if (other is ALGUIEN)
                    //PASA ALGO
            }

            Console.WriteLine("Auto: Colisione!");
            return false;
        }
    }
}