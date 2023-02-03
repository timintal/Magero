using System;
using UnityEngine;

namespace EasyTweens
{
    [Serializable]
    public class TweenLocalScale : Vector3Tween<Transform>
    {
        public static string TweenName => "Local Scale";

        protected override Vector3 Property
        {
            get => target.localScale;
            set => target.localScale = value;
        }
    }
}