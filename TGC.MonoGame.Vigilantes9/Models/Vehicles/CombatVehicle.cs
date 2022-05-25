#region Using Statements

using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Collisions;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Models.Vehicles
{
    public class CombatVehicle : VehicleModel
    {
        protected override string VehicleModelName { get; } = "CombatVehicle/Vehicle";
        protected override string VehicleEffectName { get; } = "TextShader";
        public override float ModelScale { get; } = 0.1f;

        public CombatVehicle(float x, float y, float z) : base(new Vector3(x,y,z), Vector3.Forward, Vector3.Up)
        {
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
/* 
            foreach (var mesh in Model.Meshes)
                foreach (var meshärt in mesh.MeshParts) {
                    var texture = ((BasicEffect)meshärt.Effect).Texture;
                    meshärt.Effect = Effect.Clone();
                    if(texture != null)
                        meshärt.Effect.Parameters["ModelTexture"].SetValue(texture);
                }
 */
            // Look up shortcut references to the bones we are going to animate.
            // Store the original transform matrix for each animating bone.
            AddBone("Wheel1");
            AddBone("Wheel2");
            AddBone("Wheel3");
            // AddBone("Wheel4");
            AddBone("Wheel5");
            AddBone("Wheel6");
            AddBone("Wheel7");
            AddBone("Wheel8");
        }

        public override void Draw(Matrix viewProjection)
        {
            // Set the world matrix as the root transform of the model.
            Model.Root.Transform = World;

            // Look up combined bone matrices for the entire model.
            Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            
            // Calculate matrices based on the current animation position.
            // var wheelsRotation = -Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitY, HorizontalVelocity * dTime));
            // var wheelsRotation = Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitY, WheelsRotation));

            // var wheelsRotation = Matrix.CreateRotationY(WheelsRotation);
            var wheelsRotation = Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(WheelsRotation, 0f, WheelsAcceleration));

            foreach (var wheel in WheelBones)
            {
                wheel.Bone.Transform =  wheelsRotation * wheel.Transform;
            }

            Effect.Parameters["ViewProjection"].SetValue(viewProjection);
            base.Draw(viewProjection);
        }

        protected override void ApplyEffect(ModelMesh mesh, Texture2D[] textures)
        {
            var worldMesh = boneTransforms[mesh.ParentBone.Index] * World * Matrix.CreateRotationY(-MathHelper.PiOver2);
            Effect.Parameters["World"]?.SetValue(worldMesh);
/* 
            foreach (var meshPart in mesh.MeshParts) {
                meshPart.Effect.Parameters["World"].SetValue(worldMesh);
            } */
            
            if(textures[0] != null)
                Effect.Parameters["ModelTexture"]?.SetValue(textures[0]);
/* 
            var index = 0;
            foreach (var meshPart in mesh.MeshParts)
            {
                var texture = textures[index];
                if(texture != null)
                    Effect.Parameters["ModelTexture"]?.SetValue(texture);
                index++;
            } */
        }
    }
}