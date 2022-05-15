using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.TP.Models;

namespace TGC.MonoGame.TP
{
    class PlayerInput
    {
        public PlayerInput(CombatVehicle playerVehicle)
        {
            PlayerVehicle = playerVehicle;
        }

        public void Update(float elapsedTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                PlayerVehicle.Speed += elapsedTime * PlayerVehicle.Acceleration;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                if(PlayerVehicle.Speed > 0)
                    PlayerVehicle.Speed += elapsedTime * -PlayerVehicle.Acceleration;
                else
                    PlayerVehicle.Speed += elapsedTime * -PlayerVehicle.Acceleration / 2;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                PlayerVehicle.Rotation += (Vector3.UnitY * elapsedTime) * PlayerVehicle.RotationSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                PlayerVehicle.Rotation -= (Vector3.UnitY * elapsedTime) * PlayerVehicle.RotationSpeed;
            }
        }

        #region Fields

        private CombatVehicle PlayerVehicle;

        #endregion Fields
    }
}
