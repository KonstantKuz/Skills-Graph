using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Skill")]
    public class Skill : ScriptableObject
    {
        public string Id => name;
        public int Cost = 10;
        public BoolReactiveProperty IsLearned = new BoolReactiveProperty();
        public Skill[] ChildSkills;

        [HideInInspector] public int Depth;
        [HideInInspector] public HashSet<Skill> ParentSkills = new HashSet<Skill>();

        [ContextMenu("Reset temp")]
        public void ResetTemp()
        {
            IsLearned = new BoolReactiveProperty(false);
            ParentSkills = new HashSet<Skill>();
        }
    }
}
