using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MBTweens
{
    public class MBTweenMaterialColor : MBTweenBase
    {

        [SerializeField] private Material[] materials;
        [SerializeField] private string colorShaderName;

        
        [SerializeField] Color startColor;
        [SerializeField] Color endColor;
        
        
        protected override void UpdateTweenWithFactor(float factor)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor(colorShaderName, Color.Lerp(startColor, endColor, factor));
            }

        }
    }

}

