﻿using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.PhysicsEntities
{
    internal abstract class BodyEntity : PhysicsEntity, ICollitionHandler
    {
        protected virtual Vector3 Scale { get; } = Vector3.One;
        protected abstract TypedIndex Shape();
        protected BodyHandle Handle;
        protected BodyReference Body() => TGCGame.PhysicsSimulation.GetBody(Handle);
        protected bool Destroyed { get; private set; } = false;

        internal override Matrix World()
        {
            RigidPose pose = Body().Pose;
            return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(pose.Orientation.ToQuaternion()) * Matrix.CreateTranslation(pose.Position.ToVector3());
        }

        
        internal override void Instantiate(Vector3 position, Quaternion rotation)
        {
            Handle = CreateBody(position, rotation);
            TGCGame.PhysicsSimulation.CollitionEvents.RegisterCollider(Handle, this);
            base.Instantiate(position, rotation);
        }

        internal override void Destroy()
        {
            TGCGame.PhysicsSimulation.CollitionEvents.UnregisterCollider(Handle);
            if (!Body().Exists && !Destroyed)
                TGCGame.PhysicsSimulation.DestroyBody(Handle);
            base.Destroy();
            Destroyed = true;
        }

        protected abstract BodyHandle CreateBody(Vector3 position, Quaternion rotation);

        public virtual bool HandleCollition(ICollitionHandler other) => !Destroyed;
    }
}