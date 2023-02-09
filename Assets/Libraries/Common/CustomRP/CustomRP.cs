using UnityEngine;
using UnityEngine.Rendering;


public class CustomRP : RenderPipeline
{
    private CustomCameraRenderer _renderer = new CustomCameraRenderer();
    private readonly bool useGPUInstancing;

    public CustomRP()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }

    public CustomRP(bool useGPUInstancing, bool useSrpBatcher)
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = useSrpBatcher;
        this.useGPUInstancing = useGPUInstancing;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            _renderer.Render(context, cameras[i], useGPUInstancing);
        }
    }
}