using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Flow
{
    public class GameFlowService 
    {
        private Dictionary<Type, GameFlowSequence> _sequences = new();
        
        protected void AddSequence(GameFlowSequence sequence)
        {
            Debug.Assert(!_sequences.ContainsKey(sequence.GetType()));
            _sequences.Add(sequence.GetType(), sequence);
        }

        public void AddAction<TSequence>(SequenceAction action) where TSequence : GameFlowSequence
        {
            if (_sequences.ContainsKey(typeof(TSequence)))
            {
                _sequences[typeof(TSequence)].AddAction(action);
            }
        }

        public void RemoveAction<TSequence>(SequenceAction action) where TSequence : GameFlowSequence
        {
            if (_sequences.ContainsKey(typeof(TSequence)))
            {
                _sequences[typeof(TSequence)].RemoveAction(action);
            }
        }

        public UniTask ExecuteSequence<TSequence>(CancellationToken token) where TSequence : GameFlowSequence
        {
            if (_sequences.ContainsKey(typeof(TSequence)))
            {
                return _sequences[typeof(TSequence)].Execute(token);
            }
            
            return UniTask.CompletedTask;
        }
    }
}