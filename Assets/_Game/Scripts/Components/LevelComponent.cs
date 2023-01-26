using Entitas;

public class LevelComponent : IComponent
{
    public LevelStage[] Stages;
}

public class CurrentLevelStageComponent : IComponent
{
    public int Index;
}

public class LevelFinishedComponent : IComponent
{
    
}