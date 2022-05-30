using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Geometries
{
    internal abstract class Geometry
    {
        internal VertexBuffer Vertices { get; set; }
        internal IndexBuffer Indices { get; set; }

        internal Geometry(GraphicsDevice graphicsDevice) 
        {
            CreateVertexBuffer(graphicsDevice);
            CreateIndexBuffer(graphicsDevice);
        }

        protected abstract void CreateVertexBuffer(GraphicsDevice graphicsDevice);
        protected abstract void CreateIndexBuffer(GraphicsDevice graphicsDevice);
    }
}