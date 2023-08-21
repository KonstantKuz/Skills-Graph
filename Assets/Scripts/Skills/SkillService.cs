using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Skills
{
    public class SkillService : MonoBehaviour
    {
        [SerializeField] private Skill _rootConfig;

        private SkillsTree _skillsTree;

        public SkillsTree SkillsTree => _skillsTree ??= new SkillsTree(_rootConfig);
        public IntReactiveProperty Points { get; } = new IntReactiveProperty(0);
        public void AddSkillPoints(int value) => Points.Value += value;
        public void RemoveSkillPoints(int value) => Points.Value -= value;
        public bool HasEnoughPoints(int value) => Points.Value >= value;

        public bool TryLearn(string id)
        {
            var skill = _skillsTree[id];
            if (CanLearn(id) && TryRemovePoints(skill.Cost))
            {
                skill.IsLearned.Value = true;
                return true;
            }
            return false;
        }

        public bool TryForget(string id)
        {
            if (!_skillsTree.CanForget(id)) return false;
            Forget(id);
            return true;
        }
        
        public bool CanLearn(string id)
        {
            return _skillsTree.CanLearn(id)
                   && HasEnoughPoints(_skillsTree[id].Cost);
        }
        
        public bool CanForget(string id) => _skillsTree.CanForget(id);

        public bool TryRemovePoints(int value)
        {
            var hasPoints = HasEnoughPoints(value);
            if(hasPoints) RemoveSkillPoints(value);
            return hasPoints;
        }

        public void Forget(string id)
        {
            _skillsTree[id].IsLearned.Value = false;
            AddSkillPoints(_skillsTree[id].Cost);
        }

        public void ForgetAll()
        {
            foreach (var skill in _skillsTree.Skills.Where(it => it.IsLearned.Value))
            {
                if(skill == _skillsTree.Root) continue;
                skill.IsLearned.Value = false;
                AddSkillPoints(skill.Cost);
            }
        }

        private void OnDisable()
        {
            foreach (var skill in _skillsTree.Skills)
            {
                skill.ParentSkills = new HashSet<Skill>();
            }
        }
    }
}