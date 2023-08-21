using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class SkillItemView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _idText;

        private SkillItemModel _itemModel;
        private IDisposable _disposable;
        
        public void Init(SkillItemModel itemModel)
        {
            _itemModel = itemModel;
            _idText.text = itemModel.Id;
            (transform as RectTransform).anchoredPosition = itemModel.Position;
            _disposable = itemModel.IsLearned.Subscribe(UpdateIsLearnedState);
            _button.onClick.AddListener(() => itemModel.OnItemClicked?.Invoke(itemModel));
        }

        private void UpdateIsLearnedState(bool isLearned)
        {
            _button.image.color = isLearned ? Color.green : Color.gray;
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
            _disposable = null;
            _button.onClick.RemoveAllListeners();
        }
    }
}