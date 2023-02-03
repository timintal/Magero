using System;
using UnityEngine;

namespace EasyTweens
{
    [Serializable]
    public class TweenLocalRotation : QuaternionTween<Transform>
    {
        public static string TweenName => "Local Rotation";

        protected override Quaternion Property
        {
            get => target.localRotation;
            set => target.localRotation = value;
        }
    }
}