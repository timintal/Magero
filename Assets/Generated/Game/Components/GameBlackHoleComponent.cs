//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public BlackHoleComponent blackHole { get { return (BlackHoleComponent)GetComponent(GameComponentsLookup.BlackHole); } }
    public bool hasBlackHole { get { return HasComponent(GameComponentsLookup.BlackHole); } }

    public void AddBlackHole(float newExplosionRadius, float newPullSpeed) {
        var index = GameComponentsLookup.BlackHole;
        var component = (BlackHoleComponent)CreateComponent(index, typeof(BlackHoleComponent));
        component.ExplosionRadius = newExplosionRadius;
        component.PullSpeed = newPullSpeed;
        AddComponent(index, component);
    }

    public void ReplaceBlackHole(float newExplosionRadius, float newPullSpeed) {
        var index = GameComponentsLookup.BlackHole;
        var component = (BlackHoleComponent)CreateComponent(index, typeof(BlackHoleComponent));
        component.ExplosionRadius = newExplosionRadius;
        component.PullSpeed = newPullSpeed;
        ReplaceComponent(index, component);
    }

    public void RemoveBlackHole() {
        RemoveComponent(GameComponentsLookup.BlackHole);
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

    static Entitas.IMatcher<GameEntity> _matcherBlackHole;

    public static Entitas.IMatcher<GameEntity> BlackHole {
        get {
            if (_matcherBlackHole == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.BlackHole);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBlackHole = matcher;
            }

            return _matcherBlackHole;
        }
    }
}
