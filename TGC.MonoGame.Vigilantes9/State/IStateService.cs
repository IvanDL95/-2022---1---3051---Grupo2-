using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGC.MonoGame.Vigilantes9.State
{
    public interface IStateService
    {
        void ChangeState(GameState newState);
        GameState GetCurrentState();
    }
}