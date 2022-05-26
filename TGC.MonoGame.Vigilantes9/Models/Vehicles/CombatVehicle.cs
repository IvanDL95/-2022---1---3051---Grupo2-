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

        public CombatVehicle(float x, float y, float z) : base(new Vector3(x,y,z), Vector3.Forward, Vector3.Up)
        {
        }

        protected override float ModelScale { get; } = 0.1f;

        protected override string[] wheelBonesName { get; } = new string[]{
            "Wheel1",
            "Wheel2",
            "Wheel3",
            // "Wheel4",
            "Wheel5",
            "Wheel6",
            "Wheel7",
            "Wheel8",
        };

        public override void Load(Model model, Effect effect)
        {
            base.Load(model, effect);
        }


        public override void Draw(Effect effect)
        {
            // Set the world matrix as the root transform of the model.
            Model.Root.Transform = World;

            // Look up combined bone matrices for the entire model.
            Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            
            // Calculate matrices based on the current animation position.
            var wheelsRotation = Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(WheelsRotation, 0f, WheelsAcceleration));

            foreach (var wheel in WheelBones)
            {
                wheel.Bone.Transform =  wheelsRotation * wheel.Transform;
            }

            base.Draw(effect);
        }

        protected override void ApplyEffect(ModelMesh mesh, Effect effect)
        {
            var worldMesh = boneTransforms[mesh.ParentBone.Index] * Model.Root.Transform /* * Matrix.CreateRotationY(MathHelper.PiOver2) */;
            var texture = MeshTextures[mesh.Name][0];
            effect.Parameters["World"]?.SetValue(worldMesh);

            if(texture != null)
                effect.Parameters["ModelTexture"]?.SetValue(texture);

        /*         
            foreach (var meshPart in mesh.MeshParts) {
                meshPart.Effect.Parameters["ModelTexture"]?.SetValue(texture);
                meshPart.Effect.Parameters["World"]?.SetValue(worldMesh);
            }
        */
        /* 
            effect.Parameters["World"].GetValueMatrix()
            var index = 0;
            foreach (var meshPart in mesh.MeshParts)
            {
                var texture = textures[index];
                if(texture != null)
                    Effect.Parameters["ModelTexture"]?.SetValue(texture);
                index++;
            }
        */
        }
    }
}