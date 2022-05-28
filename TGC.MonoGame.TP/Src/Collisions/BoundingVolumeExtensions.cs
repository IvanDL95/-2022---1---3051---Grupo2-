using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Collisions
{
    public static class BoundingVolumesExtensions
    {

        /// <summary>
        ///     Get an extents vector that contains the half-size on each axis of the box.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBox"/> to calculate its extents</param>
        /// <returns>The extents on each axis</returns>
        public static Vector3 GetExtents(BoundingBox box)
        {
            var max = box.Max;
            var min = box.Min;

            return (max - min) * 0.5f;            
        }

        public static Vector3 GetCenter(BoundingBox box)
        {
            return (box.Max + box.Min) * 0.5f;
        }

        public static BoundingBox FromMatrix(Matrix matrix)
        {
            return new BoundingBox(Vector3.Transform(-Vector3.One * 0.5f, matrix), Vector3.Transform(Vector3.One * 0.5f, matrix));
        }

        public static Vector3 ClosestPoint(BoundingBox box, Vector3 point)
        {
            var min = box.Min;
            var max = box.Max;
            point.X = MathHelper.Clamp(point.X, min.X, max.X);
            point.Y = MathHelper.Clamp(point.Y, min.Y, max.Y);
            point.Z = MathHelper.Clamp(point.Z, min.Z, max.Z);
            return point;
        }

        public static BoundingBox CreateAABBFrom(Model model)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
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
            return new BoundingBox(minPoint, maxPoint);
        }
    }
}