using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class GameMenu : MonoBehaviour {
    public static GameMenu instance;
    Navigation customNav = new Navigation();

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject menu;
    public Button btn;
    public Button item;
    public Button equip;
    public Button skills;
    public Button status;
    public Button close;
    public ItemButton[] itemButtons;
    public ItemButton[] equipItemButtons;
    public Button[] itemButtonsB;
    public Button[] equipItemButtonsB;
    public ReadHighlightedButton[] highlightedItemButtons;
    public ReadHighlightedButton[] highlightedEquipItemButtons;
    public Button itemMenuItem0;
    public Button equipMenuItem0;
     public GameObject gold;
    public TextMeshProUGUI goldText;
    public GameObject itemWindow;
    public GameObject equipWindow;
    public Image itemSprite;
    public Image equipItemSprite;
    public bool itemSelected;
    public GameObject gotItemMessage;
    public TextMeshProUGUI gotItemMessageText;
    public Item activeItem;
    public Button useItemButton;
    public Button useEquipItemButton;
    public TextMeshProUGUI itemName, itemDescription, useButtonText, equipItemName, equipItemDescription, equipUseButtonText;
    public int buttonValue;

    //Event sytsem
    public EventSystem es;

    //Game menu
    public Button discardItemButton;
    public Button discardEquipItemButton;
    public Button itemUse;

    [Header("Item Selection")]
    public bool itemConfirmed = false;
    public string selectedItem;
    
    [Header("Linked Scenes")]
    public string mainMenuScene;
    public string loadGameScene;

    [Header("Sound")]
    public int openMenuButtonSound;
    public int cancelButtonSound;
    
    private bool menuCooldownComplete;

    // Use this for initialization
    void Start () {
        instance = this;     
    }
	
	void Update () {

        //Open game menu
        if (Input.GetButtonDown("OpenGameMenu") && !menu.activeInHierarchy)
        {
            //Check if game menu can be opened
            if (ScreenFade.instance.fading == false && !GameManager.instance.battleActive && !GameManager.instance.dialogueActive && !GameManager.instance.cutSceneActive)
            {
                if (!menu.activeInHierarchy)
                {
                    AudioManager.instance.PlaySFX(openMenuButtonSound);
                }

                //Prevents highlighted button from going back to btn = btn when pressing OpenGameMenu during open game menu 
                if (!menu.activeInHierarchy)
                {
                    btn = item;
                    SelectFirstButton();
                }
                    menu.SetActive(true);
                    //UpdateMainStats();
                    GameManager.instance.gameMenuOpen = true;
                    menuCooldownComplete = false;
                    StartCoroutine(MenuCooldown());
            }
        }
        
        //Close game menu
        if (!GameManager.instance.battleActive)
        {
            if (Input.GetButtonDown("OpenGameMenu") && GameManager.instance.gameMenuOpen && menuCooldownComplete)
            {
                if (GameManager.instance.gameMenuOpen)
                {
                    AudioManager.instance.PlaySFX(cancelButtonSound);
                }

              
                CloseMenu();
                CloseAllWindows();

            }
        }
        
	}

    public void OpenGameMenu()
    {        
        menu.SetActive(true);
        UpdateMainStats();
        GameManager.instance.gameMenuOpen = true;
    }

private IEnumerator MenuCooldown()
    {
    yield return new WaitForSeconds(0.2f);
    menuCooldownComplete = true;
    }

    public void ItemConfirmed(bool callItemConfirmed)
    {
    itemConfirmed = callItemConfirmed;   
    }

    public void SelectFirstButton()
    {        
        es.SetSelectedGameObject(btn.gameObject);
        // Select the button
        btn.Select();
        // Highlight the button
        btn.OnSelect(null);
        Debug.Log(btn.gameObject.name);       
    }

    public void OpenItemWindow()
    {
        buttonValue = 0;
        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        useItemButton.interactable = false;
        discardItemButton.interactable = false;
       
        for (int i = 0; i < itemButtonsB.Length; i++)
        {
        itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.itemsHeld[i] != "")
            {
            itemButtonsB[i].navigation = customNav;
            }

            //Make only those item buttons interactable which actually hold items 
            if (GameManager.instance.itemsHeld[i] == "")
            {
            itemButtonsB[i].interactable = false;
            }
        }

        itemWindow.SetActive(true);
        GameManager.instance.itemMenu = true;
        ShowItems();

        EventSystem.current.SetSelectedGameObject(null);
        btn = itemMenuItem0;
        SelectFirstButton();       
    }

    public void UpdateMainStats()
    {
    //playerStats = GameManager.instance.characterStatus;
    goldText.text = GameManager.instance.currentGold.ToString() + "G";
    }

    public void CloseMenu()
    {
        menuCooldownComplete = true;
        //playerStats.Clear();
        itemWindow.SetActive(false);
        equipWindow.SetActive(false);
        menu.SetActive(false);
        //itemCharChoiceMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;
        GameManager.instance.itemMenu = false;
        GameManager.instance.equipMenu = false;
        //GameManager.instance.itemCharChoiceMenu = false;
    }

    public void CloseWindow()
    {

    CloseMenu();
    CloseAllWindows();
    menu.SetActive(true);
    GameManager.instance.gameMenuOpen = true;
    menuCooldownComplete = true;
    }

    public void CloseAllWindows()
    {
                //Close item window
                if (itemWindow.activeInHierarchy && !useItemButton.interactable)
                {
                    itemWindow.SetActive(false);

                    //Set disabled buttons back to interactable
                    // item.interactable = true;
                    // equip.interactable = true;
                    // skills.interactable = true;
                    // status.interactable = true;
                    // close.interactable = true;
                    // //quit.interactable = true;
                    btn = item;
                    SelectFirstButton();
                }

                //Close equip window
                if (equipWindow.activeInHierarchy && !useEquipItemButton.interactable)
                {
                    equipWindow.SetActive(false);

                    //Set disabled buttons back to interactable
                    // item.interactable = true;
                    // equip.interactable = true;
                    // skills.interactable = true;
                    // status.interactable = true;
                    // close.interactable = true;
                    // //quit.interactable = true;
                    btn = item;
                    SelectFirstButton();
                }

                //Close item action buttons
                if (itemWindow.activeInHierarchy && useItemButton.interactable)
                {
                    for (int i = 0; i < itemButtonsB.Length; i++)
                    {
                    itemButtonsB[i].interactable = true;
                    }

                    for (int i = 0; i < itemButtonsB.Length; i++)
                    {
                        //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                        if (GameManager.instance.itemsHeld[i] != "")
                        {
                        itemButtonsB[i].navigation = customNav;
                        }

                        //Make only those item buttons interactable which actually hold items 
                        if (GameManager.instance.itemsHeld[i] == "")
                        {
                        itemButtonsB[i].interactable = false;
                        }
                    }
                }

                //Close equip action buttons
                if (equipWindow.activeInHierarchy && useEquipItemButton.interactable)
                {
                    for (int i = 0; i < equipItemButtonsB.Length; i++)
                    {
                    equipItemButtonsB[i].interactable = true;
                    }

                    for (int i = 0; i < equipItemButtonsB.Length; i++)
                    {
                        //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                        if (GameManager.instance.equipItemsHeld[i] != "")
                        {
                        equipItemButtonsB[i].navigation = customNav;
                        }

                        //Make only those item buttons interactable which actually hold items 
                        if (GameManager.instance.equipItemsHeld[i] == "")
                        {
                        equipItemButtonsB[i].interactable = false;
                        }
                    }
                }
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {            
            itemButtons[i].buttonValue = i;
            highlightedItemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
            itemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemName;
            } else
            {
            itemButtons[i].amountText.text = "";
            }
        }
    }

    public void UpdateItemDetailsOnHighlighted(Item newItem)
    {
        activeItem = newItem;
        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
        itemSprite.sprite = activeItem.itemSprite;
    }

    public void UpdateNoHighlightedButton()
    {
        activeItem = null;
        SelectItem(activeItem);
        
    }

    public void SelectItem(Item newItem)
    {
        if (newItem != null)
        { //If any item was selected, show proper description and item sprite
            itemSelected = true;
            activeItem = newItem;
            if (activeItem.item)
            {
                useButtonText.text = "Use";
            }
            if (activeItem.offense || activeItem.defense)
            {
                useButtonText.text = "Equip";
            }

            itemName.text = activeItem.itemName;
            itemDescription.text = activeItem.description;
            //Set item sprite to visible
            itemSprite.color = new Color(1, 1, 1, 1);
            itemSprite.sprite = activeItem.itemSprite;
        }
        else
        { //If no item is selected/item inventory empty don't show anything in description panel
            itemSelected = false;
            activeItem = null;
            itemName.text = "";
            itemDescription.text = "";
            //Set item sprite to invisible
            itemSprite.color = new Color(1, 1, 1, 0);
        }
    }

    public void DiscardItem()
    {
        for (int i = 0; i < itemButtonsB.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            equipItemButtonsB[i].interactable = true;
        }

        buttonValue = 0;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        useItemButton.interactable = false;
        discardItemButton.interactable = false;
        
        if (itemWindow.activeInHierarchy)
        {
            for (int i = 0; i < itemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.itemsHeld[i] != "")
                {
                    itemButtonsB[i].navigation = customNav;
                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.itemsHeld[i] == "")
                {
                    itemButtonsB[i].interactable = false;
                }
            }
        }
        
        if (equipWindow.activeInHierarchy)
        {
            for (int i = 0; i < equipItemButtonsB.Length; i++)
            {
                //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
                if (GameManager.instance.equipItemsHeld[i] != "")
                {
                    equipItemButtonsB[i].navigation = customNav;
                }

                //Make only those item buttons interactable which actually hold items 
                if (GameManager.instance.equipItemsHeld[i] == "")
                {
                    equipItemButtonsB[i].interactable = false;
                }
            }
        }
        
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
            if (itemWindow.activeInHierarchy)
            {
                    btn = itemMenuItem0;
                    SelectFirstButton();               
            }
            if (equipWindow.activeInHierarchy)
            {
                    btn = equipMenuItem0;
                    SelectFirstButton();               
            }

            if (!GameManager.instance.HasItem(activeItem.itemName))
            {
                SelectItem(null);
            }            
        }
        if (itemWindow.activeInHierarchy)
        {
            OpenItemWindow();
        }

        if (equipWindow.activeInHierarchy)
        {
            OpenEquipWindow();
        }        
    }

    public void OpenEquipWindow()
    {
        buttonValue = 0;
        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        useEquipItemButton.interactable = false;
        discardEquipItemButton.interactable = false;

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
        equipItemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < equipItemButtonsB.Length; i++)
        {
            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.equipItemsHeld[i] != "")
            {
            equipItemButtonsB[i].navigation = customNav;
            }

            //Make only those item buttons interactable which actually hold items 
            if (GameManager.instance.equipItemsHeld[i] == "")
            {
            equipItemButtonsB[i].interactable = false;   
            }
        }

        equipWindow.SetActive(true);
        GameManager.instance.equipMenu = true;
        //EquipStatsChar();
        UpdateMainStats();
        ShowEquipItems();
        
        btn = equipMenuItem0;
        SelectFirstButton();
    }

    public void ShowEquipItems()
    {       
        GameManager.instance.SortEquipItems();

        for (int i = 0; i < equipItemButtons.Length; i++)
        {
            equipItemButtons[i].buttonValue = i;
            highlightedEquipItemButtons[i].buttonValue = i; 

            if (GameManager.instance.equipItemsHeld[i] != "")
            {
            equipItemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.equipItemsHeld[i]).itemName;   
            }
            else
            {
                equipItemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectEquipItem(Item newItem)
    {
        if (newItem != null)
        {
            activeItem = newItem;
            if (activeItem.item)
            {
                equipUseButtonText.text = "Use";
            }
            if (activeItem.offense || activeItem.defense)
            {
                equipUseButtonText.text = "Equip";
            }

            equipItemName.text = activeItem.itemName;
            equipItemDescription.text = activeItem.description;
            //Set item sprite to visible
            equipItemSprite.color = new Color(1, 1, 1, 1);
            equipItemSprite.sprite = activeItem.itemSprite;
        }
        //f no item is selected/item inventory empty don't show anything in description panel
        if (activeItem == null)
        {
            itemSelected = false;
            activeItem = null;
            equipItemName.text = "";
            equipItemDescription.text = "";            
        }
    }
        public void PlayButtonSound(int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }
}