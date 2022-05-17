using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Collisions;

namespace TGC.MonoGame.TP
{
    public class Vehicle
    {   
        private Model Model { get; set; }
        private Effect Effect { get; set; }
        private Texture2D Texture { get; set; }
        private BoundingBox Collider { get; set; }
        public Matrix World { get; set; }
        private Matrix Scale { get; set; }
        private Matrix Rotation { get; set; }
        private Matrix Translation { get; set; }
        private Vector3 Position { get; set; }
        private Matrix WheelRotation { get; set; }

        private float SpinningSensibility { get; set; } = 0f;
        private float MaxSpinSensibility { get; set; } = 3f;
        
        private float Acceleration { get; set; } = 9f;
        private float Friction { get; set; } = 5f;
        private float HorizontalVelocity { get; set; } = 0f;
        private float MaxHorizontalVelocity { get; set; } = 700f;
        private float MinHorizontalVelocity { get; set; } = -200f;
        private bool Grounded = false; 

        private Vector3 Color;

        private const float EPSILON = 0.00001f;

        public Vehicle(ContentManager content)
        {
            Model = content.Load<Model>(TGCContent.ContentFolder3D + "vehicles/Car/car");
            Effect = content.Load<Effect>(TGCContent.ContentFolderEffects + "TextShader");
            var effect = Model.Meshes.FirstOrDefault().Effects.FirstOrDefault() as BasicEffect;
            Effect.Parameters["ModelTexture"].SetValue(effect.Texture);

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            Scale = Matrix.CreateScale(1f);
            Rotation = Matrix.Identity;
            WheelRotation = Rotation;

            Position = Vector3.Zero;
            Translation = Matrix.CreateTranslation(Position);

            Collider = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);

            Color = new Vector3(0, 1, 1);

            World = Scale * Rotation * Translation;
        }

        public void Update(float dTime, BoundingBox[] colliders, KeyboardState keyboardState) {
            VelocityUpdate(dTime, keyboardState);
            SpinSensibilityUpdate();
            Position += World.Forward * HorizontalVelocity * dTime;

            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);
            DetectCollision(colliders);

            Rotation *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitY, YAxis(keyboardState) * SpinningSensibility * dTime));
            Translation = Matrix.CreateTranslation(Position);

            if(Grounded && keyboardState.IsKeyDown(Keys.S))
                Jump();
 
            World = Scale * Rotation * Translation;
        }

        public void Draw(float dTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["ViewProjection"].SetValue(view * projection);
            WheelRotation *= -Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitX, HorizontalVelocity * dTime));
            float index = 0f;

            foreach (var mesh in Model.Meshes)
            {
                var worldMesh = mesh.ParentBone.Transform;

                if(index>0)
                    worldMesh *= WheelRotation * World ;
                else
                    worldMesh *=  World;
                index++;
                
                Effect.Parameters["World"].SetValue(worldMesh);
                mesh.Draw();
            }
        }
        
        //////////////////COLLISION//////////////////
        private void DetectCollision(BoundingBox[] colliders) {

            //VERTICAL
            // Start by moving the Cylinder
            //Collider.Center += Vector3.Up * scaledVelocity.Y;
            // Set the OnGround flag on false, update it later if we find a collision
            /*Grounded = false;
            Color = new Vector3(0, 1, 1);

            // Collision detection
           var colliding = false;
            var foundIndex = -1;
            for (var index = 0; index < colliders.Length; index++)
            {
                if(!Collider.Intersects(colliders[index])) 
                    continue;
                
                // If we collided with something, set our velocity in Y to zero to reset acceleration
                //HorizontalVelocity = 0; Vertical

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
            }*/

            //HORIZONTAL
            if (HorizontalVelocity == 0f)
                return;
            
            // Start by moving the Cylinder horizontally
            // RobotCylinder.Center += new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            // Check intersection for every collider
            for (var index = 0; index < colliders.Length; index++)
            {
                if(!Collider.Intersects(colliders[index]))
                    continue;

                // Get the intersected collider and its center
                var externCollider = colliders[index];
                var colliderCenter = BoundingVolumesExtensions.GetCenter(externCollider);

                // Get the cylinder center at the same Y-level as the box
                var sameLevelCenter = BoundingVolumesExtensions.GetCenter(Collider);
                sameLevelCenter.Y = colliderCenter.Y;

                // Find the closest horizontal point from the box
                var closestPoint = BoundingVolumesExtensions.ClosestPoint(externCollider, sameLevelCenter);

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - closestPoint;
                var normalVectorLength = normalVector.Length();

                // Our penetration is the difference between the radius of the Cylinder and the Normal Vector
                // For precission problems, we push the cylinder with a small increment to prevent re-colliding into the geometry
                var selfHalfWidth = (Collider.Max.X - Collider.Min.X) / 2;
                var penetration = selfHalfWidth - normalVector.Length() + EPSILON;

                // Push the center out of the box
                // Normalize our Normal Vector using its length first
                //RobotCylinder.Center += (normalVector / normalVectorLength * penetration);
                HorizontalVelocity = 0f;
                Position += normalVector / normalVectorLength * penetration;
                Color = new Vector3(1, 0, 0);
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

        private void Jump() {}
    }
}