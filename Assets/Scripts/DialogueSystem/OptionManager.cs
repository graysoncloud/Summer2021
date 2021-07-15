using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance = null;
    public GameObject[] buttons;

    private Color normalButtonColor = new Color(.6f, .6f, .6f, .4f);
    private Color shadedButtonColor = new Color(.25f, .25f, .25f, .3f);

    private Option currentOption;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void PresentOption(Option option)
    {
        DialogueUIManager.instance.SetUpForOption();
        currentOption = option;

        // Reset remnants of disabled buttons form previous option
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponent<Image>().color = normalButtonColor;

        }

        for (int i = 0; i < currentOption.choices.Length; i++)
        {
            Choice currentChoice = currentOption.choices[i];
            bool enabled = false;

            // Determine if choice will be choosable, or greyed out. Currently, choices can be enabled / disabled by prerequisite events and character attitudes
            if (currentChoice.checkAttitude)
            {
                Character charToCheck = SceneChangeManager.instance.characterPool.transform.Find(currentChoice.characterToCheck.ToString()).GetComponent<Character>();
                if (currentChoice.comparison.ToString() == "greaterThanOrEqual")
                {
                    if (charToCheck.attitude >= currentChoice.attitudeValueToCompare)
                        enabled = true;
                } 
                else if (currentChoice.comparison.ToString() == "lessThanOrEqual")
                {
                    if (charToCheck.attitude <= currentChoice.attitudeValueToCompare)
                        enabled = true;
                }
                else
                {
                    Debug.LogError("Invalid comparator: " + currentChoice.comparison.ToString());
                }

            } 
            else if (currentChoice.checkEvent)
            {
                // If player prefs had bools this would look a bit simpler. As is, 1 is on, 0 is off
                if (PlayerPrefs.GetInt(currentChoice.eventToCheck.ToString()) == 1)
                {
                    enabled = true;
                }

            } 
            else
            {
                // Default case means there's no restrictions on the option
                enabled = true;
            }

            buttons[i].gameObject.SetActive(true);

            if (!enabled)
            {
                buttons[i].gameObject.GetComponent<Button>().interactable = false;
                // Change sprite ideally, not just color. But maybe just color would work
                buttons[i].gameObject.GetComponent<Image>().color = shadedButtonColor;
            }


            // The only children of these buttons right now is their text. If this changes, make sure the text is the first child
            buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentOption.choices[i].choiceText;
        }
    }

    public void ExecuteChoice(int index)
    {
        Choice choiceToExecute = currentOption.choices[index];

        if (choiceToExecute.changesAttitude)
        {
            // Attitude To Change is a charName enumerator
            Character charToChange = SceneChangeManager.instance.characterPool.transform.Find(choiceToExecute.attitudeToChange.ToString()).GetComponent<Character>();
            Debug.Log(charToChange.name);
            charToChange.attitude += choiceToExecute.amountToAlter;
        }

        if (choiceToExecute.logsEvent)
        {
            PlayerPrefs.SetInt(choiceToExecute.eventToLog.ToString(), 1);
        }


        foreach (GameObject button in buttons)
            button.SetActive(false);

        // Execute Consequences

        // Note that options must result in conversations right now. It should be possible to run an empty conversation if need be
        ConversationManager.instance.StartConversation(currentOption.possibleConversations[index]);
        // This should probably be in ConversationManager, or renamed
    }

}
