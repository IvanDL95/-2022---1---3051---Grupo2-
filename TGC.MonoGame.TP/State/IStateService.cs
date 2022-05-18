using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.State
{
    public interface IStateService
    {
        void ChangeState(GameState newState);
        GameState GetCurrentState();
    }
}