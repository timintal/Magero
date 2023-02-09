//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly UIOverlayComponent uIOverlayComponent = new UIOverlayComponent();

    public bool isUIOverlay {
        get { return HasComponent(GameComponentsLookup.UIOverlay); }
        set {
            if (value != isUIOverlay) {
                var index = GameComponentsLookup.UIOverlay;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : uIOverlayComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherUIOverlay;

    public static Entitas.IMatcher<GameEntity> UIOverlay {
        get {
            if (_matcherUIOverlay == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.UIOverlay);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherUIOverlay = matcher;
            }

            return _matcherUIOverlay;
        }
    }
}
