//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnimatorSpeedSyncComponent animatorSpeedSync { get { return (AnimatorSpeedSyncComponent)GetComponent(GameComponentsLookup.AnimatorSpeedSync); } }
    public bool hasAnimatorSpeedSync { get { return HasComponent(GameComponentsLookup.AnimatorSpeedSync); } }

    public void AddAnimatorSpeedSync(int newPropertyHash) {
        var index = GameComponentsLookup.AnimatorSpeedSync;
        var component = (AnimatorSpeedSyncComponent)CreateComponent(index, typeof(AnimatorSpeedSyncComponent));
        component.PropertyHash = newPropertyHash;
        AddComponent(index, component);
    }

    public void ReplaceAnimatorSpeedSync(int newPropertyHash) {
        var index = GameComponentsLookup.AnimatorSpeedSync;
        var component = (AnimatorSpeedSyncComponent)CreateComponent(index, typeof(AnimatorSpeedSyncComponent));
        component.PropertyHash = newPropertyHash;
        ReplaceComponent(index, component);
    }

    public void RemoveAnimatorSpeedSync() {
        RemoveComponent(GameComponentsLookup.AnimatorSpeedSync);
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

    static Entitas.IMatcher<GameEntity> _matcherAnimatorSpeedSync;

    public static Entitas.IMatcher<GameEntity> AnimatorSpeedSync {
        get {
            if (_matcherAnimatorSpeedSync == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnimatorSpeedSync);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnimatorSpeedSync = matcher;
            }

            return _matcherAnimatorSpeedSync;
        }
    }
}