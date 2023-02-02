using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class SummonUnitsSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private int _layerMask;

    private IGroup<GameEntity> _groundFlowField;
    private IGroup<GameEntity> _summonFlowField;

    public SummonUnitsSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        
        _groundFlowField =
            _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.GroundEnemyFlowField));
        
        _summonFlowField =
            _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.SummonFlowField));
        
        _layerMask = LayerMask.GetMask("Environment");
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.SummonSpell.Added(),
            GameMatcher.WeaponCooldown.Removed(),
            GameMatcher.WeaponDisabled.Removed());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasSummonSpell &&
               entity.hasTransform &&
               entity.hasDirection &&
               !entity.isWeaponDisabled &&
               !entity.hasWeaponCooldown;
    }

    protected override void Execute(List<GameEntity> entities)
    {

        var groundFlowField = _groundFlowField.GetSingleEntity();
        var summonFlowField = _summonFlowField.GetSingleEntity();

        foreach (var e in entities)
        {
            if (Physics.Raycast(e.transform.Transform.position,
                    e.direction.Value,
                    out RaycastHit hit,
                    1000,
                    _layerMask))
            {
                if (hit.point.y < 1 && groundFlowField.flowField.IsPassablePosition(hit.point, int.MaxValue - 100))
                {
                    for (int i = 0; i < e.summonSpell.UnitsCount; i++)
                    {
                        Vector3 pos = new Vector3(hit.point.x + Random.Range(-1f, 1f), 0,
                            hit.point.z + Random.Range(-1f, 1f));

                        var summonEntity = _contexts.game.CreateEntity();
                        summonEntity.AddPosition(pos);
                        summonEntity.AddRotation(Quaternion.identity);
                        summonEntity.AddRadius(e.summonSpell.UnitRadius);
                        summonEntity.isSummon = true;
                        summonEntity.AddDamage(e.damage.Value);
                        summonEntity.AddAttacker(e.attacker.TargetType, e.attacker.TargetMask);
                        summonEntity.AddSpeed(e.summonSpell.UnitsSpeed, e.summonSpell.UnitsSpeed);
                        summonEntity.AddFlowFieldMover(summonFlowField.id.Value);
                        summonEntity.AddResource(e.assetLink.Asset);
                        summonEntity.AddAutoDestruction(e.summonSpell.UnitLifetime);
                        summonEntity.isOnDestroyFx = true;
                        summonEntity.AddAssetLink(e.summonSpell.UnitExplosionPrefab);
                    }
                }
            }

            WeaponCooldownComponent.StartWeaponCooldown(e, e.summonSpell.Cooldown, _contexts.game);
        }
    }
}