using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
     //Make instance of this script to be able reference from other scripts!
    public static DialogueManager instance;

[Header("Initialization")]
    //Game objects used by this code
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialogueBox;
    public GameObject nameBox;
    public Image portrait;
    public DialogueStarter dialogueStarter;

[Header("Dialogue")]
    //Confirm the spoken lines + portraits
    public Sprite[] dialoguePortraits;
    string portraitText;
    public string[] dialogueLines;
    public int currentLine;
    public bool justStarted;
    public bool fullInventory;
    public bool itemRecieved;
    public bool itemGiven;
    public GameObject dialogueObject;
    [HideInInspector]
    public bool dontOpenDialogueAgain;

    [Header("Dialogue Type")]
    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;
    private string eventToMark;
    private bool markEventComplete1;
    private bool shouldMarkEvent;
    public GameObject NPC;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the dialogue box is shown by the DialogueStarter script
        if(dialogueBox.activeInHierarchy)
        {
            //Progress through the lines by pressing the following button/s
            if(Input.GetButtonUp("Interact"))
            {
                //Check if dialogue just opened without any progression
                if (!justStarted)
                {
                    //Prevents opening the dialog box again after confirming the last line with button press
                    dontOpenDialogueAgain = false;
                    currentLine++;

                    //Check if the current line is within the length of dialog lines and close the dialogue box if the last line was reached
                    if (currentLine >= dialogueLines.Length)
                        {
                        dialogueBox.SetActive(false);

                        if (itemRecieved && !fullInventory)
                            {
                                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
                                StartCoroutine(gotItemMessageCo());
                                itemRecieved = false;
                            }

                        if (itemGiven)
                            {
                                GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
                                StartCoroutine(gotItemMessageCo());
                                itemGiven = false;
                            }

                        if (fullInventory)
                            {
                                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                                StartCoroutine(gotItemMessageCo());
                                fullInventory = false;
                            }

                        GameManager.instance.dialogueActive = false;

                            // //Marks quest complete
                            if (shouldMarkQuest)
                            {
                                shouldMarkQuest = false;
                                if (markQuestComplete)
                                {
                                    QuestManager.instance.MarkQuestComplete(questToMark);
                                }
                                else
                                {
                                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                                }
                            }

                            //Marks event complete
                            if (shouldMarkEvent)
                            {
                                shouldMarkEvent = false;
                                if (markEventComplete1)
                                {
                                    EventManager.instance.MarkEventComplete(eventToMark);
                                }
                                else
                                {
                                    EventManager.instance.MarkEventIncomplete(eventToMark);
                                }
                            }
                        }
                    else
                    {
                        //Show name 
                        CheckIfName();
                        CheckIfPortrait();
                        dialogueText.text = dialogueLines[currentLine];
                    }   
                } else
                    {
                    justStarted = false;
                    }
            }        
        }
    }

//Method to call the dialogue. Needs the lines as string array + bool for 
//Use this to call a dialogue that is activated by a button press
public void ShowDialogue(Sprite[] portraits, string[] newLines, bool displayName)
    {
        if (newLines.Length != 0)
        {
            dialoguePortraits = portraits;
            dialogueLines = newLines;

            currentLine = 0;

            CheckIfName();
            CheckIfPortrait();

            dialogueText.text = dialogueLines[currentLine];
            dialogueBox.SetActive(true);

            justStarted = true;

            nameBox.SetActive(displayName);

            GameManager.instance.dialogueActive = true;
        }

            if (newLines.Length == 0)
        {
            if (itemRecieved && !fullInventory)
            {
                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemRecieved = false;
            }

            if (itemGiven)
            {
                GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemGiven = false;
            }

            if (fullInventory)
            {
                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                StartCoroutine(gotItemMessageCo());
                fullInventory = false;
            }

                if (shouldMarkQuest)
            {
                shouldMarkQuest = false;
                if (markQuestComplete)
                {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }
                else
                {
                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                }
            }

            //Marks event complete
            if (shouldMarkEvent)
            {
                shouldMarkEvent = false;
                if (markEventComplete1)
                {
                    EventManager.instance.MarkEventComplete(eventToMark);
                }
                else
                {
                    EventManager.instance.MarkEventIncomplete(eventToMark);
                }
            }
        }
    }

//Method to call the dialogue. Needs the lines as string array + bool for 
//Use this to call a dialogue that is activated on awake/enter
         public void ShowDialogueAuto(Sprite[] portraits, string[] newLines, bool displayName)
    {
        if (newLines.Length != 0)
        {
            dialoguePortraits = portraits;
            dialogueLines = newLines;

            currentLine = 0;

            CheckIfName();
            CheckIfPortrait();

            dialogueText.text = dialogueLines[currentLine];
            dialogueBox.SetActive(true);

            justStarted = false;

            nameBox.SetActive(displayName);

            GameManager.instance.dialogueActive = true;
        }

            if (newLines.Length == 0)
        {
            if (itemRecieved && !fullInventory)
            {
                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemRecieved = false;
            }

            if (itemGiven)
            {
                GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemGiven = false;
            }

            if (fullInventory)
            {
                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                StartCoroutine(gotItemMessageCo());
                fullInventory = false;
            }

                if (shouldMarkQuest)
            {
                shouldMarkQuest = false;
                if (markQuestComplete)
                {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }
                else
                {
                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                }
            }

            //Marks event complete
            if (shouldMarkEvent)
            {
                shouldMarkEvent = false;
                if (markEventComplete1)
                {
                    EventManager.instance.MarkEventComplete(eventToMark);
                }
                else
                {
                    EventManager.instance.MarkEventIncomplete(eventToMark);
                }
            }
        }
    }

    //Show name tag on dialogue box. Start a line with // to indicate a name. DialogueStarter script must have the displayName bool true to display names 
    public void CheckIfName()
    {

        if (dialogueLines[currentLine].StartsWith("//"))
        {
            nameText.text = dialogueLines[currentLine].Replace("//", "");
            currentLine++;
        }
        else
        {
            CheckIfPortrait();
        }

    }

    public void CheckIfPortrait()
    {
        
        if (dialoguePortraits != null)
        {
            if (dialogueLines[currentLine].StartsWith("**"))
            {
                portraitText = dialogueLines[currentLine].Replace("**", "");

                if (portraitText != "")
                {
                    portrait.color = new Color(1, 1, 1, 1);
                    int currentPortrait = Convert.ToInt32(portraitText);
                    portrait.sprite = dialoguePortraits[currentPortrait];
                    currentLine++;
                }
                else
                {
                    portrait.color = new Color(1, 1, 1, 0);
                    currentLine++;
                }

            }
        } 
    }

    //Method to complete a quest after dialogue
    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;
        shouldMarkQuest = true;
    }

    //Method to complete an event after dialogue
    public void ActivateEventAtEnd(string eventName, bool markEventComplete)
    {
        eventToMark = eventName;
        markEventComplete1 = markEventComplete;
        shouldMarkEvent = true;
    }

    public IEnumerator gotItemMessageCo()
    {
        yield return new WaitForSeconds(.5f);
        GameMenu.instance.gotItemMessage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameMenu.instance.gotItemMessage.SetActive(false);
    }
}
