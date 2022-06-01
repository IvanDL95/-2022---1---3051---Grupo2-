using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.PhysicsEntities;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP.FinalEntities
{
    internal class Floor: StaticEntity
    {
        private float ScaleValue;
        protected override Vector3 Scale => Vector3.One * ScaleValue;
        protected override Drawer Drawer() => TGCGame.GameContent.D_Floor;
        protected override TypedIndex Shape() => TGCGame.GameContent.SH_Floor;

        internal Floor(float scale)
        {
            ScaleValue = scale; 
        }
    }
}
