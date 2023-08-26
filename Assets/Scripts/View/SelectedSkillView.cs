using UniRx;
using UnityEngine;

namespace View
{
    public class SelectedSkillView : MonoBehaviour
    {
        [SerializeField] private ActionButton _learnSkill;
        [SerializeField] private ActionButton _forgetSkill;
        [SerializeField] private ReactiveTextView _skillCost;

        private CompositeDisposable _disposable;

        public void Init(SelectedSkillModel model)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();

            _learnSkill.Init(model.OnLearnButtonClicked);
            _forgetSkill.Init(model.OnForgetButtonClicked);

            _skillCost.Init(model.Cost);
            model.CanLearn.Subscribe(UpdateLearnButton).AddTo(_disposable);
            model.CanForget.Subscribe(UpdateForgetButton).AddTo(_disposable);
        }

        private void UpdateLearnButton(bool canLearn) => _learnSkill.Color = canLearn ? Color.green : Color.gray;
        private void UpdateForgetButton(bool canForget) => _forgetSkill.Color = canForget ? Color.red : Color.gray;

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}