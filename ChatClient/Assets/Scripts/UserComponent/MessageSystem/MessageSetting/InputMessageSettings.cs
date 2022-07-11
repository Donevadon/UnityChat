using System;
using System.Collections.Generic;
using Server;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserComponent.MessageSystem.MessageSetting
{
    public class InputMessageSettings : MonoBehaviour, IConnectionWindow
    {
        private Message _message;
        private Button _applyButton;
        private TMP_InputField _nickInput;
        private InputColor _colorInput;
        private string _color;
        private string _nick;
        private GameObject _parent;

        private void Awake()
        {
            _nickInput = GetComponentInChildren<TMP_InputField>();
            _colorInput = GetComponentInChildren<InputColor>();
            _message = GetComponentInChildren<Message>();
            _applyButton = GetComponentInChildren<Button>();
            _parent = transform.parent.gameObject;
            Close();
        }

        public event Action<string, string> OnLogin;

        public void Open()
        {
            _parent.SetActive(true);
        }
    
        private void Start()
        {
            _nickInput.onValueChanged.AddListener(SetNick);
            _colorInput.ColorChanged += SetColor;
            _applyButton.onClick.AddListener(Apply);
            _color = _message.Color;
            _nick = _message.Nick;
        }

        public void Close()
        {
            _parent.SetActive(false);
        }

        private void Apply()
        {
            OnLogin?.Invoke(_nick, _color);
        }

        private void SetColor(string color)
        {
            _message.Color = color;
            _color = color;
        }

        private void SetNick(string nick)
        {
            _nick = nick;
            _message.Nick = nick;
        }

        private void OnDestroy()
        {
            _colorInput.ColorChanged -= SetColor;
            _nickInput.onValueChanged.RemoveListener(SetNick);
            _applyButton.onClick.RemoveListener(Apply);
        }
    }
}