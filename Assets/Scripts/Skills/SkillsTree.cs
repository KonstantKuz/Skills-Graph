using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;

namespace Skills
{
    public class SkillsTree
    {
        public readonly Skill Root;
        private readonly Dictionary<string, Skill> _skills;
        
        public Skill this[string id] => _skills[id];
        public IEnumerable<Skill> Skills => _skills.Values;
        
        public SkillsTree(Skill root)
        {
            Root = root;
            Root.IsLearned = new BoolReactiveProperty(true);
            _skills = BuildTree(Root).ToDictionary(it => it.Id, it => it);
        }

        private IEnumerable<Skill> BuildTree(Skill skill, int depth = -1)
        {
            var set = new HashSet<Skill>();
            depth++;
            skill.Depth = depth;
            set.Add(skill);
            foreach (var next in skill.ChildSkills)
            {
                next.ParentSkills.Add(skill);
                set.AddRange(BuildTree(next, depth));
            }
            return set;
        }

        public bool CanLearn(string id)
        {
            if (_skills[id].IsLearned.Value) return false;
            
            var skill = _skills[id];
            var queue = new Queue<Skill>();
            queue.Enqueue(skill);
            while (queue.Count > 0)
            {
                var predecessor = queue.Dequeue();
                if (predecessor.ParentSkills.Count == 0) return true;
                var previousLearnedSkill = predecessor.ParentSkills.FirstOrDefault(it => it.IsLearned.Value);
                if (previousLearnedSkill == null) return false;
                queue.Enqueue(previousLearnedSkill);
            }

            return true;
        }

        public bool CanForget(string id)
        {
            var childIsOneAndLearned = _skills[id].ChildSkills.Length == 1 && _skills[id].ChildSkills.First().IsLearned.Value;
            var anyChildLearned = _skills[id].ChildSkills.Any(it => it.IsLearned.Value);
            return _skills[id].IsLearned.Value 
                   && _skills[id] != Root 
                   && !(childIsOneAndLearned || anyChildLearned);
        }
    }
}