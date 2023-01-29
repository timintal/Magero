using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using UnityEngine.VFX;

[Game, Unique]
public class GameSceneReferences : MonoBehaviour
{
    public Transform CameraTransform;
    public Joystick Joystick;
    
    public Transform FireballsShootTransform;
    public Transform LaserShootTransform;

    public LineRenderer LaserRenderer;
    public Transform LaserSparkles;

}
