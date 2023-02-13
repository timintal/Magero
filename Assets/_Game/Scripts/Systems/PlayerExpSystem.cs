using System.Collections.Generic;
using _Game.Data;
using Entitas;

public class PlayerExpSystem : ReactiveSystem<GameEntity>
{
    private readonly PlayerData _playerData;

    public PlayerExpSystem(IContext<GameEntity> context, PlayerData playerData) : base(context)
    {
        _playerData = playerData;
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroyed &&
               entity.hasExp;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            _playerData.PlayerExp += e.exp.Value;
        }
    }
}