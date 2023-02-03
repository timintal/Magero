using System;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTweens
{
    [Serializable]
    public class TweenGraphicColorProperty : ColorTween<Graphic>
    {
        public static string TweenName => "UI Graphic Color";

        protected override Color Property
        {
            get => target.color;
            set => target.color = value;
        }
    }
}