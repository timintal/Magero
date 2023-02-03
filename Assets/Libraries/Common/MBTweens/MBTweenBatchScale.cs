using MBTweens;
using UnityEngine;

public class MBTweenBatchScale : MBTweenScale
{
    [SerializeField] private Transform[] otherTarget;

    protected override Vector3 Scale
    {
        get => base.Scale;
        set
        {
            base.Scale = value;
            foreach (Transform t in otherTarget)
            {
                t.localScale = value;
            }
        }
    }
}
