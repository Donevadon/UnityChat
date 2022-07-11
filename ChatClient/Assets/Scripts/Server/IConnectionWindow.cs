using System;

namespace Server
{
    public interface IConnectionWindow
    {
        event Action<string, string> OnLogin;
        void Open();
        void Close();
    }
}