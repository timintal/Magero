using Entitas;
using UnityEngine;

[Game]
public class AcidPuddleComponent : IComponent
{
    public float PuddleLifetime;
    public float InitialRadius;
    public AnimationCurve RadiusCurve;
}