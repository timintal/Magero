using System;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
[Game, Unique]
public class GameSetup : ScriptableObject
{
    public bool AddLaser;
    public bool AddFireballs;
    public bool AddLightning;
    public bool AddAcid;
    public CameraSettings CameraSettings;
    public FlowFieldSettings FlowFieldSettings;
    public DebugSettings DebugSettings;
    public FireballSettings FireballSetings;
    public LaserSettings LaserSettings;
    public LightningStrikeSettings LightningStrikeSettings;
    public AcidStreamSettings AcidStreamSettings;
    public PlayerSettings PlayerSettings;
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
public class FireballSettings
{
    public float Cooldown;
    public int Damage;
    public GameObject ProjectilePrefab;
    public GameObject ExplosionVisualPrefab;
    public float ProjectileSpeed;
    public float ExplosionRadius;
}

[Serializable]
public class LaserSettings
{
    public float DamagePerSecond;
}

[Serializable]
public class LightningStrikeSettings
{
    public float Cooldown;
    public float EffectRadius;
    public int TargetDamage;
    public int AOEDamage;
    public float StunDuration;
    public GameObject ImpactFx;
}

[Serializable]
public class AcidStreamSettings
{
    public float Cooldown;
    public float PoolRadius;
    public GameObject PuddlePrefab;
    public float DamagePerSecond;
    public float PuddleLifetime;
    public AnimationCurve RadiusCurve;
}

[Serializable]
public class PlayerSettings
{
    public int Health;
    public float DistanceForDamage;
}

