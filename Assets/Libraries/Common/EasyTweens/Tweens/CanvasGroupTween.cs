using UnityEngine;

namespace EasyTweens
{
    public class CanvasGroupTween : FloatTween<CanvasGroup>
    {
        public static string TweenName => "Canvas Group Alpha";
        
        protected override float Property
        {
            get => target.alpha;
            set => target.alpha = value;
        }
    }
}