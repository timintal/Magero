//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly WeaponCooldownComponent weaponCooldownComponent = new WeaponCooldownComponent();

    public bool hasWeaponCooldown {
        get { return HasComponent(GameComponentsLookup.WeaponCooldown); }
        set {
            if (value != hasWeaponCooldown) {
                var index = GameComponentsLookup.WeaponCooldown;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : weaponCooldownComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherWeaponCooldown;

    public static Entitas.IMatcher<GameEntity> WeaponCooldown {
        get {
            if (_matcherWeaponCooldown == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.WeaponCooldown);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherWeaponCooldown = matcher;
            }

            return _matcherWeaponCooldown;
        }
    }
}
