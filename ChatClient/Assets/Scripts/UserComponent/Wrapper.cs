using System;
using UnityEngine.Serialization;

namespace UserComponent
{
    [Serializable]
    public class Wrapper<T>
    {
        public Wrapper(T[] items)
        {
            this.items = items;
        }

        public T[] items;
    }
}