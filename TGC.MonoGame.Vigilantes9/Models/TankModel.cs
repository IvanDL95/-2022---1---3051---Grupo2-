#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Models
{
    public sealed class TankModel
    {
        /// <summary>
        ///     Loads the tank model.
        /// </summary>
        public void Load(Model model)
        {
            tankModel = model;

            foreach (ModelBone bone in tankModel.Bones) {
                string boneName = bonesMap[bone.Name];
                modelBoneD.Add(boneName, bone);
                transformD.Add(boneName, bone.Transform);
            }

            // Allocate the transform matrix array.
            boneTransforms = new Matrix[tankModel.Bones.Count];
        }
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            // Set the world matrix as the root transform of the model.
            tankModel.Root.Transform = world;

            // Calculate matrices based on the current animation position.
            var wheelRotation = Matrix.CreateRotationX(WheelRotation);
            var steerRotation = Matrix.CreateRotationY(SteerRotation);
            var turretRotation = Matrix.CreateRotationY(TurretRotation);
            var cannonRotation = Matrix.CreateRotationX(CannonRotation);
            var hatchRotation = Matrix.CreateRotationX(HatchRotation);

            // Apply matrices to the relevant bones.
/* 
           foreach( KeyValuePair<string, ModelBone> kvp in modelBoneD ) {
               string boneName = kvp.Key;
               ModelBone bone = kvp.Value;
               bone.Transform = wheelRotation * transformD[boneName];
            }
 */
            Transform("leftBackWheel", wheelRotation);
            Transform("rightBackWheel", wheelRotation);
            Transform("leftFrontWheel", wheelRotation);
            Transform("rightFrontWheel", wheelRotation);
            Transform("leftSteer", steerRotation);
            Transform("rightSteer", steerRotation);
            Transform("turret", turretRotation);
            Transform("cannon", cannonRotation);
            Transform("hatch", hatchRotation);

            // Look up combined bone matrices for the entire model.
            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            // Draw the model.
            foreach (var mesh in tankModel.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }

        public void Transform(string bone, Matrix rotation)
        {
            modelBoneD[bone].Transform = rotation * transformD[bone];
        }

        #region Fields

        // The XNA framework Model object that we are going to display.
        private Model tankModel;

        // Map  the ugly bone name to something human readable
        private Dictionary<string, string> bonesMap = new Dictionary<string, string>{
            {"l_back_wheel_geo", "leftBackWheel"},
            {"r_back_wheel_geo", "rightBackWheel"},
            {"l_front_wheel_geo", "leftFrontWheel"},
            {"r_front_wheel_geo", "rightFrontWheel"},
            {"l_steer_geo", "leftSteer"},
            {"r_steer_geo", "rightSteer"},
            {"turret_geo", "turret"},
            {"canon_geo", "cannon"},
            {"hatch_geo", "hatch"},
        };

        // Shortcut references to the bones that we are going to animate.
        // We could just look these up inside the Draw method,
        // but it is more efficient to do the lookups while loading and cache the results.
        private Dictionary<string, ModelBone> modelBoneD = new Dictionary<string, ModelBone>();

        // Store the original transform matrix for each animating bone.
        private Dictionary<string, Matrix> transformD = new Dictionary<string, Matrix>();

        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method,
        // but it is more efficient to reuse a single array, as this avoids creating unnecessary garbage.
        private Matrix[] boneTransforms;

        // Current animation positions.

        #endregion Fields

        #region Properties

        /// <summary>
        ///     Gets or sets the wheel rotation amount.
        /// </summary>
        public float WheelRotation { get; set; }

        /// <summary>
        ///     Gets or sets the steering rotation amount.
        /// </summary>
        public float SteerRotation { get; set; }

        /// <summary>
        ///     Gets or sets the turret rotation amount.
        /// </summary>
        public float TurretRotation { get; set; }

        /// <summary>
        ///     Gets or sets the cannon rotation amount.
        /// </summary>
        public float CannonRotation { get; set; }

        /// <summary>
        ///     Gets or sets the entry hatch rotation amount.
        /// </summary>
        public float HatchRotation { get; set; }

        #endregion Properties
    }
}