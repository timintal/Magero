using System;
using UnityEngine;

namespace EasyTweens
{
    [Serializable]
    public abstract class TargetedTween<T1, T2>: TweenBase where T1 : UnityEngine.Object
    {
        public T1 target;
        public T2 startValue;
        public T2 endValue;

        protected abstract T2 Property
        {
            get;
            set;
        }

        public override void SetFactor(float f)
        {
            Property = Lerp(f);
        }

        public override void SetCurrentAsEndValue()
        {
            endValue = Property;
        }

        public override void SetCurrentAsStartValue()
        {
            startValue = Property;
        }

        protected abstract T2 Lerp(float factor);

    }

    public abstract class FloatTween<T> : TargetedTween<T, float> where T : UnityEngine.Object
    {
        protected override float Lerp(float factor)
        {
            return Mathf.LerpUnclamped(startValue, endValue, factor);
        }
    }
    
    public abstract class Vector2Tween<T> : TargetedTween<T, Vector2> where T : UnityEngine.Object
    {
        protected override Vector2 Lerp(float factor)
        {
            return Vector2.LerpUnclamped(startValue, endValue, factor);
        }
    }
    
    public abstract class Vector3Tween<T> : TargetedTween<T, Vector3> where T : UnityEngine.Object
    {
        protected override Vector3 Lerp(float factor)
        {
            return Vector3.LerpUnclamped(startValue, endValue, factor);
        }
    }
    
    public abstract class ColorTween<T> : TargetedTween<T, Color> where T : UnityEngine.Object
    {
        protected override Color Lerp(float factor)
        {
            return Color.LerpUnclamped(startValue, endValue, factor);
        }
    }
    
    public abstract class QuaternionTween<T> : TargetedTween<T, Quaternion> where T : UnityEngine.Object
    {
        protected override Quaternion Lerp(float factor)
        {
            return Quaternion.LerpUnclamped(startValue, endValue, factor);
        }
    }
}