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
    }

    public void SetUpForNews()
    {
        characterParentObject.SetActive(false);
        dialogueTextObject.alignment = TextAlignmentOptions.Top;
        dialogueTextObject.alignment = TextAlignmentOptions.Midline;
        dialogueTextObject.fontStyle = FontStyles.Italic;
        dialogueTextObject.rectTransform.sizeDelta = new Vector2(420, 67);
        dialogueTextObject.rectTransform.localPosition = new Vector3(1.3f, 2.5f, 0);
    }

    public void SetUpDefault()
    {
        characterParentObject.SetActive(true);
        dialogueTextObject.alignment = TextAlignmentOptions.TopLeft;
        dialogueTextObject.fontStyle = FontStyles.Normal;
        dialogueTextObject.rectTransform.sizeDelta = new Vector2(570, 67);
        dialogueTextObject.rectTransform.localPosition = new Vector3(1.3f, -1.1f, 0);
    }

    public void SetUpForOption()
    {
        UIParent.SetActive(true);
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

    public void SetUpForCharacterFade()
    {
        UIParent.SetActive(false);
    }

    public void turnOffDialogueUI()
    {
        UIParent.SetActive(false);
    }

}
