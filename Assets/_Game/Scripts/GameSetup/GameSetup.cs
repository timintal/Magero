using System;
using _Game.Data;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[CreateAssetMenu]
[Game, Unique]
public class GameSetup : ScriptableObject
{
    
    public CameraSettings CameraSettings;
    public FlowFieldSettings FlowFieldSettings;
    public DebugSettings DebugSettings;
    public WeaponSettings[] WeaponSettings;
    public PlayerSettings PlayerSettings;

    public GameObject DamageUIOverlayPrefab;

    public WeaponSettings GetWeaponSettings(WeaponType t)
    {
        foreach (var weaponSetting in WeaponSettings)
        {
            if (weaponSetting.Type == t)
                return weaponSetting;
        }

        return null;
    }
}

[Serializable]
public class CameraSettings
{
    public float RotationSpeed;
    public Vector2 HorizontalBounds;
    public Vector2 VerticalBounds;
}

[Serializable]
public class FlowFieldSettings
{
    public float CellSize;
    
    public int StepWeight;
    public int StepDiagonalWeight;

    public int MaxCalculationDistance;
    
    public int MoverRepulsionValue;

    public int ObstacleRepulsionSize;
    public int ObstacleRepulsionValue;
    
    public float ExplosionRepulsionTime;
    public int ExplosionRepulsionValue;
    public int ExplosionRepulsionSizeMultiplier;
    
    public float FlowFieldUpdateCooldown;
}

[Serializable]
public class DebugSettings
{
    public bool VisualizeGroundEnemyFlowField;
    public bool VisualizeFlyingEnemyFlowField;
    public bool VisualizeSummonFlowField;
    public Material FlowFieldMaterial;
    public Mesh CellMesh;
    public int MaxValueColor;
    public Gradient mapGradient;
}

[Serializable]
public class PlayerSettings
{
    public float DistanceForDamage;
}



