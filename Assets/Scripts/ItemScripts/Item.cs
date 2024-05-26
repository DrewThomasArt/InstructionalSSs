using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [Header("Item Type")]
    public bool item;
    public bool battleStatusModifier;
    public bool healStatusEffects;
    public bool revive;
    public bool offense;
    public bool defense;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int price;
    public int sellPrice;
    public Sprite itemSprite;

    [Header("Item Details")]
    public bool affectHP;
    public bool affectMP;
    public bool raiseAgility;
    public bool raiseOffense;
    public bool raiseDefense;
    public bool raiseExp;
    public int amountToChange;
    
    [Header("Strength/Defense Details")]
    public int offenseStrength;
    public int defenseStrength;  
}


