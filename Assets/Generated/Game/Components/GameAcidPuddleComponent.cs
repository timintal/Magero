//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AcidPuddleComponent acidPuddle { get { return (AcidPuddleComponent)GetComponent(GameComponentsLookup.AcidPuddle); } }
    public bool hasAcidPuddle { get { return HasComponent(GameComponentsLookup.AcidPuddle); } }

    public void AddAcidPuddle(float newPuddleLifetime, float newInitialRadius, UnityEngine.AnimationCurve newRadiusCurve) {
        var index = GameComponentsLookup.AcidPuddle;
        var component = (AcidPuddleComponent)CreateComponent(index, typeof(AcidPuddleComponent));
        component.PuddleLifetime = newPuddleLifetime;
        component.InitialRadius = newInitialRadius;
        component.RadiusCurve = newRadiusCurve;
        AddComponent(index, component);
    }

    public void ReplaceAcidPuddle(float newPuddleLifetime, float newInitialRadius, UnityEngine.AnimationCurve newRadiusCurve) {
        var index = GameComponentsLookup.AcidPuddle;
        var component = (AcidPuddleComponent)CreateComponent(index, typeof(AcidPuddleComponent));
        component.PuddleLifetime = newPuddleLifetime;
        component.InitialRadius = newInitialRadius;
        component.RadiusCurve = newRadiusCurve;
        ReplaceComponent(index, component);
    }

    public void RemoveAcidPuddle() {
        RemoveComponent(GameComponentsLookup.AcidPuddle);
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

    static Entitas.IMatcher<GameEntity> _matcherAcidPuddle;

    public static Entitas.IMatcher<GameEntity> AcidPuddle {
        get {
            if (_matcherAcidPuddle == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AcidPuddle);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAcidPuddle = matcher;
            }

            return _matcherAcidPuddle;
        }
    }
}
