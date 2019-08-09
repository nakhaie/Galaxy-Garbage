using UnityEngine;
using UnityEngine.UI;

namespace Modules
{
    public class UiCurrencyModule : MonoBehaviour, IUiCurrencyModule
    {
        [SerializeField] private Text nameField;
        [SerializeField] private Text amountField;

    #region Interface Methods
        
        public void SetName(string currencyName)
        {
            nameField.text = currencyName;
        }

        public void SetAmount(int value)
        {
            amountField.text = value.ToString();
        }
        
    #endregion
    }
    
    public interface IUiCurrencyModule
    {
        void SetName(string currencyName);
        void SetAmount(int value);
    }
}
