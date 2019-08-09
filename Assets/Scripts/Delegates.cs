using UnityEngine;

namespace Delegates
{
    public delegate void NoneValue();

    

    public delegate void IntValue(int value);

    public delegate void DoubleStringValue(string first, string second);
    
    public delegate void InstantiateValue(GameObject point);

    public delegate void ObstacleTerminatedValue(EObstacleTerminator value);
    
    public delegate void PlayerDefeatValue(EPlayerDefeat value);

}