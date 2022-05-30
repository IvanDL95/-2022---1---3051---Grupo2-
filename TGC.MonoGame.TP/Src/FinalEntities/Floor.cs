using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP.FinalEntities
{
    internal class Floor
    {
        private Matrix World;
        internal Drawer Drawer() => TGCGame.GameContent.D_Floor;
        internal Floor(float scale)
        {
            World = Matrix.CreateScale(scale); 
        }

        internal void Draw()
        {
            Drawer().Draw(World);
        }
    }
}
