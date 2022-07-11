using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UserComponent.MessageSystem
{
    public class MessageHandler : MonoBehaviour
    {
        [SerializeField] private Message proto;
        private readonly List<Action> _actions = new List<Action>();
        private IMessageReceived _received;

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

                _actions.Clear();
            }
        }

        [Inject]
        private void Init(IMessageReceived received)
        {
            _received = received;
            _received.MessageReceived += OnMessageReceived;
            _received.BatchMessageReceived += OnBatchMessageReceived;
        }

        private void OnMessageReceived(string json)
        {
            _actions.Add(() =>
            {
                var dto = JsonUtility.FromJson<MessageDTO>(json);
                ReceiveHandler(dto);
            });
        }

        private void OnBatchMessageReceived(string json)
        {
            _actions.Add(() =>
            {
                var messages = JsonUtility.FromJson<Wrapper<MessageDTO>>(json);
                foreach (var dto in messages.items) ReceiveHandler(dto);
            });
        }

        private void ReceiveHandler(MessageDTO dto)
        {
            var message = Instantiate(proto, transform);
            message.Color = dto.color;
            message.Nick = dto.nick;
            message.Text = dto.text;
        }
    }
}