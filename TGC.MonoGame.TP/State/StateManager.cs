using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.State
{
    public class StateManager : GameComponent, IStateService
    {
            GameState ActiveScreen;

            public StateManager(Game game) : base(game)
            {
            }

            public override void Initialize()
            {
                ChangeState(new GameScenario(Game));
                base.Initialize();
            }

            public void ChangeState(GameState newState)
            {
                Game.Components.Remove(ActiveScreen);
                ActiveScreen = newState;
                Game.Components.Add(ActiveScreen);
            }

            public GameState GetCurrentState()
            {
                return ActiveScreen;
            }
    }
}