using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueStarter : MonoBehaviour
{
[Header("Dialogue Lines")]
    //The lines the npcs say when the player talks to them
    [Tooltip("Set the dialog scriptable object")]
    public Dialogue dialogue;
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

    [Header("Quest Settings")]
    //For completing quests after dialog
    [Tooltip("Enter the quest that should be completed. This quest has to be registered in the Quest Manager")]
    public string questToMark;
    [Tooltip("Mark a quest as complete after the dialogue")]
    public bool markComplete;

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
         //Check if dialogue should be activated on awake or enter
        if (activateOnAwake || activateOnEnter)
        {
            //Check if player is in reach and if no other dialogue is currently active
            if (canActivate && !DialogueManager.instance.dialogueBox.activeInHierarchy //&& !GameMenu.instance.menu.activeInHierarchy
            )
            {
                PlayerController.instance.canMove = false;
                onDialogueStart?.Invoke();
                //Set this to false to prevent activating dialogue endlessly
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

                        DialogueManager.instance.ShowDialogueAuto(dialogue.portraits, dialogue.lines, displayName);
                        DialogueManager.instance.dialogueStarter = this;
                        if (markComplete)
                        {
                        DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        }
                        if (markEventComplete)
                        {
                        DialogueManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                    }
                }
            }
        }

 //Check for button input
        if (Input.GetButtonDown("Interact") && !DialogueManager.instance.dialogueBox.activeInHierarchy)
        {
            if (canActivate && !DialogueManager.instance.dialogueBox.activeInHierarchy //&& !GameMenu.instance.menu.activeInHierarchy
            )
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

                    DialogueManager.instance.ShowDialogue(dialogue.portraits, dialogue.lines, displayName);
                    DialogueManager.instance.dialogueStarter = this;
                        if (markComplete)
                        {
                        DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        }
                        if (markEventComplete)
                        {
                            DialogueManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
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

