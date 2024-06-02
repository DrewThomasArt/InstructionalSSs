using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueStarter : MonoBehaviour
{
    [HideInInspector]
    public int numberOfItemsHeld;
    [Header("Dialogue Lines")]
    //The lines the npcs say when the player talks to them
    [Tooltip("Set the dialogue scriptable object")]
    public Dialogue dialogue;
    public Dialogue itemQuestCompletedDialogue;
    //Check wheather the player is in range to talk to npc
    private bool canActivate;
    [Header("Activation")]
    //For different activation methods
    [Tooltip("Activates the dialog as soon as this script is activated. Keep in mind that the player character still has to be in the trigger zone")]
    public bool activateOnAwake;
    [Tooltip("Activates the dialog when the player presses the confirm button")]
    public bool activateOnButtonPress;
    [Tooltip("Activates the dialog when the player enters the trigger zone")]
    public bool activateOnEnter;
    [Tooltip("Activate a delay before showing the dialog")]
    public bool waitBeforeActivatingDialog;
    [Tooltip("Enter the duration of the delay in seconds")]
    public float waitTime;

    [Header("NPC Settings")]
    //Check if the player talks to a person for displaying a name tag
    [Tooltip("Display a name tag")]
    public bool displayName = true;

    [Header("Receive Settings")]
    [Tooltip("Receive an item after conversation")]
    public bool receiveItem;    
    [Tooltip("The item you will receive")]
    public Item itemToReceive;
    [Tooltip("Give an item after conversation")]
    public bool giveItem;
    [Tooltip("The item you will give")]
    public Item itemToGive;
    [Tooltip("Receive gold after conversation")]
    public bool receiveGold;
    [Tooltip("The amount of gold received")]
    public int goldAmount;

    [Header("Quest Settings")]
    //For completing quests after dialog
    [Tooltip("Enter the quest that should be completed. This quest has to be registered in the Quest Manager")]
    public string questToMark;
    [Tooltip("Mark a quest as complete after the dialogue")]
    public bool markComplete;
    [Tooltip("Will only complete the quest if player has the required item from the itemToCheck setting")]
    public bool requiresItemToComplete;
    public Item itemToCheck;
      [Tooltip("Whether or not you have the required item to complete the quest")]
    public bool gotItem;
    [Tooltip("Will only complete the quest if player has the received an item")]
    public bool requiresItemReceivedToComplete;

    [Header("Event Settings")]
    //For completing quests after dialog
    //public bool shouldActivateQuest;
    [Tooltip("Enter the event that should be completed. This quest has to be registered in the Event Manager")]
    public string eventToMark;
    [Tooltip("Mark an event as complete after the dialogue")]
    public bool markEventComplete;

    public UnityEvent onCanActivate;
    public UnityEvent onDialogueStart;

    // Update is called once per frame
    void Update()
    {
        DialogueTriggerByButton();
        DialogueTriggerByEnter();
    }
    private void DialogueTriggerByButton()
    {
        //Check for button input
        if (Input.GetButtonDown("Interact") && !DialogueManager.instance.dialogueBox.activeInHierarchy)
        {
            Debug.Log("Interacted");
            if (canActivate && !DialogueManager.instance.dialogueBox.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy && !GameManager.instance.battleActive)
            {
                PlayerController.instance.canMove = false;
                onDialogueStart?.Invoke();

                if (!DialogueManager.instance.dontOpenDialogueAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }
                    else
                    {
                        activateOnAwake = false;

                        if (markComplete && requiresItemToComplete)
                        {
                            if (!gotItem)
                            {
                                for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                                {
                                    if (GameManager.instance.itemsHeld[i] == itemToCheck.itemName)
                                    {
                                        gotItem = true;
                                        break;
                                    }
                                }
                            }
                                if (gotItem)
                                {
                                DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                                }
                        }

                    if (!gotItem)
                        {
                        DialogueManager.instance.ShowDialogue(dialogue.portraits, dialogue.lines, displayName);
                        }

                        if (gotItem)
                        {
                        DialogueManager.instance.ShowDialogue(itemQuestCompletedDialogue.portraits, itemQuestCompletedDialogue.lines, displayName);
                        }

                    DialogueManager.instance.dialogueStarter = this;

                        if (markComplete && !requiresItemToComplete && !requiresItemReceivedToComplete)
                        {
                        DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        }

                        if (markEventComplete)
                        {
                            DialogueManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }

                        if (giveItem && gotItem)
                        {
                            Shop.instance.selectedItem = itemToGive;
                            DialogueManager.instance.itemGiven = true;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.itemsHeld[i] = "";
                                    giveItem = false;
                                }

                                if (GameManager.instance.equipItemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.equipItemsHeld[i] = "";
                                    giveItem = false;
                                }
                            }
                        }

                        //Add item after conversation
                        if (receiveItem)
                        {
                            //Take the reference for isItem/isWeapon/isArmour from shop instance
                            Shop.instance.selectedItem = itemToReceive;

                            //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                            numberOfItemsHeld = 0;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] != "")
                                {
                                    numberOfItemsHeld++;
                                }
                            }

                            if (itemToReceive)
                            {
                                if (Shop.instance.selectedItem.item)
                                {
                                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + itemToReceive.itemName + "!";
                                        GameManager.instance.AddItem(itemToReceive.itemName);
                                        DialogueManager.instance.itemReceived = true;

                                        if (requiresItemReceivedToComplete)
                                        {
                                        DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                                        }

                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        DialogueManager.instance.fullInventory = true;
                                        receiveItem = true;
                                    }

                                }
                            }                            
                        }

                        //Add gold after conversation
                        if (receiveGold)
                        {
                            GameMenu.instance.gotItemMessageText.text = "You found " + receiveGold + " Gold!";
                            //StartCoroutine(gotItemMessageCo());
                            GameManager.instance.currentGold += goldAmount;
                            receiveGold = false;
                        }
                }
                }
            }
        }
    }

 private void DialogueTriggerByEnter()
    {
        //Check if dialog should be activated on awake or enter
        if (activateOnAwake || activateOnEnter)
        {
            //Check if player is in reach and if no other dialog is currently active
            if (canActivate && !DialogueManager.instance.dialogueBox.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy)
            {
                PlayerController.instance.canMove = false;
                onDialogueStart?.Invoke();
                //Set this to false to prevent activating dialog endlessly
                activateOnEnter = false;

                if (!DialogueManager.instance.dontOpenDialogueAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }
                    else
                    {
                        activateOnAwake = false;

                      if (markComplete && requiresItemToComplete)
                        {
                            if (!gotItem)
                            {
                                for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                                {
                                    if (GameManager.instance.itemsHeld[i] == itemToCheck.itemName)
                                    {
                                        gotItem = true;
                                        break;
                                    }
                                }
                            }
                                if (gotItem)
                                {
                                DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                                }
                        }

                        if (!gotItem)
                        {
                        DialogueManager.instance.ShowDialogueAuto(dialogue.portraits, dialogue.lines, displayName);
                        }

                        if (gotItem)
                        {
                        DialogueManager.instance.ShowDialogueAuto(itemQuestCompletedDialogue.portraits, itemQuestCompletedDialogue.lines, displayName);
                        }

                        DialogueManager.instance.dialogueStarter = this;
                        if (markComplete && !requiresItemToComplete && !requiresItemReceivedToComplete)
                        {
                        DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        }

  
                        if (markEventComplete)
                        {
                            DialogueManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }

                           if (giveItem && gotItem)
                        {
                            Shop.instance.selectedItem = itemToGive;
                            DialogueManager.instance.itemGiven = true;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.itemsHeld[i] = "";
                                    giveItem = false;
                                }

                                if (GameManager.instance.equipItemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.equipItemsHeld[i] = "";
                                    giveItem = false;
                                }
                            }
                        }

                        //Add item after conversation
                        if (receiveItem)
                        {
                            //Take the reference for isItem/isWeapon/isArmour from shop instance
                            Shop.instance.selectedItem = itemToReceive;

                            //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                            numberOfItemsHeld = 0;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] != "")
                                {
                                    numberOfItemsHeld++;
                                }
                            }

                            if (itemToReceive)
                            {
                                if (Shop.instance.selectedItem.item)
                                {
                                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + itemToReceive.itemName + "!";
                                        GameManager.instance.AddItem(itemToReceive.itemName);
                                        DialogueManager.instance.itemReceived = true;

                                        if (requiresItemReceivedToComplete)
                                        {
                                        DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                                        }
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        DialogueManager.instance.fullInventory = true;
                                        receiveItem = true;
                                    }
                                }
                            }                            
                        }

                        //Add gold after conversation
                        if (receiveGold)
                        {
                            GameMenu.instance.gotItemMessageText.text = "You found " + receiveGold + " Gold!";
                            //StartCoroutine(gotItemMessageCo());
                            GameManager.instance.currentGold += goldAmount;
                            receiveGold = false;
                        }
                        
                    }
                }
            }
        }
    }

 //Check if player enters trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = true;
            onCanActivate?.Invoke();
        }
    }

    //Check if player exits trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;

            if (!activateOnButtonPress)
            {
                activateOnEnter = true;
            }
        }
    }

    //Put in a slight delay between activating the dialogue and showing the dialogue
    IEnumerator waitCo()
    {
        yield return new WaitForSeconds(waitTime);
        waitBeforeActivatingDialog = false;  
    }
}

