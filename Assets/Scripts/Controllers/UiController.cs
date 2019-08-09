using Modules;
using UnityEngine;

namespace Controllers
{
    public class UiController : MonoBehaviour, IUiController
    {

        [Header("Panels")]
        [SerializeField] private RectTransform achievementPanel;
        [SerializeField] private RectTransform powerUpPanel;

        private IUiAchievementModule[] _achievementItem;

    #region Unity Methods
        
        private void Awake()
        {
            _achievementItem = achievementPanel.GetComponentsInChildren<IUiAchievementModule>();

            for (int i = 0; i < _achievementItem.Length; i++)
            {
                _achievementItem[i].Init(i);
                _achievementItem[i].EvnCollected += OnCollected;
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
    #endregion

    #region Interface Methods

        public float GetHudHorizontalOffset()
        {
            return powerUpPanel.anchorMax.x;
        }

    #endregion

    #region Event Handler

        private void OnCollected(int index)
        {
            
        }
        
    #endregion
        
    }
    
    public interface IUiController
    {
        float GetHudHorizontalOffset();
    }
    
}
