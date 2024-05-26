using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemButton : MonoBehaviour {

    public Image buttonImage;
    public TextMeshProUGUI amountText;
    public int buttonValue;

    public void Press()
    {
        //Check if item window of game menu is currently active
        if (GameMenu.instance.itemWindow.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.buttonValue = buttonValue;
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));

                    //Enable use and discard buttons
                    GameMenu.instance.useItemButton.interactable = true;
                    GameMenu.instance.discardItemButton.interactable = true;
                    GameMenu.instance.btn = GameMenu.instance.itemUse;
                    GameMenu.instance.SelectFirstButton();
            }else
            {
                GameMenu.instance.activeItem = null;
            }
        }

        //Check if equip item window of game menu is currently active
        if (GameMenu.instance.equipWindow.activeInHierarchy)
        {
            if (GameManager.instance.equipItemsHeld[buttonValue] != "")
            {
                GameMenu.instance.buttonValue = buttonValue;
                GameMenu.instance.SelectEquipItem(GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[buttonValue]));

                    //Enable use and discard buttons
                    GameMenu.instance.useEquipItemButton.interactable = true;
                    GameMenu.instance.discardEquipItemButton.interactable = true;
                    GameMenu.instance.btn = GameMenu.instance.useEquipItemButton;
                    GameMenu.instance.SelectFirstButton();
            }else
            {
                GameMenu.instance.activeItem = null;
            }
        }
    }
    
}
