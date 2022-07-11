using System;
using TMPro;
using UnityEngine;

namespace UserComponent
{
    public class ColorText : MonoBehaviour
    {
        private TMP_Text _text;

        public virtual string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        public string Color
        {
            get => "#" + ColorUtility.ToHtmlStringRGB(_text.color);
            set => _text.color = ColorUtility.TryParseHtmlString(value, out var color)
                ? color
                : throw new ArgumentException("Incorrect color");
        }

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }
    }
}