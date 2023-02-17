using UIFramework.Runtime;
using Magero.UIFramework;

namespace _Game.Flow
{
    public abstract class FSMState
    {
        public void SetParent(GameFSM fsm, UIFrame uiFrame)
        {
            _parentFSM = fsm;
            _uiFrame = uiFrame;
        }
        
        protected GameFSM _parentFSM;
        protected UIFrame _uiFrame;
        internal virtual void OnEnter(){}
        internal virtual void OnExit(){}

    }
}