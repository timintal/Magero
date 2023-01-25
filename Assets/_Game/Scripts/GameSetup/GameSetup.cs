using System;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[CreateAssetMenu]
[Game, Unique]
public class GameSetup : ScriptableObject
{
    public CameraSettings CameraSettings;
    public FlowFieldSettings FlowFieldSettings;
    public DebugSettings DebugSettings;
    public TestEnemySettings TestEnemySettings;
    public TestWeaponSettings TestWeaponSettings;
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
    public int StepWeight;
    public int StepDiagonalWeight;

    public int MaxCalculationDistance;
    
    public int MoverRepulsionSize;
    public int MoverRepulsionValue;
    
    public float ExplosionRepulsionTime;
    public int ExplosionRepulsionValue;
    public int ExplosionRepulsionSizeMultiplier;
}

[Serializable]
public class DebugSettings
{
    public bool VisualizeFlowField;
    public Material FlowFieldMaterial;
    public Mesh CellMesh;
    public int MaxValueColor;
    public Gradient mapGradient;
}

[Serializable]
public class TestEnemySettings
{
    public GameObject Prefab;

    public float Speed;

    public int Health;
}

[Serializable]
public class TestWeaponSettings
{
    public float Cooldown;
    public int Damage;
    public GameObject ProjectilePrefab;
    public GameObject ExplosionVisualPrefab;
    public float ProjectileSpeed;
    public float ExplosionRadius;
}


