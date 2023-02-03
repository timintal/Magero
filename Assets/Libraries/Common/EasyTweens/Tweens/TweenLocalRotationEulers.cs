using System;
using UnityEngine;

namespace EasyTweens
{
    [Serializable]
    public class TweenLocalRotationEulers : Vector3Tween<Transform>
    {
        public static string TweenName => "Local Rotation Eulers";

        protected override Vector3 Property
        {
            get => target.localRotation.eulerAngles;
            set => target.localRotation = Quaternion.Euler(value);
        }
    }
}