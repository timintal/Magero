using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using UnityEngine.UI;

[Game, Unique]
public class GameSceneReferences : MonoBehaviour
{
    public Transform CameraTransform;
    public Joystick Joystick;
    
    public LineRenderer LaserRenderer;
    public Transform LaserSparkles;
    
    public LineRenderer AcidRenderer;

    public Image PlayerHealthBar;
    
    public ArmView[] Arms;

}
