using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique]
public class GameSceneReferences : MonoBehaviour
{
    public Transform CameraTransform;
    public Joystick Joystick;
    
    public Transform ShootTransform;

}
