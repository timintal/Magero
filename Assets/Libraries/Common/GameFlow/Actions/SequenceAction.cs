
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Game.Flow
{
    [Serializable]
    public abstract class SequenceAction
    {
        public static readonly int DefaultPriority = 0;
        public static readonly int HighPriority = 100;
        public static readonly int HighestPriority = 200;
        public static readonly int LowPriority = -100;
        public static readonly int LowestPriority = -200;
        
        private readonly int _priority;

        public int Priority => _priority;

        public abstract UniTask Execute(CancellationToken token);

        protected SequenceAction()
        {
            _priority = DefaultPriority;
        }

        protected SequenceAction(int priority)
        {
            _priority = priority;
        }
    }
}