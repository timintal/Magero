//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public BlackHoleProjectileComponent blackHoleProjectile { get { return (BlackHoleProjectileComponent)GetComponent(GameComponentsLookup.BlackHoleProjectile); } }
    public bool hasBlackHoleProjectile { get { return HasComponent(GameComponentsLookup.BlackHoleProjectile); } }

    public void AddBlackHoleProjectile(float newExplosionRadius, float newPullSpeed, float newPullRadius, UnityEngine.GameObject newBlackHolePrefab, float newLifetime) {
        var index = GameComponentsLookup.BlackHoleProjectile;
        var component = (BlackHoleProjectileComponent)CreateComponent(index, typeof(BlackHoleProjectileComponent));
        component.ExplosionRadius = newExplosionRadius;
        component.PullSpeed = newPullSpeed;
        component.PullRadius = newPullRadius;
        component.BlackHolePrefab = newBlackHolePrefab;
        component.Lifetime = newLifetime;
        AddComponent(index, component);
    }

    public void ReplaceBlackHoleProjectile(float newExplosionRadius, float newPullSpeed, float newPullRadius, UnityEngine.GameObject newBlackHolePrefab, float newLifetime) {
        var index = GameComponentsLookup.BlackHoleProjectile;
        var component = (BlackHoleProjectileComponent)CreateComponent(index, typeof(BlackHoleProjectileComponent));
        component.ExplosionRadius = newExplosionRadius;
        component.PullSpeed = newPullSpeed;
        component.PullRadius = newPullRadius;
        component.BlackHolePrefab = newBlackHolePrefab;
        component.Lifetime = newLifetime;
        ReplaceComponent(index, component);
    }

    public void RemoveBlackHoleProjectile() {
        RemoveComponent(GameComponentsLookup.BlackHoleProjectile);
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

    static Entitas.IMatcher<GameEntity> _matcherBlackHoleProjectile;

    public static Entitas.IMatcher<GameEntity> BlackHoleProjectile {
        get {
            if (_matcherBlackHoleProjectile == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.BlackHoleProjectile);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBlackHoleProjectile = matcher;
            }

            return _matcherBlackHoleProjectile;
        }
    }
}