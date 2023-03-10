//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly FieldUpdateCooldownComponent fieldUpdateCooldownComponent = new FieldUpdateCooldownComponent();

    public bool hasFieldUpdateCooldown {
        get { return HasComponent(GameComponentsLookup.FieldUpdateCooldown); }
        set {
            if (value != hasFieldUpdateCooldown) {
                var index = GameComponentsLookup.FieldUpdateCooldown;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : fieldUpdateCooldownComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherFieldUpdateCooldown;

    public static Entitas.IMatcher<GameEntity> FieldUpdateCooldown {
        get {
            if (_matcherFieldUpdateCooldown == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.FieldUpdateCooldown);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherFieldUpdateCooldown = matcher;
            }

            return _matcherFieldUpdateCooldown;
        }
    }
}
