using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Models;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class Player
    {
        private CombatVehicle Vehicle { get; set; }

        public Player(CombatVehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public void Update(GameTime gameTime) {

        }
    }
}