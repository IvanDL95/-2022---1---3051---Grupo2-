#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TGC.MonoGame.TP;

#endregion Using Statements

namespace TGC.MonoGame.TP.Models
{
    class CombatVehicle
    {
        private const float WeaponOffset = 60f;

        public CombatVehicle(ContentManager content)
        {
            Content = content;
        }

        /// <summary>
        ///     Loads the tank model.
        /// </summary>
        public void Load(Vector3 position)
        {
            Vehicle = Content.Load<Model>(TGCGame.ContentFolder3D + "vehicles/CombatVehicle/Vehicle");
            Weapon = Content.Load<Model>(TGCGame.ContentFolder3D + "vehicles/CombatVehicle/Weapons");
            World = Matrix.Identity;

            // Starting position
            Position = position;
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public void Update(float elapsedTime)
        {
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation = elapsedTime * Vector3.UnitY;
            World = Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.Z, Rotation.X) * Matrix.CreateTranslation(Position);
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            // Matriz de mundo del arma con desplazamiento para que quede arriba del vehiculo
            Matrix WeaponWorld = World * Matrix.CreateTranslation(Vector3.UnitY * WeaponOffset);
            // Vehicle.Draw(World, View, Projection);
            // Weapon.Draw(WeaponWorld, View, Projection);
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            foreach (ModelMesh mesh in Vehicle.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = World;
                    effect.View = View;
                    effect.Projection = Projection;
                }

                mesh.Draw();
            }

            foreach (ModelMesh mesh in Weapon.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = WeaponWorld;
                    effect.View = View;
                    effect.Projection = Projection;
                }

                mesh.Draw();
            }
        }
        
        #region Fields

        /// <summary>
        ///    The XNA framework Content Manager.
        /// </summary>
        private ContentManager Content;

        /// <summary>
        ///    The XNA framework Model object that we are going to display.
        /// </summary>
        private Model Vehicle;

        /// <summary>
        ///    The XNA framework Model object that we are going to display.
        /// </summary>
        private Model Weapon;

        /// <summary>
        ///    Position in the vehicle world.
        /// </summary>a
        private Vector3 Position{ get; set; }

        /// <summary>
        ///     Rotation in radians per axis.
        /// </summary>a
        private Vector3 Rotation { get; set; }

        /// <summary>
        ///    The World matrix which contain the current transformation.
        ///    Scale * Rotation * Translation
        /// </summary>
        private Matrix World { get; set; }

        private Effect Effect { get; set; }

        #endregion Properties
    }
}
