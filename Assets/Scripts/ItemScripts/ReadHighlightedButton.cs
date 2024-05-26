using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

//This script reads the currently highlighted item button and updates the item information in the info panel

public class ReadHighlightedButton : MonoBehaviour, ISelectHandler
{
    public int buttonValue;
    // ItemButton is highlighted, show item information
    public void OnSelect(BaseEventData eventData)
    {
        // Show message in debug log for testing
        Debug.Log("<color=red>Event:</color> Completed selection.");

        
        // If viewing item window in game menu
        if (GameMenu.instance.itemWindow.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }else
            {
                GameMenu.instance.itemName.text = "Select an item!";
                GameMenu.instance.itemDescription.text = "No items!";
                GameMenu.instance.itemSprite.color = new Color(1, 1, 1, 0);
            }
        }

        // If viewing equip window in game menu
        if (GameMenu.instance.equipWindow.activeInHierarchy)
        {
            if (GameManager.instance.equipItemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectEquipItem(GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[buttonValue]));
            }else
            {
                GameMenu.instance.equipItemName.text = "Select an item!";
                GameMenu.instance.equipItemDescription.text = "No equipment!";
                //Set item sprite to invisible
                GameMenu.instance.equipItemSprite.color = new Color(1, 1, 1, 0);
            }

        }
    }
}

