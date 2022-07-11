using UnityEngine;
using UnityEngine.Serialization;

namespace UserComponent.MessageSystem
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private NickInMessage nick;
        [SerializeField] private ColorText text;

        public string Nick
        {
            get => nick.Text;
            set => nick.Text = value;
        }
    
        public string Text
        {
            get => text.Text;
            set => text.Text = value;
        }
    
        public string Color
        {
            get => nick.Color;
            set
            {
                text.Color = value;
                nick.Color = value;
            }
        }
    }
}