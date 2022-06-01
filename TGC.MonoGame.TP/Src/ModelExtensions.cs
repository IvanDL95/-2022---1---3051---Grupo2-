using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP
{
    internal static class ModelExtensions
    {
        internal static void setEffect(this Model model, Effect effect) {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
        }

        internal static Texture2D getTexture(this Model model)
        {
            var effect = model.Meshes.FirstOrDefault().Effects.FirstOrDefault() as BasicEffect;
            return effect.Texture;
        }

        internal static Box CreateBoxShape(this Model model, float scale = 1)
        {
            Vector3 minPoint = Vector3.One * float.MaxValue;
            Vector3 maxPoint = Vector3.One * float.MinValue;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            var meshes = model.Meshes;
            for (int index = 0; index < meshes.Count; index++)
            {
                var meshParts = meshes[index].MeshParts;
                for (int subIndex = 0; subIndex < meshParts.Count; subIndex++)
                {
                    var vertexBuffer = meshParts[subIndex].VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    var vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[meshes[index].ParentBone.Index];
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }
            
            Vector3 size = (maxPoint - minPoint) * scale;
            return new Box(size.X, size.Y, size.Z);
        }
    }
}