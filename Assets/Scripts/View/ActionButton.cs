using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ActionButton : MonoBehaviour
    {
        private Button _button;
        public Button Button => _button ??= gameObject.GetComponent<Button>();

        public Color Color
        {
            get => Button.image.color;
            set => Button.image.color = value;
        }
        
        public void Init(Action action)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => action?.Invoke());
        }

        private void OnDisable() => Button.onClick.RemoveAllListeners();
    }
}