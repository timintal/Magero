//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity {

    public UserInputComponent userInput { get { return (UserInputComponent)GetComponent(InputComponentsLookup.UserInput); } }
    public bool hasUserInput { get { return HasComponent(InputComponentsLookup.UserInput); } }

    public void AddUserInput(UnityEngine.Vector2 newAxis) {
        var index = InputComponentsLookup.UserInput;
        var component = (UserInputComponent)CreateComponent(index, typeof(UserInputComponent));
        component.Axis = newAxis;
        AddComponent(index, component);
    }

    public void ReplaceUserInput(UnityEngine.Vector2 newAxis) {
        var index = InputComponentsLookup.UserInput;
        var component = (UserInputComponent)CreateComponent(index, typeof(UserInputComponent));
        component.Axis = newAxis;
        ReplaceComponent(index, component);
    }

    public void RemoveUserInput() {
        RemoveComponent(InputComponentsLookup.UserInput);
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
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherUserInput;

    public static Entitas.IMatcher<InputEntity> UserInput {
        get {
            if (_matcherUserInput == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.UserInput);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherUserInput = matcher;
            }

            return _matcherUserInput;
        }
    }
}
