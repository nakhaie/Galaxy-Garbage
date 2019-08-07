using UnityEngine;

namespace Delegates
{
    public delegate void NoneValue();

    public delegate void BoolValue(bool value);

    public delegate void IntValue(int value);

    public delegate void DoubleStringValue(string first, string second);
    
    public delegate void InstantiateValue(GameObject point);

    public delegate void ObstacleTerminatedValue(ObstacleTerminator value);

}