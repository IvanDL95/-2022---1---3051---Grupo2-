using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.PhysicsEntities;
using TGC.MonoGame.TP.Drawers;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP.FinalEntities
{
    internal class WoodenBox: StaticEntity
    {
        protected override Vector3 Scale => Vector3.One * 2f;
        protected override Drawer Drawer() => TGCGame.GameContent.D_Box;
        protected override TypedIndex Shape() => TGCGame.GameContent.SH_Box;
    }    
}