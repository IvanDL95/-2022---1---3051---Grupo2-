using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.MonoGame.TP.FinalEntities
{
    public class Floor
    {
        private Matrix World;
        private Quad Quad;
        private Effect Effect;
        private Texture2D Texture;

        public Floor(float scale)
        {
            World = Matrix.CreateScale(scale); 
            Quad = new Quad();
        }

        public void Load(ContentManager content)
        {
            
            Effect = content.Load<Effect>(TGCContent.ContentFolderEffects + "TilingShader");
            Texture = content.Load<Texture2D>(TGCContent.ContentFolderTextures + "floor/palette");
            Quad.Load(Effect.GraphicsDevice);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["WorldViewProjection"].SetValue(World * view * projection);
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["Texture"].SetValue(Texture);
            Effect.Parameters["Tiling"].SetValue(10f);
            Quad.Draw(Effect);
        }
    }
}
