using Magero.UIFramework;

public class EnemyPointersFeature : Feature
{
    public EnemyPointersFeature(Contexts contexts, UIFrame uiFrame) : base("Enemy Pointers Feature")
    {
        Add(new OutOfScreenEnemiesSystem(contexts));
        Add(new EnemyPointersPositionSystem(contexts, uiFrame));
    }
}