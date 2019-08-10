using UnityEngine;

namespace Delegates
{
    public delegate void NoneValue();

    public delegate void IntValue(int value);

    public delegate void DoubleStringValue(string first, string second);
    
    public delegate void InstantiateValue(GameObject point);

    public delegate void ObstacleTerminatedValue(EStatusType state);
    
    public delegate void PlayerDefeatValue(EStatusType state);
    
    public delegate void PlayerDamageValue(EStatusType state, int amount);
    
    public delegate void PlayerHpValue(float amount);

}