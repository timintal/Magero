//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public WindBlowerComponent windBlower { get { return (WindBlowerComponent)GetComponent(GameComponentsLookup.WindBlower); } }
    public bool hasWindBlower { get { return HasComponent(GameComponentsLookup.WindBlower); } }

    public void AddWindBlower(float newPushSpeed, float newPushDamping, float newMaxDistance) {
        var index = GameComponentsLookup.WindBlower;
        var component = (WindBlowerComponent)CreateComponent(index, typeof(WindBlowerComponent));
        component.PushSpeed = newPushSpeed;
        component.PushDamping = newPushDamping;
        component.MaxDistance = newMaxDistance;
        AddComponent(index, component);
    }

    public void ReplaceWindBlower(float newPushSpeed, float newPushDamping, float newMaxDistance) {
        var index = GameComponentsLookup.WindBlower;
        var component = (WindBlowerComponent)CreateComponent(index, typeof(WindBlowerComponent));
        component.PushSpeed = newPushSpeed;
        component.PushDamping = newPushDamping;
        component.MaxDistance = newMaxDistance;
        ReplaceComponent(index, component);
    }

    public void RemoveWindBlower() {
        RemoveComponent(GameComponentsLookup.WindBlower);
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

    static Entitas.IMatcher<GameEntity> _matcherWindBlower;

    public static Entitas.IMatcher<GameEntity> WindBlower {
        get {
            if (_matcherWindBlower == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.WindBlower);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherWindBlower = matcher;
            }

            return _matcherWindBlower;
        }
    }
}
