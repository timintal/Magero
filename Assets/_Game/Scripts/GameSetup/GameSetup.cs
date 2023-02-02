using System;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[CreateAssetMenu]
[Game, Unique]
public class GameSetup : ScriptableObject
{
    public bool AddLaser;
    public bool AddFireballs;
    public bool AddLightning;
    public bool AddAcid;
    public bool AddGasCloud;
    public bool AddWind;
    public CameraSettings CameraSettings;
    public FlowFieldSettings FlowFieldSettings;
    public DebugSettings DebugSettings;
    public FireballSettings FireballSetings;
    public LaserSettings LaserSettings;
    public LightningStrikeSettings LightningStrikeSettings;
    public AcidStreamSettings AcidStreamSettings;
    public GasCloudSettings GasCloudSettings;
    public WindGustSettings WindGustSettings;
    public BlackHoleSettings BlackHoleSettings;
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
    public float Damage;
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
    public float RefreshTimestamp;
    public AnimationCurve RadiusCurve;
}

[Serializable]
public class PlayerSettings
{
    public int Health;
    public float DistanceForDamage;
}

[Serializable]
public class GasCloudSettings
{
    public float Cooldown;
    public float Damage;
    public GameObject ProjectilePrefab;
    public GameObject CloudPrefab;
    public float ProjectileSpeed;
    public float CloudRadius;
    public float CloudSpeedMultiplier;
    public float CloudLifetime;
}

[Serializable]
public class WindGustSettings
{
    public float Damage;
    public float PushSpeed;
    public float PushDamping;
    public float WindStreamRadius;
    public float MaxDistance;
}

[Serializable]
public class BlackHoleSettings
{
    public float Damage;
    public float ExplosionRadius;
    public float PullSpeed;
    public float PullRadius;
    public float Lifetime;
    public GameObject ProjectilePrefab;
    public GameObject BlackHolePrefab;

}

