using Entitas;
using UnityEngine;

[Game]
public class RagdollDeathComponent : IComponent
{
}

[Game]
public class RagdollCurrentVelocityComponent : IComponent
{
    public Vector3 Value;
}

[Game]
public class RagdollAngularVelocityComponent : IComponent
{
    public Vector3 Axis;
    public float AnglePerSecond;
}

public class RagdollRemoveTimerComponent : IComponent
{
    public float TimeLeft;
}

