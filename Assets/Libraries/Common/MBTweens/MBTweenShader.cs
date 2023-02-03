using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBTweens
{
	public class MBTweenShader : MBTweenBase
	{
		[SerializeField] protected List<Renderer> renderers = new List<Renderer>();
		[SerializeField] protected string propertyName;

		public List<Renderer> Renderers => renderers;

		public string PropertyName
		{
			get { return propertyName; }
			set { propertyName = value; }
		}

		private MaterialPropertyBlock _materialPropertyBlock;

		protected MaterialPropertyBlock PropertyBlock
		{
			get
			{
				if (_materialPropertyBlock == null)
				{
					_materialPropertyBlock = new MaterialPropertyBlock();
				}

				return _materialPropertyBlock;
			}
		}
	}
}