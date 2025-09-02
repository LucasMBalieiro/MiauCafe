using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxNumberMachines", menuName = "Game Data/Max Number Machines")]
public class MaxNumberMachines : ScriptableObject
{
    public DayData[] maxNumberMachinesPerDay;
}

[Serializable]public class DayData
{
    public int maxCoffeeMachine;
    public int maxIceCreamMachine;
    public int maxCakeMachine;
    public int maxBreadMachine;
}
