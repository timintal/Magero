using JetBrains.Annotations;
using Magero.UIFramework;
using Magero.UIFramework.Components.NavBar;
using VContainer;

namespace Magero
{
    public class ShopNavBarScreen : NavBarScreen
    {
        private UIFrame _uiFrame;
        
        [Inject, UsedImplicitly]
        void SetDependencies(UIFrame uiFrame)
        {
            _uiFrame = uiFrame;
        }
    }
}