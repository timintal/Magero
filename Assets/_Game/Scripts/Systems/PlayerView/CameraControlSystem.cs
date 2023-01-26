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
        var camera = cameraEntity.camera;

        float xOffset = input.Axis.x * camera.RotationSpeed * Time.deltaTime;
        float yOffset = -input.Axis.y * camera.RotationSpeed * Time.deltaTime;

        var currentRotation = camera.CameraTransform.localRotation.eulerAngles;
        
        if (currentRotation.x > 180) currentRotation.x -= 360;
        if (currentRotation.y > 180) currentRotation.y -= 360;
        
        currentRotation.x = Mathf.Clamp(currentRotation.x + yOffset, camera.VerticalRange.x, camera.VerticalRange.y);
        currentRotation.y = Mathf.Clamp(currentRotation.y + xOffset, camera.HorizontalRange.x, camera.HorizontalRange.y);
        
        camera.CameraTransform.localRotation = Quaternion.Euler(currentRotation);
    }
}