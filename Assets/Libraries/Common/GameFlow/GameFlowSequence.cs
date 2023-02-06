using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Game.Flow
{
    [Serializable]
    public abstract class GameFlowSequence : SequenceAction
    {
        private List<SequenceAction> _actions = new();
        public IReadOnlyList<SequenceAction> Actions => _actions;

        public void AddAction(SequenceAction action)
        {
            int index = 0;
            for (int i = _actions.Count - 1; i >= 0; i--)
            {
                if (_actions[i].Priority >= action.Priority)
                {
                    index = i + 1;
                    break;
                }
            }

            _actions.Insert(index, action);
        }

        public void RemoveActions<T>() where T : SequenceAction
        {
            for (int i = _actions.Count - 1; i >= 0; i--)
            {
                if (_actions[i] is T)
                {
                    _actions.RemoveAt(i);
                }
            }
        }

        public void RemoveAction(SequenceAction action)
        {
            for (int i = _actions.Count - 1; i >= 0; i--)
            {
                if (_actions[i] == action)
                {
                    _actions.RemoveAt(i);
                }
            }
        }

        public void ClearSequence()
        {
            _actions.Clear();
        }
        
        public override async UniTask Execute(CancellationToken token)
        {
            foreach (var action in _actions)
            {
                if (token.IsCancellationRequested)
                    return;

                await action.Execute(token);
            }
        }
    }
}