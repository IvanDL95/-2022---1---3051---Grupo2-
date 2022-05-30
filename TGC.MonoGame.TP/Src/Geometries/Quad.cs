using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP.Geometries
{
    internal class Quad: Geometry
    {
        internal Quad(GraphicsDevice graphicsDevice): base(graphicsDevice) {}

        internal Drawer Drawer() => TGCGame.GameContent.D_Vehicle;

        protected override void CreateVertexBuffer(GraphicsDevice graphicsDevice)
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

        protected override void CreateIndexBuffer(GraphicsDevice graphicsDevice)
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
    
        internal void Draw(Matrix world) {
            Drawer().Draw(world);
        }
    }
}