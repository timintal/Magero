//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public LightningShooterComponent lightningShooter { get { return (LightningShooterComponent)GetComponent(GameComponentsLookup.LightningShooter); } }
    public bool hasLightningShooter { get { return HasComponent(GameComponentsLookup.LightningShooter); } }

    public void AddLightningShooter(float newCooldown, float newEffectRadius, int newTargetDamage, float newAOEDamage, float newStunDuration) {
        var index = GameComponentsLookup.LightningShooter;
        var component = (LightningShooterComponent)CreateComponent(index, typeof(LightningShooterComponent));
        component.Cooldown = newCooldown;
        component.EffectRadius = newEffectRadius;
        component.TargetDamage = newTargetDamage;
        component.AOEDamage = newAOEDamage;
        component.StunDuration = newStunDuration;
        AddComponent(index, component);
    }

    public void ReplaceLightningShooter(float newCooldown, float newEffectRadius, int newTargetDamage, float newAOEDamage, float newStunDuration) {
        var index = GameComponentsLookup.LightningShooter;
        var component = (LightningShooterComponent)CreateComponent(index, typeof(LightningShooterComponent));
        component.Cooldown = newCooldown;
        component.EffectRadius = newEffectRadius;
        component.TargetDamage = newTargetDamage;
        component.AOEDamage = newAOEDamage;
        component.StunDuration = newStunDuration;
        ReplaceComponent(index, component);
    }

    public void RemoveLightningShooter() {
        RemoveComponent(GameComponentsLookup.LightningShooter);
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

    static Entitas.IMatcher<GameEntity> _matcherLightningShooter;

    public static Entitas.IMatcher<GameEntity> LightningShooter {
        get {
            if (_matcherLightningShooter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LightningShooter);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLightningShooter = matcher;
            }

            return _matcherLightningShooter;
        }
    }
}
