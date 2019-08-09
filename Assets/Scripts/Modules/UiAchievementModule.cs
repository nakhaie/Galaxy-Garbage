using Delegates;
using UnityEngine;
using UnityEngine.UI;

namespace Modules
{
    public class UiAchievementModule : MonoBehaviour, IUiAchievementModule
    {
        public event IntValue EvnCollected;
        
        [SerializeField] private Text nameField;
        [SerializeField] private Text descriptionField;
        [SerializeField] private Text counterField;
        [SerializeField] private Text rewardField;
        
        [SerializeField] private Slider pointBar;
        [SerializeField] private Button collectBtn;

        private const string NamingStyle  = "{0} {1}";
        private const string CounterStyle = "{0} / {1}";
        private const string RewardStyle  = "{0} X{1}";

        private int _index;

    #region Interface Methods
        
        public void Init(int index)
        {
            _index = index;
            collectBtn.onClick.AddListener(OnCollectBtnClick);
        }
        
        public void SetName(string achieveName, int curLevel)
        {
            nameField.text = string.Format(NamingStyle, achieveName, curLevel);
        }
        
        public void SetDescription(string description, int maxPoint)
        {
            descriptionField.text = string.Format(description, maxPoint);
        }
        
        public void SetCounter(int curPoint, int maxPoint)
        {
            counterField.text = string.Format(CounterStyle, curPoint, maxPoint);
        }
        
        public void SetReward(string typeName, int amount)
        {
            rewardField.text = string.Format(RewardStyle, typeName, amount);
        }

        public void SetPointBar(int maxPoint)
        {
            pointBar.maxValue = maxPoint;
            pointBar.value = 0;
            collectBtn.interactable = false;
        }

        public void SetPointBarValue(int curPoint)
        {
            if (pointBar.maxValue <= curPoint)
            {
                curPoint = (int)pointBar.maxValue;
                
                collectBtn.interactable = true;
            }

            pointBar.value = curPoint;
        }
        
    #endregion
        
    #region Event Handler

        private void OnCollectBtnClick()
        {
            EvnCollected?.Invoke(_index);
        }
        
    #endregion
    }

    public interface IUiAchievementModule
    {
        event IntValue EvnCollected;

        void Init(int index);
        void SetName(string achieveName, int curLevel);
        void SetDescription(string description, int maxPoint);
        void SetCounter(int curPoint, int maxPoint);
        void SetReward(string typeName, int amount);
        void SetPointBar(int maxPoint);
        void SetPointBarValue(int curPoint);
    }
}
