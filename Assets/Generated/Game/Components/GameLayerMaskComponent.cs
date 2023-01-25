//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public LayerMaskComponent layerMask { get { return (LayerMaskComponent)GetComponent(GameComponentsLookup.LayerMask); } }
    public bool hasLayerMask { get { return HasComponent(GameComponentsLookup.LayerMask); } }

    public void AddLayerMask(UnityEngine.LayerMask newMask) {
        var index = GameComponentsLookup.LayerMask;
        var component = (LayerMaskComponent)CreateComponent(index, typeof(LayerMaskComponent));
        component.Mask = newMask;
        AddComponent(index, component);
    }

    public void ReplaceLayerMask(UnityEngine.LayerMask newMask) {
        var index = GameComponentsLookup.LayerMask;
        var component = (LayerMaskComponent)CreateComponent(index, typeof(LayerMaskComponent));
        component.Mask = newMask;
        ReplaceComponent(index, component);
    }

    public void RemoveLayerMask() {
        RemoveComponent(GameComponentsLookup.LayerMask);
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

    static Entitas.IMatcher<GameEntity> _matcherLayerMask;

    public static Entitas.IMatcher<GameEntity> LayerMask {
        get {
            if (_matcherLayerMask == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LayerMask);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLayerMask = matcher;
            }

            return _matcherLayerMask;
        }
    }
}
