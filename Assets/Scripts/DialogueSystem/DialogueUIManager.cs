using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager instance = null;

    public string currentDialogue;
    public string currentCharacter;

    public GameObject UIParent;
    public TextMeshProUGUI dialogueTextObject;
    public TextMeshProUGUI characterTextObject;
    public GameObject characterParentObject;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Possibly could change
    public void SetUpForOption()
    {
        characterParentObject.SetActive(false);
    }

    public void SetUpForConversation()
    {
        UIParent.SetActive(true);
        characterParentObject.SetActive(true);
    }

    public void SetUpForAnimation()
    {
        UIParent.SetActive(false);
    }

    public void SetUpForSceneChange()
    {
        UIParent.SetActive(false);
    }

    public void turnOffDialogueUI()
    {
        UIParent.SetActive(false);
    }

}
