using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(menuName = "Rendering/RP/CustomRPAsset")]
public class CustomRPAsset : RenderPipelineAsset
{
    public bool useGPUInstancing, useSRPBatcher;
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRP(useGPUInstancing, useSRPBatcher);
    }
}