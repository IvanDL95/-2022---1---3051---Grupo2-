using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Collisions;

namespace TGC.MonoGame.TP
{
    public class Box
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

        public Box(ContentManager content, Vector3 position, float scale)
        {
            Model = content.Load<Model>(TGCContent.ContentFolder3D + "scenes/Box/box");
            Effect = content.Load<Effect>(TGCContent.ContentFolderEffects + "TextShader");
            Texture = content.Load<Texture2D>(TGCContent.ContentFolderTextures + "wood/wooden_box");
            Effect.Parameters["ModelTexture"].SetValue(Texture);

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            Scale = Matrix.CreateScale(scale);
            Rotation = Matrix.Identity;

            Position = position;
            Translation = Matrix.CreateTranslation(Position);

            Collider = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);

            World = Scale * Rotation * Translation;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["ViewProjection"].SetValue(view * projection);
            //Effect.Parameters["Projection"].SetValue(projection);
            //Effect.Parameters["DiffuseColor"].SetValue(new Vector3(0, 0, 1));

            foreach (var mesh in Model.Meshes)
            {
                var worldMesh = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"].SetValue(worldMesh);
                mesh.Draw();
            }
        }
    }    
}