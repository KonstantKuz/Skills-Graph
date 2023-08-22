using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Skills;
using UnityEngine;

namespace View
{
    public class SkillsTreeModel
    {
        
        private readonly SkillsTree _skillsTree;
        private readonly Vector2 _center;
        private readonly float _distance;

        public readonly Dictionary<string, SkillItemModel> Models;
        [CanBeNull]
        public SkillItemModel SelectedModel { get; private set; }
        
        public SkillsTreeModel(SkillsTree skillsTree, Vector2 center, float distance, Action<SkillItemModel> selectedCallback)
        {
            _skillsTree = skillsTree;
            _center = center;
            _distance = distance;
            Models = skillsTree.Skills.ToDictionary(it => it.Id, it => new SkillItemModel(it, selectedCallback));
            SetPositionsRecursive();
        }

        public void SetSelectedItem(SkillItemModel itemModel)
        {
            SelectedModel?.SwitchSelectedState();
            SelectedModel = itemModel;
        }

        private void SetPositionsRecursive()
        {
            var branchRoots = _skillsTree.Root.ChildSkills;
            var angleStep = 360f / branchRoots.Length;
            for (var i = 0; i < branchRoots.Length; i++)
            {
                SetPositionsInBranchRecursive(branchRoots[i], angleStep * i, Vector2.zero);
            }
        }
        
        private void SetPositionsInBranchRecursive(Skill skill, float relativeAngle, Vector2 sideOffset)
        {
            var position = Vector2.zero;
            if (skill.ParentSkills.Count > 1)
            {
                position = GetRelativeToParents(skill);
            }
            else
            {
                position = Models[skill.ParentSkills.First().Id].Position;
                var offset = Quaternion.Euler(0, 0, relativeAngle) * (Vector2.up * _distance + sideOffset);
                position += new Vector2(offset.x, offset.y);
            }

            Models[skill.Id].SetPosition(position);

            var nextSkillsCount = skill.ChildSkills.Length;
            for (var i = 0; i < nextSkillsCount; i++)
            {
                var offset = GetSideOffset(nextSkillsCount, i);
                SetPositionsInBranchRecursive(skill.ChildSkills[i], relativeAngle, offset);
            }
        }
        
        private Vector2 GetRelativeToParents(Skill skill)
        {
            var relativePosition = skill.ParentSkills.Select(it => Models[it.Id].Position)
                                       .Aggregate(Vector2.zero, (positionSum, position) => positionSum + position) 
                                   / skill.ParentSkills.Count;
            var parentsLine = Models[skill.ParentSkills.First().Id].Position - Models[skill.ParentSkills.Last().Id].Position;
            parentsLine = new Vector2(parentsLine.y, -parentsLine.x);
            var directionToCenter = (relativePosition - _center);
            parentsLine *= Vector2.Dot(parentsLine, directionToCenter);
            return relativePosition + parentsLine.normalized * _distance;
        }

        private Vector2 GetSideOffset(int itemsCount, int index)
        {
            if(itemsCount == 1) return Vector2.zero;
            var width = Vector2.right * _distance * itemsCount;
            var percent = (float) index / Mathf.Max(1,itemsCount - 1);
            return (-width / 2) + width * percent;
        }
    }
}