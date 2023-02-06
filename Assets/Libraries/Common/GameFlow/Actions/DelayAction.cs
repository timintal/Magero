using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Game.Flow
{
    public class DelayAction : SequenceAction
    {
        private readonly float _delay;

        public DelayAction(float delay)
        {
            _delay = delay;
        }
        
        public override async UniTask Execute(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token);
        }
    }
}