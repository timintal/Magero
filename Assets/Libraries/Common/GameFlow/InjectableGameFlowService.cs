using System.Collections.Generic;
using _Game.Flow;
using VContainer.Unity;

namespace _Game.Flow
{
    public class InjectableGameFlowService : GameFlowService, IStartable
    {
        private readonly IReadOnlyList<GameFlowSequence> _sequences;

        public InjectableGameFlowService(IReadOnlyList<GameFlowSequence> sequences)
        {
            _sequences = sequences;
        }

        public void Start()
        {
            foreach (var flowSequence in _sequences)
            {
                AddSequence(flowSequence);
            }
        }
    }
}
