using System;

namespace Server
{
    [Serializable]
    public class NewUser
    {
        public string nick;
        public string color;

        public NewUser(string nick, string color)
        {
            this.nick = nick;
            this.color = color;
        }
    }
}