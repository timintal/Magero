using UnityEngine;
using UnityEngine.UI;

namespace MBTweens
{
	public class MBTweenSpriteAnimation : MBTweenBase
	{

		[SerializeField] private Image targetImage;
		[SerializeField] private Sprite[] sprites;

//		private int currentSpriteIndex = 0;

		protected override void UpdateTweenWithFactor(float factor)
		{
			if (sprites.Length == 0)
			{
				return;
			}

			float factorPerSprite = 1f / sprites.Length;

			int index = Mathf.FloorToInt(factor / factorPerSprite);

			if (index >= sprites.Length)
			{
				index = 0;
			}

			targetImage.sprite = sprites[index];

//		if (factor - 1 < float.Epsilon)
//		{
//			currentSpriteIndex++;
//			if (currentSpriteIndex == sprites.Length)
//			{
//				currentSpriteIndex = 0;
//			}
//
//			targetImage.sprite = sprites[currentSpriteIndex];
//		}
		}


	}
}
