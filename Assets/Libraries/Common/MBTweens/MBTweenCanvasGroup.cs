using UnityEngine;

namespace MBTweens
{
    public class MBTweenCanvasGroup : MBTweenBase
    {
        [SerializeField, Range(0f, 1f)] private float startAlpha = 0f;
        [SerializeField, Range(0f, 1f)] private float endAlpha = 1f;

        [SerializeField] private CanvasGroup target = null;
        

        protected override float GetFactor(int startFactor)
        {
            if (smooth)
            {
                float current = target.alpha;
                var factor = Mathf.Abs(startAlpha - current) / Mathf.Abs(endAlpha - startAlpha);
                return factor;
            }
            
            return startFactor;
        }
        
        
        protected override void UpdateTweenWithFactor(float factor)
        {
            base.UpdateTweenWithFactor(factor);

            target.alpha = startAlpha + (endAlpha - startAlpha) * factor;
        }

    }
}