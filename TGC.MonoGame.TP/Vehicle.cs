using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Collisions;

namespace TGC.MonoGame.TP
{
    public class Vehicle
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private Model Model { get; set; }
        private Effect Effect { get; set; }
        private Texture2D Texture { get; set; }
        private BoundingBox Collider { get; set; }
        public Matrix World { get; set; }
        private Matrix Scale { get; set; }
        private Matrix Rotation { get; set; }
        private Matrix Translation { get; set; }
        private Vector3 Position { get; set; }

        private float SpinningSensibility { get; set; } = 0f;
        private float MaxSpinSensibility { get; set; } = 3f;
        
        private float Acceleration { get; set; } = 9f;
        private float Friction { get; set; } = 5f;
        private float HorizontalVelocity { get; set; } = 0f;
        private float MaxHorizontalVelocity { get; set; } = 700f;
        private float MinHorizontalVelocity { get; set; } = -200f;

        private bool Grounded = false; 

        public Vehicle(ContentManager content)
        {
            Model = content.Load<Model>(ContentFolder3D + "vehicles/Car/car");
            Effect = content.Load<Effect>(ContentFolderEffects + "CarShader");
            //Texture = content.Load<Texture2D>(ContentFolderTextures + "car/pallette");
            //Effect.Parameters["ModelTexture"].SetValue(Texture);

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            Scale = Matrix.CreateScale(1f);
            Rotation = Matrix.Identity;

            Position = Vector3.Zero;
            Translation = Matrix.CreateTranslation(Position);

            Collider = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);

            World = Scale * Rotation * Translation;
        }

        public void Update(float dTime, BoundingBox[] colliders, KeyboardState keyboardState) {
            VelocityUpdate(dTime, keyboardState);
            SpinSensibilityUpdate();

            Rotation *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitY, YAxis(keyboardState) * SpinningSensibility * dTime));

            //Vector3 movement = World.Forward * HorizontalVelocity * dTime;
            Position += World.Forward * HorizontalVelocity * dTime;
            Translation = Matrix.CreateTranslation(Position);
            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);

            DetectCollision(colliders);

            World = Scale * Rotation * Translation;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["ViewProjection"].SetValue(view * projection);

            foreach (var mesh in Model.Meshes)
            {
                var worldMesh = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"].SetValue(worldMesh);
                mesh.Draw();
            }
        }

        private void DetectCollision(BoundingBox[] colliders) {
            // Start by moving the Cylinder
            //Collider.Center += Vector3.Up * scaledVelocity.Y;
            // Set the OnGround flag on false, update it later if we find a collision
            //OnGround = false;

            // Collision detection
            var colliding = false;
            var foundIndex = -1;
            for (var index = 0; index < colliders.Length; index++)
            {
                if(!Collider.Intersects(colliders[index])) 
                    continue;
                
                // If we collided with something, set our velocity in Y to zero to reset acceleration
                HorizontalVelocity = 0;

                // Set our index and collision flag to true
                // The index is to tell which collider the Robot intersects with
                colliding = true;
                foundIndex = index;
                break;
            }

            // We correct based on differences in Y until we don't collide anymore
            // Not usual to iterate here more than once, but could happen
            while (colliding)
            {
                var externCollider = colliders[foundIndex];
                var externColliderY = BoundingVolumesExtensions.GetCenter(externCollider).Y;
                var externExtents = BoundingVolumesExtensions.GetExtents(externCollider);

                var selfColliderY = BoundingVolumesExtensions.GetCenter(Collider).Y; //RobotCylinder.Center.Y;
                var selfHalfHeight = (Collider.Max.Y - Collider.Min.Y) / 2;


                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (selfColliderY > externColliderY)
                {
                    penetration = externColliderY + externExtents.Y - selfColliderY + selfHalfHeight;
                    Grounded = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -selfColliderY - selfHalfHeight + externColliderY - externExtents.Y;

                // Move our Cylinder so we are not colliding anymore
                //RobotCylinder.Center += Vector3.Up * penetration;
                Position += Vector3.Up * penetration;
                colliding = false;

                // Check for collisions again
                for (var index = 0; index < colliders.Length; index++)
                {
                    if (!Collider.Intersects(colliders[index]))
                        continue;

                    // Iterate until we don't collide with anything anymore
                    colliding = true;
                    foundIndex = index;
                    break;
                }
            }
        }

        //////////////////MOVEMENT//////////////////
        private float boolToFloat(bool boolean) => boolean ? 1 : 0;

        private float AccelerationAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.W)) - boolToFloat(keyboardState.IsKeyDown(Keys.S));

        private float YAxis(KeyboardState keyboardState) => 
            boolToFloat(keyboardState.IsKeyDown(Keys.A)) - boolToFloat(keyboardState.IsKeyDown(Keys.D));

        private void VelocityUpdate(float dTime, KeyboardState keyboardState) {
            if(HorizontalVelocity > 0)
                HorizontalVelocity = MathHelper.Clamp(HorizontalVelocity - Friction, 0, MaxHorizontalVelocity); 
            else
                HorizontalVelocity = MathHelper.Clamp(HorizontalVelocity + Friction, MinHorizontalVelocity, 0); 
            HorizontalVelocity = MathHelper.Clamp(HorizontalVelocity + AccelerationAxis(keyboardState) * Acceleration, MinHorizontalVelocity, MaxHorizontalVelocity); 
        }

        private void SpinSensibilityUpdate() {
            if(HorizontalVelocity == 0)
                SpinningSensibility = 0f;
            else
                SpinningSensibility = MathF.Abs(MaxSpinSensibility * (HorizontalVelocity / MaxHorizontalVelocity));
        }
    }
}