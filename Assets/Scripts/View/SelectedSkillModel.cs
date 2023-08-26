using System;
using Skills;
using UniRx;

namespace View
{
    public class SelectedSkillModel
    {
        public readonly IReadOnlyReactiveProperty<string> Cost;
        public readonly IReadOnlyReactiveProperty<bool> CanLearn;
        public readonly IReadOnlyReactiveProperty<bool> CanForget;
        public readonly Action OnLearnButtonClicked;
        public readonly Action OnForgetButtonClicked;

        public SelectedSkillModel(SkillService skillService, IReadOnlyReactiveProperty<SkillItemModel> item, Action onLearnButtonClicked, Action onForgetButtonClicked)
        {
            Cost = item.Where(it => it != null).Select(it => skillService.SkillsTree[it.Id].Cost.ToString()).ToReactiveProperty();
            CanLearn = item.Where(it => it != null).Select(it => skillService.CanLearn(it.Id)).ToReactiveProperty();
            CanForget = item.Where(it => it != null).Select(it => skillService.CanForget(it.Id)).ToReactiveProperty();
            OnLearnButtonClicked = onLearnButtonClicked;
            OnForgetButtonClicked = onForgetButtonClicked;
        }
    }
}