using Entitas;
using UnityEngine;

public class CameraControlSystem : IExecuteSystem
{
    Contexts _contexts;
    private readonly IGroup<InputEntity> _userInputGroup;
    private IGroup<GameEntity> _cameraGroup;

    public CameraControlSystem(Contexts contexts)
    {
        _contexts = contexts;
        _userInputGroup = contexts.input.GetGroup(InputMatcher.UserInput);
        _cameraGroup = contexts.game.GetGroup(GameMatcher.Camera);
    }

    public void Execute()
    {
        var input = _userInputGroup.GetSingleEntity().userInput;
        var cameraEntity = _cameraGroup.GetSingleEntity();
        var camera = cameraEntity;

        float xOffset = input.Axis.x / Screen.width * camera.camera.RotationSpeed;
        float yOffset = -input.Axis.y / Screen.height * camera.camera.RotationSpeed;

        var currentQuaternion = camera.transform.Transform.rotation;
        var currentRotation = currentQuaternion.eulerAngles;

        currentRotation = currentRotation - camera.camera.InitialForwardOffset.eulerAngles;
        
        if (currentRotation.x > 180) currentRotation.x -= 360;
        if (currentRotation.y > 180) currentRotation.y -= 360;
        if (currentRotation.x < -180) currentRotation.x += 360;
        if (currentRotation.y < -180) currentRotation.y += 360;
        

        currentRotation.x = Mathf.Clamp(currentRotation.x + yOffset,
            camera.camera.VerticalRange.x,
            camera.camera.VerticalRange.y);
        
        currentRotation.y = Mathf.Clamp(currentRotation.y + xOffset,
            camera.camera.HorizontalRange.x,
            camera.camera.HorizontalRange.y);

        camera.transform.Transform.rotation = Quaternion.Euler(currentRotation + camera.camera.InitialForwardOffset.eulerAngles);
    }
}