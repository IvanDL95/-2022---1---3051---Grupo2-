using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP;

namespace TGC.MonoGame.TP.Drawers
{
    internal class GeometryDrawer : Drawer
    {
        private static Effect Effect;
        protected readonly Geometry Geometry;
        protected readonly Texture2D Texture;

        internal GeometryDrawer(Geometry geometry, Effect effect, Texture2D texture)
        {
            Geometry = geometry;
            Effect = effect;
            Texture = texture;
        }

        internal override void Draw(Matrix world)
        {
            //Effect.Parameters["ViewProjection"].SetValue(World * TGCGame.Camera.ViewProjection);
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["Texture"].SetValue(Texture);
            //Effect.Parameters["Tiling"].SetValue(10f);
            var graphicsDevice = Effect.GraphicsDevice;

            graphicsDevice.SetVertexBuffer(Geometry.Vertices);
            graphicsDevice.Indices = Geometry.Indices;

            foreach (var effectPass in Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Geometry.Indices.IndexCount / 3);
            }
        }
    }
}