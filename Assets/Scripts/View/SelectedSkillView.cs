using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class SelectedSkillView : MonoBehaviour
    {
        [SerializeField] private ActionButton _learn;
        [SerializeField] private ActionButton _forget;
        [SerializeField] private Text _skillCostText;

        public void Init(SelectedSkillModel model)
        {
            _learn.Color = model.CanLearn ? Color.green : Color.gray;
            _learn.Init(model.OnLearnButtonClicked);
            
            _forget.Color = model.CanForget ? Color.red : Color.gray;
            _forget.Init(model.OnForgetButtonClicked);
           
            _skillCostText.text = model.Cost;
        }
    }
}