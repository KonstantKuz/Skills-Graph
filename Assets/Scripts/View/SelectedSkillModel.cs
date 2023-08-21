using System;
using Skills;

namespace View
{
    public class SelectedSkillModel
    {
        public readonly bool CanLearn;
        public readonly bool CanForget;
        public readonly Action OnLearnButtonClicked;
        public readonly Action OnForgetButtonClicked;

        public SelectedSkillModel(SkillService skillService, SkillItemModel item, Action onLearnButtonClicked, Action onForgetButtonClicked)
        {
            CanLearn = skillService.CanLearn(item.Id);
            CanForget = skillService.CanForget(item.Id);
            OnLearnButtonClicked = onLearnButtonClicked;
            OnForgetButtonClicked = onForgetButtonClicked;
        }
    }
}