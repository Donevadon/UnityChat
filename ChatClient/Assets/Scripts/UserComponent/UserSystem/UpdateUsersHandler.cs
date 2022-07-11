using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UserComponent.UserSystem
{
    public class UpdateUsersHandler : MonoBehaviour
    {
        [SerializeField] private ColorText proto;
        [SerializeField] private Transform[] tables;
        private readonly List<Action> _actions = new List<Action>();
        private readonly List<ColorText> _users = new List<ColorText>();
        private IUserReceived _received;

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

        private void OnDestroy()
        {
            if (_received != null)
            {
                _received.UserReceived -= OnUserReceived;
                _received.BatchUserReceived -= OnBatchUserReceived;
                _received.UserLogout -= OnUserLogout;
            }
        }

        [Inject]
        private void Init(IUserReceived received)
        {
            _received = received;
            _received.UserReceived += OnUserReceived;
            _received.BatchUserReceived += OnBatchUserReceived;
            _received.UserLogout += OnUserLogout;
        }

        private void OnUserReceived(string json)
        {
            _actions.Add(() =>
            {
                var dto = JsonUtility.FromJson<UserDTO>(json);
                InstantiateUser(dto);
            });
        }

        private void OnBatchUserReceived(string json)
        {
            _actions.Add(() =>
            {
                var users = JsonUtility.FromJson<Wrapper<UserDTO>>(json);
                foreach (var dto in users.items) InstantiateUser(dto);
            });
        }

        private void OnUserLogout(string json)
        {
            _actions.Add(() =>
            {
                var userData = JsonUtility.FromJson<UserDTO>(json);
                var user = _users.First(text => text.Text == userData.nick);
                user.transform.parent = tables[(int) Status.Offline];
            });
        }

        private void InstantiateUser(UserDTO dto)
        {
            var user = Instantiate(proto, tables[(int) dto.status]);
            user.Text = dto.nick;
            user.Color = dto.color;
            _users.Add(user);
        }
    }
}