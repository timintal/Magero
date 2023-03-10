//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly LaserShooterComponent laserShooterComponent = new LaserShooterComponent();

    public bool isLaserShooter {
        get { return HasComponent(GameComponentsLookup.LaserShooter); }
        set {
            if (value != isLaserShooter) {
                var index = GameComponentsLookup.LaserShooter;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : laserShooterComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherLaserShooter;

    public static Entitas.IMatcher<GameEntity> LaserShooter {
        get {
            if (_matcherLaserShooter == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LaserShooter);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLaserShooter = matcher;
            }

            return _matcherLaserShooter;
        }
    }
}
