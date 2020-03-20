using System;
using System.Collections.Generic;

namespace Svnvav.UberSpace
{
    public class ActionContainer<T>
    {
        private HashSet<Action<T>> _actions = new HashSet<Action<T>>();

        public void RegisterCallback(Action<T> action)
        {
            _actions.Add(action);
        }
        
        public void UnregisterCallback(Action<T> action)
        {
            _actions.Remove(action);
        }

        public void Execute(T param)
        {
            foreach (var action in _actions)
            {
                action.Invoke(param);
            }
        }

        public void Clear()
        {
            _actions.Clear();
        }
    }
}