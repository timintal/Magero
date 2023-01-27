using Entitas;
using UnityEngine;

public class UserInputSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;

    private readonly IGroup<InputEntity> _userInputs;

    public UserInputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _userInputs = _contexts.input.GetGroup(InputMatcher.UserInput);
    }

    public void Execute()
    {
        var inputEntity = _userInputs.GetSingleEntity();

        var input = inputEntity.userInput;
        if (Input.GetMouseButtonDown(0))
        {
            input.PreviousPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            input.Axis = (Vector2)Input.mousePosition - input.PreviousPosition;
            input.PreviousPosition = Input.mousePosition;
            input.IsFirePressed = true;
        }
        else
        {
            input.Axis = Vector2.zero;
            input.PreviousPosition = Vector2.zero;
            input.IsFirePressed = false;
        }
        
        inputEntity.ReplaceUserInput(input.PreviousPosition, input.Axis, input.IsFirePressed);
    }

    

    public void Initialize()
    {
        var gameSceneReferences = _contexts.game.gameSceneReferences.value;
        var joystickEntity = _contexts.input.CreateEntity();
        joystickEntity.AddJoystick(gameSceneReferences.Joystick);
        _contexts.input.CreateEntity().AddUserInput(Vector2.zero, Vector2.zero, false);

    }
}