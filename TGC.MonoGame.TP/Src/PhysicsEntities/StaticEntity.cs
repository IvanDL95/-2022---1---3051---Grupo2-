using System;
using Microsoft.Xna.Framework;
using BepuPhysics.Collidables;
using TGC.MonoGame.TP.Physics;
using BepuPhysics;

namespace TGC.MonoGame.TP.PhysicsEntities
{
    internal abstract class StaticEntity : PhysicsEntity, ICollitionHandler
    {
        private StaticHandle Handle;
        protected abstract TypedIndex Shape();

        protected Vector3 Position;
        protected Quaternion Rotation;
        protected abstract Vector3 Scale { get;}

        internal override Matrix World()
            => Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);

        internal override void Instantiate(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            Handle = TGCGame.PhysicsSimulation.CreateStatic(position, rotation, Shape());
            TGCGame.PhysicsSimulation.CollitionEvents.RegisterCollider(Handle, this);
            base.Instantiate(position, rotation);
        }

        internal override void Destroy()
        {
            TGCGame.PhysicsSimulation.CollitionEvents.UnregisterCollider(Handle);
            TGCGame.PhysicsSimulation.DestroyStatic(Handle);
            base.Destroy();
        }

        public virtual bool HandleCollition(ICollitionHandler other) => true;
    }
}