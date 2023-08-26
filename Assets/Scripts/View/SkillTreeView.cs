using System;
using UnityEngine;

namespace View
{
    public class SkillTreeView : MonoBehaviour
    {
        [SerializeField] private SkillItemView _skillItemViewPrefab;
        [field:SerializeField] public RectTransform Root { get; private set; }
        [field:SerializeField] public float DistanceBetweenItems { get; private set; }
        
        public void Init(SkillsTreeModel model)
        {
            Clear();

            foreach (var itemModel in model.Items.Values)
            {
                var skillView = Instantiate(_skillItemViewPrefab, Root);
                skillView.Init(itemModel);
            }
        }

        private void Clear() => Array.ForEach(Root.GetComponentsInChildren<SkillItemView>(), it => Destroy(it.gameObject));
    }
}