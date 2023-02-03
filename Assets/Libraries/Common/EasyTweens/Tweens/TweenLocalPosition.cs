using System;
using UnityEngine;

namespace EasyTweens
{
    [Serializable]
    public class TweenLocalPosition : Vector3Tween<Transform>
    {
        public static string TweenName => "Local Position";

        protected override Vector3 Property
        {
            get => target.localPosition;
            set => target.localPosition = value;
        }
    }
}