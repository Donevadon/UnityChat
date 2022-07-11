using System;
using UnityEngine;
using UnityEngine.UI;

namespace UserComponent.MessageSystem.MessageSetting
{
    internal class InputColor: MonoBehaviour
    {
        [SerializeField] private Slider r;
        [SerializeField] private Slider g;
        [SerializeField] private Slider b;

        public event Action<string> ColorChanged;

        public string Color
        {
            get
            {
                var color = new Color(r.value, g.value, b.value);
                return "#" + ColorUtility.ToHtmlStringRGB(color);
            }
        }

        private void Awake()
        {
            r.onValueChanged.AddListener(OnColorChange);
            g.onValueChanged.AddListener(OnColorChange);
            b.onValueChanged.AddListener(OnColorChange);
        }

        private void OnColorChange(float _)
        {
            ColorChanged?.Invoke(Color);
        }
    }
}