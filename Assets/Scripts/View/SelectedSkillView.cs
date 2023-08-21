using JetBrains.Annotations;
using UnityEngine;

namespace View
{
    public class SelectedSkillView : MonoBehaviour
    {
        [SerializeField] private ActionButton _learn;
        [SerializeField] private ActionButton _forget;

        public void Init([CanBeNull]SelectedSkillModel model)
        {
            gameObject.SetActive(model != null);
            
            if(model == null) return;

            _learn.Button.image.color = model.CanLearn ? Color.green : Color.gray;
            _learn.Init(model.OnLearnButtonClicked);
            _forget.Button.image.color = model.CanForget ? Color.red : Color.gray;
            _forget.Init(model.OnForgetButtonClicked);
        }
    }
}