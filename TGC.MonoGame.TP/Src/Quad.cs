using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    public class Quad
    {
        private Effect Effect { get; set; }
        private VertexBuffer Vertices { get; set; }
        private IndexBuffer Indices { get; set; }

         public void Load(GraphicsDevice graphicsDevice)
        {

            CreateVertexBuffer(graphicsDevice);
            CreateIndexBuffer(graphicsDevice);
        }

        private void CreateVertexBuffer(GraphicsDevice graphicsDevice)
        {
            var textureCoordinateLowerLeft = Vector2.Zero;
            var textureCoordinateLowerRight = Vector2.UnitX;
            var textureCoordinateUpperLeft = Vector2.UnitY;
            var textureCoordinateUpperRight = Vector2.One;

            var vertices = new[]
            {
                new VertexPositionNormalTexture(Vector3.UnitX + Vector3.UnitZ, Vector3.Up, textureCoordinateUpperRight),
                new VertexPositionNormalTexture(Vector3.UnitX - Vector3.UnitZ, Vector3.Up, textureCoordinateLowerRight),
                new VertexPositionNormalTexture(Vector3.UnitZ - Vector3.UnitX, Vector3.Up, textureCoordinateUpperLeft),
                new VertexPositionNormalTexture(-Vector3.UnitX - Vector3.UnitZ, Vector3.Up, textureCoordinateLowerLeft)
            };

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            var indices = new ushort[]
            {
                3, 1, 0,
                3, 0, 2,
            };
            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length,
                BufferUsage.WriteOnly);
            Indices.SetData(indices);
        }

        public void Draw(Effect effect)
        {
            var graphicsDevice = effect.GraphicsDevice;

            graphicsDevice.SetVertexBuffer(Vertices);
            graphicsDevice.Indices = Indices;

            foreach (var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.IndexCount / 3);
            }
        }
    }
}