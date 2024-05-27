using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static GameManager instance;

    [Header("Currently active menus")]
    //Bools for checking if one of these menus is currently active
    public bool cutSceneActive;
    public bool gameMenuOpen;
    public bool dialogueActive;
    public bool battleActive;
    public bool fadingBetweenAreas;
    public bool eventLockActive;
    public bool cutSceneMusicActive;
    public bool itemMenu;
    public bool equipMenu;

    [Header("Character Bools")]
    //For checking if the player can move
    public bool confirmCanMove;

    [Header("Existing Game Items")]
    //Put items that the game uses here!
    public Item[] existingItems;

    [Header("Currently Owned Items")]
    public Item[] itemsInInventory;
    //View items that are in your inventory Can also be used to give the player some items to start the game with
    public string[] itemsHeld;
    //public int[] numberOfItems;
    public Item[] equipmentInInventory;
    public string[] equipItemsHeld;    
    //public int[] numberOfEquipItems;
    
    [Header("Gold Settings")]
    //The amount of gold currently owned by the player. Can also be used to give the player some gold to start the game with
    public int currentGold;

    void Start()
    {
      instance = this;  
      DontDestroyOnLoad(gameObject); 

        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if (itemsInInventory[i] != null)
            {
                itemsHeld[i] = itemsInInventory[i].itemName;
            }
            else
            {
                itemsHeld[i] = "";
            }            
        }

        for (int i = 0; i < equipmentInInventory.Length; i++)
        {
            if (equipmentInInventory[i] != null)
            {
                equipItemsHeld[i] = equipmentInInventory[i].itemName;
            }
            else
            {
                equipItemsHeld[i] = "";
            }
        }

         SortItems();
    }

    // Update is called once per frame
    void Update()
    {
         //Check if any meu is currently open and prevent the player from moving
        if (gameMenuOpen || dialogueActive || fadingBetweenAreas || eventLockActive)
        {
            PlayerController.instance.canMove = false;
            confirmCanMove = PlayerController.instance.canMove;
        } else
        {
            PlayerController.instance.canMove = true;
            confirmCanMove = PlayerController.instance.canMove;
        } 
    }

        //Returns the details of a list of items
    public Item GetItemDetails(string itemToGrab)
    {

        for(int i = 0; i < existingItems.Length; i++)
        {
            if(existingItems[i].itemName == itemToGrab)
            {
                return existingItems[i];
            }
        }
        
        return null;
    }

    //An algorithm to sort items in a list to avoid empty spaces in the inventory
    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    //An algorithm to sort items in a list to avoid empty spaces in the inventory
    public void SortEquipItems()
    {
        bool itemAFterSpace = true;

        while (itemAFterSpace)
        {
            itemAFterSpace = false;
            for (int i = 0; i < equipItemsHeld.Length - 1; i++)
            {
                if (equipItemsHeld[i] == "")
                {
                    equipItemsHeld[i] = equipItemsHeld[i + 1];
                    equipItemsHeld[i + 1] = "";

                    if (equipItemsHeld[i] != "")
                    {
                        itemAFterSpace = true;
                    }
                }
            }
        }
    }

    //A method to add items to the inventory
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
        if (Shop.instance.selectedItem.item)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == "" )//|| itemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    itemsHeld[newItemPosition] = itemToAdd;
                    //numberOfItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }
        }
        GameMenu.instance.ShowItems();
    }

    //A method for equipping items
    public void EquipItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == "")// || equipItemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = equipItemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    equipItemsHeld[newItemPosition] = itemToAdd;
                    //numberOfEquipItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }
        GameMenu.instance.ShowItems();      
    }

    //A method to add reward items after battle
    public void AddRewardItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == "")// || itemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    itemsHeld[newItemPosition] = itemToAdd;
                    //numberOfItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }              
        GameMenu.instance.ShowItems();
    }

    //A method to add reward equip items after battle
    public void AddRewardEquipItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < equipItemsHeld.Length; i++)
        {
            if (equipItemsHeld[i] == "") //|| equipItemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = equipItemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < existingItems.Length; i++)
            {
                if (existingItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    i = existingItems.Length;
                }
            }

            if (itemExists)
            {
                equipItemsHeld[newItemPosition] = itemToAdd;
                //numberOfEquipItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }
        GameMenu.instance.ShowItems();
    }

    //A method for removing items after usage
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        if (GameMenu.instance.activeItem.item || GameMenu.instance.activeItem.battleStatusModifier)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;
                    i = itemsHeld.Length;
                }
            }

            if (foundItem)
            {
                itemsHeld[itemPosition] = "";
                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {
                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }

        if (GameMenu.instance.activeItem.defense || GameMenu.instance.activeItem.offense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;
                    i = equipItemsHeld.Length;
                }
            }

            if (foundItem)
            {
                equipItemsHeld[itemPosition] = "";
                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {
                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }
    }

    //Check if inventory contains a specific item
    public bool HasItem(string searchItem)
    {
        for (int i = 0; i < itemsHeld.Length - 1; i++)
        {
            if (itemsHeld[i] == searchItem)
            {
            return true;
            }
        }

        for (int i = 0; i < equipItemsHeld.Length - 1; i++)
        {
            if (equipItemsHeld[i] == searchItem)
            {
            return true;
            }
        }
        return false;
    }
}
