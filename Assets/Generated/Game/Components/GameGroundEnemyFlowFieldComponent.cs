//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly GroundEnemyFlowFieldComponent groundEnemyFlowFieldComponent = new GroundEnemyFlowFieldComponent();

    public bool isGroundEnemyFlowField {
        get { return HasComponent(GameComponentsLookup.GroundEnemyFlowField); }
        set {
            if (value != isGroundEnemyFlowField) {
                var index = GameComponentsLookup.GroundEnemyFlowField;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : groundEnemyFlowFieldComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherGroundEnemyFlowField;

    public static Entitas.IMatcher<GameEntity> GroundEnemyFlowField {
        get {
            if (_matcherGroundEnemyFlowField == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GroundEnemyFlowField);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGroundEnemyFlowField = matcher;
            }

            return _matcherGroundEnemyFlowField;
        }
    }
}
