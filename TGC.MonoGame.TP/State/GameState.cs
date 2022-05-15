using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Cameras;

namespace TGC.MonoGame.TP.State
{
    public abstract class GameState : DrawableGameComponent
    {
        public GameState(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}