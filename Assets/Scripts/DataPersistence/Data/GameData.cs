using System.Collections.Generic;
using Scriptables.Item;
using UnityEngine;
[System.Serializable]
public class GameData
{
    //Game
    public int currentDay;
    public int currentCoins;
    public int currentGridTier;
    public bool isFirstMachine;
    
    //ID das maquinas
    public List<ItemCategory> inventoryItemCategories;
    public List<int> inventoryItemTypeIDs;
    public List<int> inventoryItemTiers;
    public List<int> inventoryItemPositions;
    
    //Quantidade das maquinas
    public int numCoffeeMachines;
    public int numIceCreamMachines;
    public int numCakeMachines;
    public int numBreadMachines;
    
    //Som
    public float SFXVolume;
    public float MusicVolume;

    public GameData()
    {
        currentDay = 0;
        currentCoins = 0;
        currentGridTier = 0;
        isFirstMachine = true;
        
        inventoryItemCategories = new List<ItemCategory>();
        inventoryItemTypeIDs = new List<int>();
        inventoryItemTiers = new List<int>();
        inventoryItemPositions = new List<int>();
        
        numCoffeeMachines = 0;
        numIceCreamMachines = 0;
        numCakeMachines = 0;
        numBreadMachines = 0;
        
        SFXVolume = 1;
        MusicVolume = 1;
    }

}
