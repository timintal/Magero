using UnityEngine;

namespace MBTweens
{
    public class MBTweenRectTransformSize : MBTweenBase
    {
        [SerializeField] private Vector2 startSize;
        [SerializeField] private Vector2 endSize;
        [SerializeField] RectTransform target;

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = GetComponent<RectTransform>();
            }
        }

        protected override void UpdateTweenWithFactor(float factor)
        {
            target.sizeDelta = Vector2.Lerp(startSize, endSize, factor);
        }
    }
}