using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField] private Vector3 _startRGBValues = Vector3.zero;
    [SerializeField] private Vector3 _endRGBValues = Vector3.one;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void OnEnable()
    {
        Renderer rend = GetComponent<Renderer>();
        Color color = new Color(Random.Range(_startRGBValues.x, _endRGBValues.x),
								Random.Range(_startRGBValues.y, _endRGBValues.y),
								Random.Range(_startRGBValues.z, _endRGBValues.z),
                                1f);

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);
        block.SetColor(BaseColor, color);
        rend.SetPropertyBlock(block);
    }
}
