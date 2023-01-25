using Entitas;
using UnityEngine;

public class UserInputSystem : IExecuteSystem, ICleanupSystem, IInitializeSystem
{
    private readonly Contexts _contexts;

    private readonly IGroup<InputEntity> _userInputs;
    private readonly IGroup<InputEntity> _joystickGroup;

    public UserInputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _userInputs = _contexts.input.GetGroup(InputMatcher.UserInput);
        _joystickGroup = _contexts.input.GetGroup(InputMatcher.Joystick);
    }

    public void Execute()
    {
        Vector2 currentInput;
        var joystick = _joystickGroup.GetSingleEntity().joystick;
        if (joystick.Joystick.Direction != Vector2.zero)
        {
            currentInput = joystick.Joystick.Direction;
        }
        else
        {
            currentInput.x = Input.GetAxis("Horizontal");
            currentInput.y = Input.GetAxis("Vertical");
            if (currentInput.x > float.Epsilon && currentInput.y > float.Epsilon)
            {
                currentInput.Normalize();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount == 4)
        {
            var spawnRequest = _contexts.game.CreateEntity();
            spawnRequest.AddEnemySpawnRequest(100);
        }
        
        _contexts.input.CreateEntity().AddUserInput(currentInput);
    }

    public void Cleanup()
    {
        foreach (var entity in _userInputs.GetEntities())
        {
            entity.Destroy();
        }
    }

    public void Initialize()
    {
        var gameSceneReferences = _contexts.game.gameSceneReferences.value;
        var joystickEntity = _contexts.input.CreateEntity();
        joystickEntity.AddJoystick(gameSceneReferences.Joystick);

    }
}