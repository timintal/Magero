using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField] private Vector3 _startRGBValues = Vector3.zero;
    [SerializeField] private Vector3 _endRGBValues = Vector3.one;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color color = new Color(Random.Range(_startRGBValues.x, _endRGBValues.x),
								Random.Range(_startRGBValues.y, _endRGBValues.y),
								Random.Range(_startRGBValues.z, _endRGBValues.z),
                                1f);

        renderer.material.color = color;
    }
}
