using UnityEngine;
using Sirenix.OdinInspector;

public class ColorRandomizer : MonoBehaviour
{
	[SerializeField] private bool _uniform = false;
	[HideIf("_uniform")][SerializeField] private Vector3 _startRGBValues = Vector3.zero;
	[HideIf("_uniform")] [SerializeField] private Vector3 _endRGBValues = Vector3.one;
	[ShowIf("_uniform")] [SerializeField] private float _startValue = 0f;
	[ShowIf("_uniform")] [SerializeField] private float _endValue = 1f;
	private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void OnEnable()
    {
        Renderer rend = GetComponent<Renderer>();
		Color color;
		if (_uniform)
		{
			float random = Random.Range(_startValue, _endValue);
			color = new Color(random, random, random, 1f);
		}
		else
		{
			color = new Color(Random.Range(_startRGBValues.x, _endRGBValues.x),
								Random.Range(_startRGBValues.y, _endRGBValues.y),
								Random.Range(_startRGBValues.z, _endRGBValues.z),
								1f);
		}


		rend.material.color = color;
    }
}
