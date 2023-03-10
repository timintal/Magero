//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public GasProjectileShooterComponent gasProjectileShooter { get { return (GasProjectileShooterComponent)GetComponent(GameComponentsLookup.GasProjectileShooter); } }
    public bool hasGasProjectileShooter { get { return HasComponent(GameComponentsLookup.GasProjectileShooter); } }

    public void AddGasProjectileShooter(float newCloudRadius, float newMoveSpeedMultiplier, UnityEngine.GameObject newCloudPrefab, float newCloudLifetime) {
        var index = GameComponentsLookup.GasProjectileShooter;
        var component = (GasProjectileShooterComponent)CreateComponent(index, typeof(GasProjectileShooterComponent));
        component.CloudRadius = newCloudRadius;
        component.MoveSpeedMultiplier = newMoveSpeedMultiplier;
        component.CloudPrefab = newCloudPrefab;
        component.CloudLifetime = newCloudLifetime;
        AddComponent(index, component);
    }

    public void ReplaceGasProjectileShooter(float newCloudRadius, float newMoveSpeedMultiplier, UnityEngine.GameObject newCloudPrefab, float newCloudLifetime) {
        var index = GameComponentsLookup.GasProjectileShooter;
        var component = (GasProjectileShooterComponent)CreateComponent(index, typeof(GasProjectileShooterComponent));
        component.CloudRadius = newCloudRadius;
        component.MoveSpeedMultiplier = newMoveSpeedMultiplier;
        component.CloudPrefab = newCloudPrefab;
        component.CloudLifetime = newCloudLifetime;
        ReplaceComponent(index, component);
    }

    public void RemoveGasProjectileShooter() {
        RemoveComponent(GameComponentsLookup.GasProjectileShooter);
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

    static Entitas.IMatcher<GameEntity> _matcherGasProjectileShooter;

    public static Entitas.IMatcher<GameEntity> GasProjectileShooter {
        get {
            if (_matcherGasProjectileShooter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GasProjectileShooter);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGasProjectileShooter = matcher;
            }

            return _matcherGasProjectileShooter;
        }
    }
}
