using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Configurable
{
    private readonly Dictionary<string, int> _statusCount = new Dictionary<string, int>();

    private readonly Dictionary<string, int> _achievementLevel = new Dictionary<string, int>();
    
    private int _chip;
    private int _core;
    private int _bit;

    private const string StatusDataKey      = "StatusData";
    private const string AchievementDataKey = "AchievementData";
    
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
        LoadAchievementData();
        LoadCurrencies();
    }
    
    private void LoadStatusData()
    {
        string statusData = PlayerPrefs.GetString(StatusDataKey, string.Empty);
        
        if (!string.IsNullOrEmpty(statusData))
        {
            DicSaveData statusSaveData = JsonUtility.FromJson<DicSaveData>(statusData);

            _statusCount.Clear();
            
            foreach (DicSaveUnit unit in statusSaveData.dicSaveUnits)
            {
                _statusCount.Add(unit.key, unit.value);
            }
        }
    }
    
    private void LoadAchievementData()
    {
        string achievementData = PlayerPrefs.GetString(AchievementDataKey, string.Empty);
        
        if (!string.IsNullOrEmpty(achievementData))
        {
            DicSaveData achievementSaveData = JsonUtility.FromJson<DicSaveData>(achievementData);

            _achievementLevel.Clear();
            
            foreach (DicSaveUnit unit in achievementSaveData.dicSaveUnits)
            {
                _achievementLevel.Add(unit.key, unit.value);
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
        SaveAchievementData();
        
        PlayerPrefs.Save();
    }
    
    public void SaveStatusData()
    {
        string statusData = string.Empty;
        
        if (_statusCount.Count > 0)
        {
            List<DicSaveUnit> status = new List<DicSaveUnit>();

            foreach (KeyValuePair<string, int> state in _statusCount)
            {
                status.Add(new DicSaveUnit(state));
            }

            statusData = JsonUtility.ToJson(new DicSaveData(status.ToArray()));
        }

        PlayerPrefs.SetString(StatusDataKey, statusData);
    }
    
    public void SaveAchievementData()
    {
        string achievementData = string.Empty;
        
        if (_achievementLevel.Count > 0)
        {
            List<DicSaveUnit> achievement = new List<DicSaveUnit>();

            foreach (KeyValuePair<string, int> state in _achievementLevel)
            {
                achievement.Add(new DicSaveUnit(state));
            }

            achievementData = JsonUtility.ToJson(new DicSaveData(achievement.ToArray()));
        }

        PlayerPrefs.SetString(AchievementDataKey, achievementData);
    }

    public void SaveCurrencies()
    {
        PlayerPrefs.SetInt(ECurrencyType.Chip.ToString(), _chip);
        PlayerPrefs.SetInt(ECurrencyType.Core.ToString(), _core);
        PlayerPrefs.SetInt(ECurrencyType.Bit.ToString(), _bit);
    }
    
#endregion
    
#region Setter & Getter

    public int AddStatusCount(string state, int point)
    {
        if (_statusCount.ContainsKey(state))
        {
            _statusCount[state] += point;
        }
        else
        {
            _statusCount.Add(state, point);
        }

        return _statusCount[state];
    }

    public void TakeStatusPoints(string state, int amount)
    {
        if (_statusCount.ContainsKey(state))
        {
            _statusCount[state] -= amount;
        }
        else
        {
            _statusCount.Add(state, 0);
        }
    }
    
    public int GetStatusPoint(string state)
    {
        if (!_statusCount.ContainsKey(state))
        {
            _statusCount.Add(state, 0);
        }

        return _statusCount[state];
    }

    public void AddAchievementLevel(string achieveName)
    {
        if (_achievementLevel.ContainsKey(achieveName))
        {
            _achievementLevel[achieveName]++;
        }
        else
        {
            _achievementLevel.Add(achieveName, 1);
        }
    }

    public int GetAchievementLevel(string achieveName)
    {
        if (!_achievementLevel.ContainsKey(achieveName))
        {
            _achievementLevel.Add(achieveName, 0);
        }

        return _achievementLevel[achieveName];
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
    public class DicSaveData
    {
        public DicSaveUnit[] dicSaveUnits;

        public DicSaveData(DicSaveUnit[] dicSaveUnits)
        {
            this.dicSaveUnits = dicSaveUnits;
        }
    }
  
    [System.Serializable]
    public class DicSaveUnit
    {
        public string key;
        public int value;

        public DicSaveUnit(string key, int value)
        {
            this.key = key;
            this.value = value;
        }
        
        public DicSaveUnit(KeyValuePair<string, int> keyValuePair)
        {
            key = keyValuePair.Key;
            value = keyValuePair.Value;
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
