using System;
using UnityEngine;

namespace EasyTweens
{
    [Serializable]
    public class TweenBase : MonoBehaviour // To be able to serialize generic fields
    {
        public float delay;
        public float duration;
        public AnimationCurve curve;
        
        public virtual void SetFactor(float f)
        {
            
        }

        public void UpdateTween(float time)
        {
            float currentFactor = 0;

            if (time > delay + duration)
            {
                currentFactor = 1;
            }
            else if (time > delay)
            {
                currentFactor = (time - delay) / (duration);
            }
            
            SetFactor(curve.Evaluate(currentFactor));
        }
        
        #if UNITY_EDITOR
        public virtual void SetCurrentAsStartValue(){}
        public virtual void SetCurrentAsEndValue(){}
        #endif
       
    }
}

