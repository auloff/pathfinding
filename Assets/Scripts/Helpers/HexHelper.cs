using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

namespace Pathfinding.Helpers
{
    public class HexHelper : MonoBehaviour
    {
        public string hexRegularExpression = "^#([A-Fa-f0-9]{6})$";

        public Button applyButton;
        public InputField hexField;

        public Color apply, deny;
        public event Action<Color> hexReady;
        private Color _regular;
        private bool _isMatch = false;

        private void Start()
        {
            _regular = hexField.image.color;

            hexField?.onValueChanged.AddListener((hex) => {
                
                _isMatch = Regex.IsMatch(hex, hexRegularExpression);
                if (_isMatch)
                {
                    hexField.image.color = apply;
                }
                else
                {
                    if (hexField.text == string.Empty)
                        hexField.image.color = _regular;
                    else
                        hexField.image.color = deny;
                }
            });

            applyButton?.onClick.AddListener(() => {
                if (_isMatch) hexReady?.Invoke(HexToColor(hexField.text));
            });
        }

        public Color HexToColor(string hex)
        {
            float red = Convert.ToInt32(hex.Substring(1,2), 16) / 255f;
            float green = Convert.ToInt32(hex.Substring(3,2), 16) / 255f;
            float blue = Convert.ToInt32(hex.Substring(5,2), 16) / 255f;

            return new Color(red, green, blue);
        }
    }
}