using Delegates;
using Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UiController : MonoBehaviour, IUiController
    {
        public event IntValue EvnAchievementDone;
        
        [Header("Panels")]
        [SerializeField] private RectTransform achievementPanel;
        [SerializeField] private RectTransform currencyPanel;
        
        [Header("Player")]
        [SerializeField] private Image playerHealthBar;

        private IUiAchievementModule[] _achievementItem;
        private IUiCurrencyModule[]    _currencyItem;
        

    #region Interface Methods

        public void Init(float playerHp)
        {
            _achievementItem = achievementPanel.GetComponentsInChildren<IUiAchievementModule>();
            _currencyItem    = currencyPanel.GetComponentsInChildren<IUiCurrencyModule>();

            for (int i = 0; i < _achievementItem.Length; i++)
            {
                _achievementItem[i].Init(i);
                _achievementItem[i].EvnCollected += OnCollected;
            }

            for (int i = 0; i < 3; i++)
            {
                _currencyItem[i].SetName(((ECurrencyType)i).ToString());
            }
            
            SetPlayerHealth(playerHp);
        }
        
        public float GetHudHorizontalOffset()
        {
            return currencyPanel.anchorMax.x;
        }

        public void SetPlayerHealth(float value)
        {
            playerHealthBar.fillAmount = value;
        }

        public void SetCurrency(ECurrencyType currencyType, int value)
        {
            _currencyItem[(int)currencyType].SetAmount(value);
        }

        public void SetAchievementItem(int index, string achieveName, string description, EStatusType statusType,
                                       string rewardType, int rewardAmount, int curLevel, int maxPoint, int curPoint)
        {

            _achievementItem[index].SetName(achieveName, curLevel);
            _achievementItem[index].SetDescription(description, maxPoint);
            _achievementItem[index].SetReward(rewardType, rewardAmount);
            _achievementItem[index].SetMaxPoint(maxPoint);
            _achievementItem[index].SetStatusType(statusType);
            _achievementItem[index].SetCounter(curPoint);
        }

        public void SetAchievementPoint(EStatusType statusType, int curPoint)
        {
            foreach (IUiAchievementModule item in _achievementItem)
            {
                if (item.GetStatusType() == statusType)
                {
                    item.SetCounter(curPoint);
                }
            }
        }
        
    #endregion

    #region Event Handler

        private void OnCollected(int index)
        {
            EvnAchievementDone?.Invoke(index);
        }
        
    #endregion
        
    }
    
    public interface IUiController
    {
        event IntValue EvnAchievementDone;
        void Init(float playerHp);
        float GetHudHorizontalOffset();
        void SetPlayerHealth(float value);
        void SetCurrency(ECurrencyType currencyType, int value);
        void SetAchievementItem(int index, string achieveName, string description, EStatusType statusType,
                                string rewardType, int rewardAmount, int curLevel, int maxPoint, int curPoint);
        void SetAchievementPoint(EStatusType statusType, int curPoint);
    }
    
}
