using UnityEngine;

namespace Controllers
{
    public class UiController : MonoBehaviour, IUiController
    {

        [Header("Panels")]
        [SerializeField] private RectTransform achievementPanel;
        [SerializeField] private RectTransform powerUpPanel;
        
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

    #region Interface Methods

        public float GetHudHorizontalOffset()
        {
            return powerUpPanel.anchorMax.x;
        }

    #endregion

        
    }
    
    public interface IUiController
    {
        float GetHudHorizontalOffset();
    }
    
}
