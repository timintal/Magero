using System.Collections.Generic;
using Magero.UIFramework;

namespace _Game.Flow
{
    public class InjectableGameFSM : GameFSM
    {
        public InjectableGameFSM(IReadOnlyList<FSMState> states, UIFrame uiFrame)
        {
            foreach (var fsmState in states)
            {
                fsmState.SetParent(this, uiFrame);
                RegisterState(fsmState);
            }   
        }
    }
}
