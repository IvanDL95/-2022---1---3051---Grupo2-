using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries.Textures;

namespace TGC.MonoGame.TP.Scene
{
    public class Floor
    {
        public Matrix World { get; set; }
        public Effect Effect { get; set; }
        // A Quad to draw the floor - TODO hacerlo polimorfico tal vez
        private QuadPrimitive Quad { get; set; }
        // The Floor World
        private Texture2D FloorTexture { get; set; }

        public Floor(QuadPrimitive quad, float scale)
        {
            Quad = quad;
            World = Matrix.CreateScale(scale);
        }

        public void Load(Texture2D texture, Effect effect)
        {
            FloorTexture = texture;
            Effect = effect;
        }

        public void Draw(Matrix viewProjection)
        {
            Effect.Parameters["WorldViewProjection"].SetValue(World * viewProjection);
            Quad.Draw(Effect);
        }
    }
}
