using Entitas;
using UnityEngine;

public class DebugKillUnitsSystem : IExecuteSystem
{
    Contexts _contexts;

    public DebugKillUnitsSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var enemieTargets = _contexts.game.GetEntitiesWithTarget(TargetType.Enemy);
            foreach (var e in enemieTargets)
            {
                if (e.hasFlowFieldMover)
                    e.isDestroyed = true;
            }
        }
    }
}