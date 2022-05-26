#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Collisions;

using Microsoft.Xna.Framework.Input;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Models
{
    public abstract class VehicleModel
    {

        public VehicleModel(Vector3 position, Vector3 forward, Vector3 up)
        {
            World = Matrix.CreateScale(ModelScale) * Matrix.CreateWorld(position, forward, up);
        }
    
        protected virtual float ModelScale { get; } = 1f;
        protected virtual string[] wheelBonesName { get; } = new string[]{};

        #region Fields

        protected Model Model;
        protected Dictionary<string, Texture2D[]> MeshTextures = new Dictionary<string, Texture2D[]>();
        protected List<(ModelBone Bone, Matrix Transform)> WheelBones { get; } = new List<(ModelBone, Matrix)>();
        protected Matrix[] boneTransforms;

        #endregion Fields

        /// <summary>
        ///     Loads the vehicle model.
        /// </summary>
        public virtual void Load(Model model, Effect effect)
        {
            Model = model;

            foreach (var mesh in Model.Meshes) {
                var partsCount = mesh.MeshParts.Count;
                Texture2D[] textures = new Texture2D[partsCount];

                for (var index = 0; index < partsCount; index++)
                {
                    ModelMeshPart meshärt = mesh.MeshParts[index];
                    var basicEffect = (BasicEffect)meshärt.Effect;
                    textures[index] = basicEffect.Texture;
                    meshärt.Effect = effect;
                }

                MeshTextures.Add(mesh.Name, textures);
            }

            foreach (var boneName in wheelBonesName)
            {
                AddBone(boneName);
            }

            // Allocate the transform matrix array.
            boneTransforms = new Matrix[Model.Bones.Count];
        }

        protected void AddBone(string modelBoneName)
        {
            ModelBone bone = Model.Bones[modelBoneName];
            WheelBones.Add((bone, bone.Transform));
        }

        public virtual void Update(KeyboardState keyboardState)
        {
            var spinningDirection = Convert.ToSingle(keyboardState.IsKeyDown(Keys.A)) - Convert.ToSingle(keyboardState.IsKeyDown(Keys.D));
            WheelsRotation = spinningDirection * MathHelper.PiOver4;
            
            var wheelsSpinning = 
                (Convert.ToSingle(keyboardState.IsKeyDown(Keys.W)) - Convert.ToSingle(keyboardState.IsKeyDown(Keys.S)));

            WheelsAcceleration += wheelsSpinning * 1f;
        }

        protected abstract void ApplyEffect(ModelMesh mesh, Effect effect);

        public virtual void Draw(Effect effect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                ApplyEffect(mesh, effect);
                mesh.Draw();
            }
        }

        public virtual void Draw(Matrix world, Matrix view, Matrix projection)
        {
            // Draw the model with basic effect.
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] * world;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        #region Properties

        /// <summary>
        ///     Gets or sets the vehicle world.
        /// </summary>
        public Matrix World { get; set; }

        /// <summary>
        ///     Gets or sets the wheel rotation amount.
        /// </summary>
        public float WheelsRotation { get; set; }

        /// <summary>
        ///     Gets or sets the wheel rotation amount.
        /// </summary>
        public float WheelsAcceleration { get; set; }

        #endregion Properties
    }
}