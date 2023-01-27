using Entitas;

[Game]
public class LevelComponent : IComponent
{
    public LevelStage[] Stages;
}

[Game]
public class CurrentLevelStageComponent : IComponent
{
    public int Index;
}

[Game]
public class LevelFinishedComponent : IComponent
{
    
}