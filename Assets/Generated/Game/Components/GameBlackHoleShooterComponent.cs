//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public BlackHoleShooterComponent blackHoleShooter { get { return (BlackHoleShooterComponent)GetComponent(GameComponentsLookup.BlackHoleShooter); } }
    public bool hasBlackHoleShooter { get { return HasComponent(GameComponentsLookup.BlackHoleShooter); } }

    public void AddBlackHoleShooter(float newExplosionRadius, float newBlackHolePullSpeed, float newBlackHolePullRadius, float newBlackHoleLifetime, UnityEngine.GameObject newBlackHolePrefab) {
        var index = GameComponentsLookup.BlackHoleShooter;
        var component = (BlackHoleShooterComponent)CreateComponent(index, typeof(BlackHoleShooterComponent));
        component.ExplosionRadius = newExplosionRadius;
        component.BlackHolePullSpeed = newBlackHolePullSpeed;
        component.BlackHolePullRadius = newBlackHolePullRadius;
        component.BlackHoleLifetime = newBlackHoleLifetime;
        component.BlackHolePrefab = newBlackHolePrefab;
        AddComponent(index, component);
    }

    public void ReplaceBlackHoleShooter(float newExplosionRadius, float newBlackHolePullSpeed, float newBlackHolePullRadius, float newBlackHoleLifetime, UnityEngine.GameObject newBlackHolePrefab) {
        var index = GameComponentsLookup.BlackHoleShooter;
        var component = (BlackHoleShooterComponent)CreateComponent(index, typeof(BlackHoleShooterComponent));
        component.ExplosionRadius = newExplosionRadius;
        component.BlackHolePullSpeed = newBlackHolePullSpeed;
        component.BlackHolePullRadius = newBlackHolePullRadius;
        component.BlackHoleLifetime = newBlackHoleLifetime;
        component.BlackHolePrefab = newBlackHolePrefab;
        ReplaceComponent(index, component);
    }

    public void RemoveBlackHoleShooter() {
        RemoveComponent(GameComponentsLookup.BlackHoleShooter);
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

    static Entitas.IMatcher<GameEntity> _matcherBlackHoleShooter;

    public static Entitas.IMatcher<GameEntity> BlackHoleShooter {
        get {
            if (_matcherBlackHoleShooter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.BlackHoleShooter);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBlackHoleShooter = matcher;
            }

            return _matcherBlackHoleShooter;
        }
    }
}
