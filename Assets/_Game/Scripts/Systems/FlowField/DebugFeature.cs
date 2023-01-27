public class DebugFeature : Feature
{
    public DebugFeature(Contexts contexts)
    {
        Add(new DebugKillUnitsSystem(contexts));
    }
}