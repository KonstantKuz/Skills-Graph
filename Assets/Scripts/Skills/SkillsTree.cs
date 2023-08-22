using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Skills
{
    public class SkillsTree
    {
        public readonly Skill Root;
        private readonly Dictionary<string, Skill> _skills;
        private Dictionary<Skill, HashSet<Skill>> _connections;
        
        public Skill this[string id] => _skills[id];
        public IEnumerable<Skill> Skills => _skills.Values;
        
        public SkillsTree(Skill root)
        {
            Root = root;
            Root.IsLearned = new BoolReactiveProperty(true);
            _skills = BuildSkills(Root).ToDictionary(it => it.Id, it => it);
            _connections = BuildConnections();
        }

        private IEnumerable<Skill> BuildSkills(Skill skill, int depth = -1)
        {
            var set = new HashSet<Skill>();
            depth++;
            skill.Depth = depth;
            skill.Cost = Random.Range(10, 20);
            set.Add(skill);
            foreach (var childSkill in skill.ChildSkills)
            {
                childSkill.ParentSkills.Add(skill);
                set.AddRange(BuildSkills(childSkill, depth));
            }

            return set;
        }

        private Dictionary<Skill, HashSet<Skill>> BuildConnections()
        {
            var dictionary = new Dictionary<Skill, HashSet<Skill>>();
            foreach (var skill in _skills.Values)
            {
                if (!dictionary.ContainsKey(skill))
                {
                    dictionary.Add(skill, new HashSet<Skill>());
                }
                dictionary[skill].AddRange(skill.ChildSkills);
                dictionary[skill].AddRange(skill.ParentSkills);
            }
            return dictionary;
        }

        public bool CanLearn(string id)
        {
            var skill = _skills[id];
            if (skill.IsLearned.Value || skill == Root) return false;
            
            var queue = new Queue<Skill>();
            queue.Enqueue(skill);
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item.ParentSkills.Count == 0) return true;
                var anyParentLearnedSkill = item.ParentSkills.FirstOrDefault(it => it.IsLearned.Value);
                if (anyParentLearnedSkill == null) return false;
                queue.Enqueue(anyParentLearnedSkill);
            }

            return true;
        }
        
        public bool CanForget(string id)
        {
            var skill = _skills[id];
            if (skill == Root || !skill.IsLearned.Value) return false;
            
            var deepestLearnedSkill = GetDeepestLearnedSkill(id);
            if (skill == deepestLearnedSkill) return true;
            
            var possibleBranchesWithDeepestSkill = BuildPossibleBranches(deepestLearnedSkill).ToList();
            return possibleBranchesWithDeepestSkill.Where(branch => !branch.Contains(skill))
                .Any(branch => branch.All(it => it.IsLearned.Value));
        }

        private Skill GetDeepestLearnedSkill(string startId)
        {
            var current = _skills[startId];
            var moveNext = true;
            while (moveNext)
            {
                var next = current.ChildSkills.FirstOrDefault(it => it.IsLearned.Value);
                moveNext = next != null;
                if (moveNext) current = next;
            }
            return current;
        }

        private IEnumerable<IEnumerable<Skill>> BuildPossibleBranches(Skill current, IEnumerable<Skill> path = null)
        {
            path ??= Enumerable.Empty<Skill>();
            
            if(path.Contains(current))
            {
                yield break;
            }
    
            path = path.Append(current);
    
            if(current == Root)
            {
                yield return path;
                yield break;
            }
    
            foreach (var subPath in _connections[current].SelectMany(neighbor => BuildPossibleBranches(neighbor, path)))
            {
                yield return subPath;
            }   
        }
    }
}