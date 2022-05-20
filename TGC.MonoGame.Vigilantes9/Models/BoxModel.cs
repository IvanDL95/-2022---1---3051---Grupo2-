#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Collisions;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Models
{
    public abstract class BoxModel
    {
        public BoxModel(Vector3 position, Vector3 scale)
        {
            World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        }

        public void Load(Model model, Effect effect)
        {
            Model = model;

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;

            Collider = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Collider = new BoundingBox(Collider.Min + World.Translation, Collider.Max + World.Translation);
        }

        public virtual void Update(GameTime gameTime)
        {
            // do stuff
        }

        public void Draw(Matrix viewProjection)
        {
            foreach (var mesh in Model.Meshes)
            {
                var worldMesh = mesh.ParentBone.Transform * World;
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["World"].SetValue(worldMesh);
                    effect.Parameters["ViewProjection"].SetValue(viewProjection);
                }
                mesh.Draw();
            }
        }

        #region Properties

        public Model Model { get; set; }
        public Effect Effect { get; set; }
        public BoundingBox Collider { get; set; }
        public Matrix World { get; set; }

        #endregion Properties
    }
}
