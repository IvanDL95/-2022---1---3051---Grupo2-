using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.PhysicsEntities;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP.FinalEntities
{
    internal class Tub: StaticEntity
    {
        protected override Vector3 Scale => Vector3.One * 3000f;
        protected override Drawer Drawer() => TGCGame.GameContent.D_Tub;
        protected override TypedIndex Shape() => TGCGame.GameContent.SH_Tub;
    }
}
