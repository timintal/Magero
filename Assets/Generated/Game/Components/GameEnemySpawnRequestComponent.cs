//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public EnemySpawnRequestComponent enemySpawnRequest { get { return (EnemySpawnRequestComponent)GetComponent(GameComponentsLookup.EnemySpawnRequest); } }
    public bool hasEnemySpawnRequest { get { return HasComponent(GameComponentsLookup.EnemySpawnRequest); } }

    public void AddEnemySpawnRequest(int newCount) {
        var index = GameComponentsLookup.EnemySpawnRequest;
        var component = (EnemySpawnRequestComponent)CreateComponent(index, typeof(EnemySpawnRequestComponent));
        component.Count = newCount;
        AddComponent(index, component);
    }

    public void ReplaceEnemySpawnRequest(int newCount) {
        var index = GameComponentsLookup.EnemySpawnRequest;
        var component = (EnemySpawnRequestComponent)CreateComponent(index, typeof(EnemySpawnRequestComponent));
        component.Count = newCount;
        ReplaceComponent(index, component);
    }

    public void RemoveEnemySpawnRequest() {
        RemoveComponent(GameComponentsLookup.EnemySpawnRequest);
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

    static Entitas.IMatcher<GameEntity> _matcherEnemySpawnRequest;

    public static Entitas.IMatcher<GameEntity> EnemySpawnRequest {
        get {
            if (_matcherEnemySpawnRequest == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.EnemySpawnRequest);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherEnemySpawnRequest = matcher;
            }

            return _matcherEnemySpawnRequest;
        }
    }
}
