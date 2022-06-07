using BepuPhysics;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.PhysicsEntities
{
    internal abstract class KinematicEntity : BodyEntity
    {
        protected override BodyHandle CreateBody(Vector3 position, Quaternion rotation) =>
            TGCGame.PhysicsSimulation.CreateKinematic(position, rotation, Shape());
    }
}