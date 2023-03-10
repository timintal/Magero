using System.Collections.Generic;
using UnityEngine;

namespace MBTweens
{
	public class MBTweenShaderColor : MBTweenShader
	{
		[SerializeField] private Color startColor;
		[SerializeField] private Color endColor;

		protected override void UpdateTweenWithFactor(float factor)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].GetPropertyBlock(PropertyBlock);

				PropertyBlock.SetColor(propertyName, Color.Lerp(startColor, endColor, factor));

				renderers[i].SetPropertyBlock(PropertyBlock);
			}

		}
	}
}
