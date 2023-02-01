// using System.Collections.Generic;
// using Entitas;
// using UnityEngine;
//
// public class FloatDamageSystem : ReactiveSystem<GameEntity>
// {
//     Contexts _contexts;
//
//     public FloatDamageSystem(Contexts contexts) : base(contexts.game)
//     {
//         _contexts = contexts;
//     }
//
//     protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
//     {
//         return context.CreateCollector(GameMatcher.AllOf(GameMatcher.ReceivedFloatDamage));;
//     }
//
//     protected override bool Filter(GameEntity entity)
//     {
//         return entity.hasReceivedFloatDamage && entity.receivedFloatDamage.Value > 1;
//     }
//
//     protected override void Execute(List<GameEntity> entities)
//     {
//         foreach (var e in entities)
//         {
//             int totalDamage = Mathf.FloorToInt(e.receivedFloatDamage.Value);
//             e.ReplaceReceivedFloatDamage(e.receivedFloatDamage.Value - totalDamage);
//             if (e.hasReceivedDamage)
//             {
//                 totalDamage += e.receivedDamage.Damage;
//             }
//             
//             e.ReplaceReceivedDamage(totalDamage);
//         }
//     }
// }