using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ReactiveTextView : MonoBehaviour
    {
        private Text _text;
        private Text Text => _text ??= gameObject.GetComponent<Text>();
        
        private IDisposable _disposable;
        
        public void Init(IObservable<string> text)
        {
            _disposable = text.Subscribe(it => Text.text = it);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}