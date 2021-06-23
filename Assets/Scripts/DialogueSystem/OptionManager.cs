using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance = null;
    public GameObject[] buttons;

    private Option currentOption;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PresentOption(Option option)
    {
        currentOption = option;
        DialogueUIManager.instance.SetUpForOption();

        for (int i = 0; i < currentOption.options.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            // Finds text child of button, then sets it text 
            // The only children of these buttons right now is their text. If this changes, make sure the text is the first child
            buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentOption.options[i];
        }
    }

    public void ExecuteChoice(int index)
    {
        // Note that options must result in conversations right now. It should be possible to run an empty conversation if need be
        ConversationManager.instance.InitiateConversation(currentOption.possibleConversations[index]);
        foreach (GameObject button in buttons)
            button.SetActive(false);
        // This should probably be in ConversationManager, or renamed
        DialogueUIManager.instance.SetUpForConversation();
    }

}
