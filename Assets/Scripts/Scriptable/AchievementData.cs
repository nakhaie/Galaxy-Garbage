using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "AchievementData", menuName = "ScriptableObjects/AchievementData", order = 1)]
    public class AchievementData : ScriptableObject
    {
        public const string ResourcePath = "AchievementData";

        public AchievementProperty[] achievementProperties;
    }

    [System.Serializable]
    public class AchievementProperty
    {
        public string achieveName;
        [Multiline(3)]
        public string description;

        public EStatusType statusType;
        
        public PointAndReward[] points;
    }

    [System.Serializable]
    public struct PointAndReward
    {
        [Header("Point")]
        public int           point;
        [Header("Reward")]
        public ECurrencyType rewardType;
        public int           reward;
    }
}
