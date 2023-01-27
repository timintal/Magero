using UnityEngine;

public class ScaleRandomizer : MonoBehaviour
{
	[SerializeField] private Vector2 _scaleMinMax = Vector2.one;

	private void Awake()
	{
		transform.localScale *= Random.Range(_scaleMinMax.x, _scaleMinMax.y);
	}
}
