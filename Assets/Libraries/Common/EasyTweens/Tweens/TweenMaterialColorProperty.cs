using UnityEngine;

namespace EasyTweens
{
    public class TweenMaterialColorProperty : ColorTween<Renderer>
    {
        public static string TweenName => "Renderer Color";
        [ExposeInEditor]
        public string PropertyName;
        
        
        protected override Color Property
        {
            get => target.material.GetColor(PropertyName);
            set => target.material.SetColor(PropertyName, value);
        }
    }
}