using System;
using System.Collections.Generic;
using Server;
using UnityEngine;

namespace MonoBehaviorClient
{
    public class Async : MonoBehaviour, IMonoBehaviorAsync<Action>
    {
        private readonly List<Action> _actions = new List<Action>();
        public void Add(Action action)
        {
            _actions.Add(action);
        }

        private void Update()
        {
            if (_actions.Count > 0)
            {
                for (var i = 0; i < _actions.Count; i++)
                {
                    try
                    {
                        _actions[i]();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
    }
}