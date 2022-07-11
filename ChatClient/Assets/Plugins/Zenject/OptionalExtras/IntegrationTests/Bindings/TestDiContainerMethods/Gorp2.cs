﻿using ModestTree;
using UnityEngine;

#pragma warning disable 649

namespace Zenject.Tests.Bindings.DiContainerMethods
{
    //[CreateAssetMenu(fileName = "Gorp2", menuName = "Test/Gorp2")]
    public class Gorp2 : ScriptableObject
    {
        [Inject] private string _arg;

        public string Arg => _arg;

        [Inject]
        public void Initialize()
        {
            Log.Trace("Received arg '{0}' in Gorp", _arg);
        }
    }
}