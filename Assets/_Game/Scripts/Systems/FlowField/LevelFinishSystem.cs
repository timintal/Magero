using System.Collections.Generic;
using _Game.Flow;
using Entitas;

public class LevelFinishSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly GameFSM _gameFsm;

    public LevelFinishSystem(Contexts contexts, GameFSM gameFsm) : base(contexts.game)
    {
        _contexts = contexts;
        _gameFsm = gameFsm;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.LevelFinished);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLevelFinished;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.levelFinished.IsWin)
            {
                _gameFsm.GoTo<LevelWonState>();
            }
            else
            {
                _gameFsm.GoTo<GameOverState>();
            }
        }
    }
}