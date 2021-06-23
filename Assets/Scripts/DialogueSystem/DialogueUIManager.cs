using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    public static DialogueUIManager instance = null;

    public string currentDialogue;
    public string currentCharacter;

    public GameObject container;
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

    public void EnableDialogueOverlay()
    {
        container.SetActive(true);
    }

    public void DisableDialogueOverlay()
    {
        container.SetActive(false);
    }

    public void SetUpForOption()
    {
        characterParentObject.SetActive(false);
    }

    public void SetUpForConversation()
    {
        characterParentObject.SetActive(true);
    }

}
