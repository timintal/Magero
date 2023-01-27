using Entitas;

[Game]
public class EnemySpawnRequestComponent : IComponent
{
    public EnemySettings EnemySettings;
    public int Count;
}