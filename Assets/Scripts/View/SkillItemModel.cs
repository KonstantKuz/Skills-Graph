using System;
using Skills;
using UniRx;
using UnityEngine;

namespace View
{
    public class SkillItemModel
    {
        private readonly Skill _skill;

        public string Id => _skill.Id;
        public Vector2 Position { get; private set; }

        public IObservable<bool> IsLearned => _skill.IsLearned;
        public bool IsSelected { get; private set; }
        public Action<SkillItemModel> OnItemClicked { get; }

        public SkillItemModel(Skill skill, Action<SkillItemModel> onItemClicked)
        {
            _skill = skill;
            OnItemClicked = onItemClicked;
        }
        
        public void SetPosition(Vector2 position) => Position = position;
        
        public void SwitchSelectedState()
        {
            IsSelected = !IsSelected;
        }
    }
}