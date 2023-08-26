using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class SkillItemView : MonoBehaviour
    {
        [SerializeField] private ActionButton _button;
        [SerializeField] private Text _idText;

        private IDisposable _disposable;
        
        public void Init(SkillItemModel itemModel)
        {
            _idText.text = itemModel.Id;
            (transform as RectTransform).anchoredPosition = itemModel.Position;
            _disposable = itemModel.IsLearned.Subscribe(UpdateColor);
            _button.Init(() => itemModel.OnItemClicked?.Invoke(itemModel));
        }

        private void UpdateColor(bool isLearned) => _button.Color = isLearned ? Color.green : Color.gray;

        private void OnDestroy()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}