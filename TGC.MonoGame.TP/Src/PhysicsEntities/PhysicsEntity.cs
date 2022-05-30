using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP.PhysicsEntities
{
    internal abstract class PhysicsEntity
    {
        protected abstract Drawer Drawer();
        internal abstract Matrix World();

        internal virtual void Instantiate(Vector3 position) => Instantiate(position, Quaternion.Identity);
        internal virtual void Instantiate(Vector3 position, Quaternion rotation)
        {
            //TGCGame.CurrentScene.Register(this);
            OnInstantiate();
        }

        internal virtual void Destroy() { }// virtual => TGCGame.CurrentScene.Unregister(this);

        protected virtual void OnInstantiate() { }
        internal virtual void Update(double elapsedTime, GameTime gameTime) { }

        internal virtual void Draw() => Drawer().Draw(World());
    }
}