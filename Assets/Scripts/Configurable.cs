using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Configurable
{

    private readonly Dictionary<string, int> _statusCount = new Dictionary<string, int>();

    private int _chip;
    private int _core;
    private int _bit;
    
    private const string StatusDataKey = "StatusData";
    
#region Initialize

    public static Configurable Instance => _instance ?? (_instance = new Configurable());

    private static Configurable _instance;

    private Configurable()
    {
        LoadAllData();

        Application.quitting += SaveAllData;
    }

#endregion

#region Save & Load

    //Load Data//////////////////////////////////////////////////////
    private void LoadAllData()
    {
        LoadStatusData();
        LoadCurrencies();
    }
    
    private void LoadStatusData()
    {
        string statusData = PlayerPrefs.GetString(StatusDataKey, string.Empty);
        
        if (!string.IsNullOrEmpty(statusData))
        {
            StatusSaveData statusSaveData = JsonUtility.FromJson<StatusSaveData>(statusData);

            _statusCount.Clear();
            
            foreach (StatusSaveUnit unit in statusSaveData.statusSaveUnits)
            {
                _statusCount.Add(unit.state, unit.point);
            }
        }
    }

    private void LoadCurrencies()
    {
        _chip = PlayerPrefs.GetInt(ECurrencyType.Chip.ToString(), 0);
        _core = PlayerPrefs.GetInt(ECurrencyType.Core.ToString(), 0);
        _bit = PlayerPrefs.GetInt(ECurrencyType.Bit.ToString(), 0);
    }
    
    //Save Data//////////////////////////////////////////////////////
    public void SaveAllData()
    {
        SaveStatusData();
        SaveCurrencies();
        
        PlayerPrefs.Save();
    }
    
    public void SaveStatusData()
    {
        string statusData = string.Empty;
        
        if (_statusCount.Count > 0)
        {
            List<StatusSaveUnit> status = new List<StatusSaveUnit>();

            foreach (KeyValuePair<string, int> state in _statusCount)
            {
                status.Add(new StatusSaveUnit(state));
            }

            statusData = JsonUtility.ToJson(new StatusSaveData(status.ToArray()));
        }

        PlayerPrefs.SetString(StatusDataKey, statusData);
    }

    public void SaveCurrencies()
    {
        PlayerPrefs.SetInt(ECurrencyType.Chip.ToString(), _chip);
        PlayerPrefs.SetInt(ECurrencyType.Core.ToString(), _core);
        PlayerPrefs.SetInt(ECurrencyType.Bit.ToString(), _bit);
    }
    
#endregion
    
#region Setter & Getter

    public void AddStatusCount(string state, int point)
    {
        if (_statusCount.ContainsKey(state))
        {
            _statusCount[state] += point;
        }
        else
        {
            _statusCount.Add(state, point);
        }
        
        //Debug.Log($"{state}: {_statusCount[state]}");
    }

    public int GetCurrency(ECurrencyType currencyType)
    {
        switch (currencyType)
        {
            case ECurrencyType.Chip:
                return _chip;
            
            case ECurrencyType.Core:
                return _core;
            
            case ECurrencyType.Bit:
                return _bit;
            
            default:
                return -1;
        }
    }
    
    public int GiveCurrency(ECurrencyType currencyType, int amount)
    {
        switch (currencyType)
        {
            case ECurrencyType.Chip:
                _chip += amount;
                return _chip;
            
            case ECurrencyType.Core:
                _core += amount;
                return _core;
            
            case ECurrencyType.Bit:
                _bit += amount;
                return _bit;
            
            default:
                return -1;
        }
    }
    
    public int TakeCurrency(ECurrencyType currencyType, int amount)
    {
        switch (currencyType)
        {
            case ECurrencyType.Chip:
                _chip -= amount;
                return _chip;
            
            case ECurrencyType.Core:
                _core -= amount;
                return _core;
            
            case ECurrencyType.Bit:
                _bit -= amount;
                return _bit;
            
            default:
                return -1;
        }
    }
    
#endregion
    
#region Classes

    [System.Serializable]
    public class StatusSaveData
    {
        public StatusSaveUnit[] statusSaveUnits;

        public StatusSaveData(StatusSaveUnit[] statusSaveUnits)
        {
            this.statusSaveUnits = statusSaveUnits;
        }
    }
  
    [System.Serializable]
    public class StatusSaveUnit
    {
        public string state;
        public int point;

        public StatusSaveUnit(string state, int point)
        {
            this.state = state;
            this.point = point;
        }
        
        public StatusSaveUnit(KeyValuePair<string, int> keyValuePair)
        {
            state = keyValuePair.Key;
            point = keyValuePair.Value;
        }
    }

#endregion
    
    
#if UNITY_EDITOR
    
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs Clean");
    }
    
#endif
}
