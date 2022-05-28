using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.FinalEntities
{
    public class WoodenBox
    {
        private Model Model { get; set; }
        private Effect Effect { get; set; }
        private Texture2D Texture { get; set; }
        public BoundingBox Collider { get; set; }
        public Matrix World { get; set; }
        private Matrix Scale { get; set; }
        private Matrix Rotation { get; set; }
        private Matrix Translation { get; set; }
        private Vector3 Position { get; set; }

        public WoodenBox(Vector3 position, float scale)
        {
            Scale = Matrix.CreateScale(scale);
            Rotation = Matrix.Identity;

            Position = position;
            Translation = Matrix.CreateTranslation(Position);

            World = Scale * Rotation * Translation;
        }

        public void Load(ContentManager content)
        {
            Model = content.Load<Model>(TGCContent.ContentFolder3D + "scenes/Box/box");
            Effect = content.Load<Effect>(TGCContent.ContentFolderEffects + "BasicShader");
            Texture = content.Load<Texture2D>(TGCContent.ContentFolderTextures + "wood/wooden_box");

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            Collider = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["Texture"].SetValue(Texture);
            Effect.Parameters["WorldViewProjection"].SetValue(World * view * projection);

            foreach (var mesh in Model.Meshes)
                mesh.Draw();
        }
    }    
}