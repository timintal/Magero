using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class FlowFieldMoverComponent : IComponent
{
    [EntityIndex] 
    public int FlowFieldIndex;
}