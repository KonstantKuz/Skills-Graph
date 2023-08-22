using Skills;
using UniRx;
using UnityEngine;

namespace View
{
    public class SkillsTreePresenter : MonoBehaviour
    {
        [SerializeField] private SkillService _skillService;
        [SerializeField] private SkillTreeView _skillTreeView;
        [SerializeField] private SelectedSkillView _selectedSkillView;
        [SerializeField] private ReactiveTextView _pointsText;
        [SerializeField] private ActionButton _addPointsButton;
        [SerializeField] private ActionButton _forgetAllButton;
        
        private SkillsTreeModel _model;
        public void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            _model = new SkillsTreeModel(_skillService.SkillsTree,
                _skillTreeView.Root.anchoredPosition,
                _skillTreeView.DistanceBetweenItems, 
                OnItemSelected);
            _skillTreeView.Init(_model);
         
            _pointsText.Init(_skillService.Points.Select(it => it.ToString()));
            _addPointsButton.Init(() => _skillService.AddSkillPoints(10));
            _forgetAllButton.Init(_skillService.ForgetAll);
        }

        private void OnItemSelected(SkillItemModel item)
        {
            _model.SetSelectedItem(item);
            InitSelectedItemView();
        }

        private void InitSelectedItemView()
        {
            var model = new SelectedSkillModel(_skillService, _model.SelectedModel, 
                () => OnLearnButtonClicked(_model.SelectedModel), 
                () => OnForgetButtonClicked(_model.SelectedModel));
            _selectedSkillView.Init(model);
        }

        private void OnLearnButtonClicked(SkillItemModel item)
        {
            _skillService.TryLearn(item.Id);
            InitSelectedItemView();
        }

        private void OnForgetButtonClicked(SkillItemModel item)
        {
            _skillService.TryForget(item.Id);
            InitSelectedItemView();
        }
    }
}