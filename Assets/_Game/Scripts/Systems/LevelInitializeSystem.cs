using Entitas;

public class LevelInitializeSystem : IInitializeSystem
{
    private readonly Level _levelToLoad;
    private readonly Contexts _contexts
        ;

    public LevelInitializeSystem(Level level, Contexts contexts)
    {
        _levelToLoad = level;
        _contexts = contexts;
    }
    public void Initialize()
    {
        var flowFieldEntity = _contexts.game.CreateEntity();

        int[][] flowField = new int[_levelToLoad.width][];
        int[][] currentField = new int[_levelToLoad.width][];
        int[][] backFiled = new int[_levelToLoad.width][];
        
        for (int i = 0; i < flowField.Length; i++)
        {
            flowField[i] = new int[_levelToLoad.height];
            currentField[i] = new int[_levelToLoad.height];
            backFiled[i] = new int[_levelToLoad.height];
        }

        var gameSetup = _contexts.game.gameSetup.value;
        
        for (int i = 0; i < _levelToLoad.width; i++)
        {
            for (int j = 0; j < _levelToLoad.height; j++)
            {
                flowField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                currentField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                backFiled[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
            }
        }

        var initialPos = _levelToLoad.transform.position;

        foreach (var obstacle in _levelToLoad.obstacles)
        {
            for (int i = 0; i < obstacle.width; i++)
            {
                for (int j = 0; j < obstacle.height; j++)
                {
                    flowField[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                }
            }
        }

        flowFieldEntity.AddFlowField(initialPos, _levelToLoad.cellSize, flowField, currentField, backFiled);

    }
}