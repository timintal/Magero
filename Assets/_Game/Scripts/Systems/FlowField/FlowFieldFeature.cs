public class FlowFieldFeature : Feature
{
    public FlowFieldFeature(Contexts contexts)
    {
        Add(new UpdateFlowFieldSystem(contexts));
        Add(new FlowFieldMovementSystem(contexts));
        Add(new FlowFieldUpdateCooldownSystem(contexts));
        #if UNITY_EDITOR
        Add(new VisualizeFlowFieldSystem(contexts));
        #endif
    }
}