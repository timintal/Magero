using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique]
public class GameSceneReferences : MonoBehaviour
{
    public Transform CameraTransform;
    
    public LineRenderer LaserRenderer;
    public Transform LaserSparkles;
    
    public LineRenderer AcidRenderer;
    
    public ArmView[] Arms;

}
