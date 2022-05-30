using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Drawers
{
    internal class BasicDrawer : Drawer
    {
        private Effect Effect() => TGCGame.GameContent.E_BasicShader;
        protected readonly Model Model;
        protected readonly Texture2D Texture;

        internal BasicDrawer(Model model, Texture2D texture) // Texture2D[] textures)
        {
            this.Model = model;
            this.Texture = texture;
        }

        internal override void Draw(Matrix generalWorld)
        {
            Effect effect = Effect();

            Model.setEffect(Effect());
            effect.Parameters["Texture"].SetValue(Texture);
            ModelMeshCollection meshes = Model.Meshes;
            foreach (var mesh in meshes)
            {
                Matrix worldMatrix = mesh.ParentBone.Transform * generalWorld;
                effect.Parameters["World"].SetValue(worldMatrix);
                //Effect.Parameters["Texture"].SetValue(Textures[index]);
                mesh.Draw();
            }
        }
    }
}