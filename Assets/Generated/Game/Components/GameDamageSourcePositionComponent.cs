//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public DamageSourcePositionComponent damageSourcePosition { get { return (DamageSourcePositionComponent)GetComponent(GameComponentsLookup.DamageSourcePosition); } }
    public bool hasDamageSourcePosition { get { return HasComponent(GameComponentsLookup.DamageSourcePosition); } }

    public void AddDamageSourcePosition(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.DamageSourcePosition;
        var component = (DamageSourcePositionComponent)CreateComponent(index, typeof(DamageSourcePositionComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceDamageSourcePosition(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.DamageSourcePosition;
        var component = (DamageSourcePositionComponent)CreateComponent(index, typeof(DamageSourcePositionComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveDamageSourcePosition() {
        RemoveComponent(GameComponentsLookup.DamageSourcePosition);
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

    static Entitas.IMatcher<GameEntity> _matcherDamageSourcePosition;

    public static Entitas.IMatcher<GameEntity> DamageSourcePosition {
        get {
            if (_matcherDamageSourcePosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.DamageSourcePosition);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDamageSourcePosition = matcher;
            }

            return _matcherDamageSourcePosition;
        }
    }
}
