//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public RagdollCurrentVelocityComponent ragdollCurrentVelocity { get { return (RagdollCurrentVelocityComponent)GetComponent(GameComponentsLookup.RagdollCurrentVelocity); } }
    public bool hasRagdollCurrentVelocity { get { return HasComponent(GameComponentsLookup.RagdollCurrentVelocity); } }

    public void AddRagdollCurrentVelocity(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.RagdollCurrentVelocity;
        var component = (RagdollCurrentVelocityComponent)CreateComponent(index, typeof(RagdollCurrentVelocityComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceRagdollCurrentVelocity(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.RagdollCurrentVelocity;
        var component = (RagdollCurrentVelocityComponent)CreateComponent(index, typeof(RagdollCurrentVelocityComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveRagdollCurrentVelocity() {
        RemoveComponent(GameComponentsLookup.RagdollCurrentVelocity);
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

    static Entitas.IMatcher<GameEntity> _matcherRagdollCurrentVelocity;

    public static Entitas.IMatcher<GameEntity> RagdollCurrentVelocity {
        get {
            if (_matcherRagdollCurrentVelocity == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.RagdollCurrentVelocity);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherRagdollCurrentVelocity = matcher;
            }

            return _matcherRagdollCurrentVelocity;
        }
    }
}
