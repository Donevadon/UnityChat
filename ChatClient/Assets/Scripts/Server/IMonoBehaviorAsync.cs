using System;

namespace Server
{
    public interface IMonoBehaviorAsync<in T> where T: Delegate
    {
        void Add(T action);
    }
}