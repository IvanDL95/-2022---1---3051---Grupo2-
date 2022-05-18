#region Using Statements

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.Collisions;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Models.Vehicles
{
    public class CombatVehicle : Vehicle
    {
        public CombatVehicle(Game game) : base(game)
        {
            World = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Up);
        }

        public override void LoadContent()
        {
            Model = Game.Content.Load<Model>(TGCContent.ContentFolder3D + "vehicles/CombatVehicle/Vehicle");
            // Effect = new BasicEffect(Game.GraphicsDevice);
         
            Effect = Game.Content.Load<Effect>(TGCContent.ContentFolderEffects + "TextShader").Clone();
            var effect = Model.Meshes.FirstOrDefault().Effects.FirstOrDefault() as BasicEffect;
            // Effect.Parameters["ModelTexture"].SetValue(effect.Texture);

            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            Collider = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Collider = new BoundingBox(Collider.Min + Position, Collider.Max + Position);
        }

        public override void Draw(float dTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["ViewProjection"].SetValue(view * projection);
            // Effect.Parameters["ViewProjection"].SetValue(view * projection);
            // Matriz de mundo del arma con desplazamiento para que quede arriba del vehiculo
            // Matrix WeaponWorld = World * Matrix.CreateTranslation(Vector3.UnitY * WeaponOffset);
            // Vehicle.Draw(World, View, Projection);
            // Weapon.Draw(WeaponWorld, View, Projection);

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            foreach (ModelMesh mesh in Model.Meshes)
            {
/*                 foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = World;
                    effect.View = view;
                    effect.Projection = projection;
                } */
                mesh.Draw();
            }
/* 
            foreach (ModelMesh mesh in Weapon.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = WeaponWorld;
                    effect.View = View;
                    effect.Projection = Projection;
                }

                mesh.Draw();
            } */
        }
        
        #region Properties

        /// <summary>
        ///    The XNA framework Model object that we are going to display.
        /// </summary>
        // private Model Weapon;

        #endregion Properties
    }
}
