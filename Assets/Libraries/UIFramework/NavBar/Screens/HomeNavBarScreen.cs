using JetBrains.Annotations;
using VContainer;
using Magero.UIFramework;
using Magero.UIFramework.Components.NavBar;

namespace Magero
{
    public class HomeNavBarScreen : NavBarScreen
    {
        private UIFrame _uiFrame;
        
        [Inject, UsedImplicitly]
        void SetDependencies(UIFrame uiFrame)
        {
            _uiFrame = uiFrame;
        }
    }
}