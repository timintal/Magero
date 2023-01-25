//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public FlowFieldComponent flowField { get { return (FlowFieldComponent)GetComponent(GameComponentsLookup.FlowField); } }
    public bool hasFlowField { get { return HasComponent(GameComponentsLookup.FlowField); } }

    public void AddFlowField(UnityEngine.Vector3 newInitialPoint, float newCellSize, int[][] newLevelField, int[][] newCurrentField, int[][] newBackField) {
        var index = GameComponentsLookup.FlowField;
        var component = (FlowFieldComponent)CreateComponent(index, typeof(FlowFieldComponent));
        component.InitialPoint = newInitialPoint;
        component.CellSize = newCellSize;
        component.LevelField = newLevelField;
        component.CurrentField = newCurrentField;
        component.BackField = newBackField;
        AddComponent(index, component);
    }

    public void ReplaceFlowField(UnityEngine.Vector3 newInitialPoint, float newCellSize, int[][] newLevelField, int[][] newCurrentField, int[][] newBackField) {
        var index = GameComponentsLookup.FlowField;
        var component = (FlowFieldComponent)CreateComponent(index, typeof(FlowFieldComponent));
        component.InitialPoint = newInitialPoint;
        component.CellSize = newCellSize;
        component.LevelField = newLevelField;
        component.CurrentField = newCurrentField;
        component.BackField = newBackField;
        ReplaceComponent(index, component);
    }

    public void RemoveFlowField() {
        RemoveComponent(GameComponentsLookup.FlowField);
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

    static Entitas.IMatcher<GameEntity> _matcherFlowField;

    public static Entitas.IMatcher<GameEntity> FlowField {
        get {
            if (_matcherFlowField == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.FlowField);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherFlowField = matcher;
            }

            return _matcherFlowField;
        }
    }
}
