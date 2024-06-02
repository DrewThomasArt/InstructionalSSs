using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    public static Save instance;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject saveMenu;
    public GameObject saving;
    public TextMeshProUGUI savingText;

    void Start()
    {
        instance = this;
    }

    public void OpenSaveMenu()
    {
        GameManager.instance.gameMenuOpen = true;
        saveMenu.SetActive(true);
        GameManager.instance.saveMenuActive = true;
    }

    public void CloseSaveMenu()
    {
        GameManager.instance.gameMenuOpen = false;
        saveMenu.SetActive(false);
        GameManager.instance.saveMenuActive = false;
        
    }

    public void SaveGame()
    {
        StartCoroutine(SavingCo());
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
        //ChestManager.instance.SaveChestData();
        EventManager.instance.SaveEventData();
        CloseSaveMenu();
    }

    public IEnumerator SavingCo()
    {
        saving.SetActive(true);
        savingText.text = "Saving";
        yield return new WaitForSeconds(.5f);
        savingText.text = "Saving .";
        yield return new WaitForSeconds(.5f);
        savingText.text = "Saving ..";
        yield return new WaitForSeconds(.5f);
        savingText.text = "Saving ...";
        yield return new WaitForSeconds(.5f);
        saving.SetActive(false);
    }
}

