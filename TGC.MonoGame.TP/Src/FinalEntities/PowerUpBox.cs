using TGC.MonoGame.TP.Physics;
using TGC.MonoGame.TP.PhysicsEntities;
using BepuPhysics.Collidables;
using TGC.MonoGame.TP.Drawers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.FinalEntities
{
    internal class PowerUpBox: KinematicEntity
    {
        protected override Drawer Drawer() => TGCGame.GameContent.D_PowerUpBox;
        protected override Vector3 Scale => Vector3.One * 2f;
        protected override TypedIndex Shape() => TGCGame.GameContent.SH_Box;
        private float Rotation = 0f; 

        protected override void OnInstantiate()
        {
            base.OnInstantiate();
        }

        public virtual void Update(float dTime) {
            Rotation += dTime * 1f;
            getBody().Pose.Orientation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, Rotation).ToBEPU(); 
        }

        public override bool HandleCollition(ICollitionHandler other)
        {
            if (!Destroyed)
            {
                if (other is Vehicle _)
                {
                    ((Vehicle)other).Turbo();
                    Destroy();
                }
                    
            }
            return false;
        }
        
    }
}