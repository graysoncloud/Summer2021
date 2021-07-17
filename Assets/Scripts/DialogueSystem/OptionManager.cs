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

    public void StartOption(Option option)
    {
        GameManager.instance.sequenceActive = true;

        currentOption = option;

        // This block of code allows instant forks to occur (no choosing necessary)
        if (option.automatic)
        {
            for (int i = 0; i < option.paths.Length; i++)
            {
                if (option.paths[i].checkAttitude)
                {
                    string attitudeToCheck = option.paths[i].characterToCheck.ToString() + "Attitude";
                    if (option.paths[i].comparison.ToString() == "greaterThanOrEqual")
                    {
                        if (PlayerPrefs.GetInt(attitudeToCheck) >= option.paths[i].attitudeValueToCompare)
                            ExecutePath(i);
                        return;
                    }
                    else if (option.paths[i].comparison.ToString() == "lessThanOrEqual")
                    {
                        if (PlayerPrefs.GetInt(attitudeToCheck) <= option.paths[i].attitudeValueToCompare)
                            ExecutePath(i);
                        return;
                    }
                    else
                    {
                        Debug.LogError("Invalid comparator: " + option.paths[i].comparison.ToString());
                    }
                }
                else if (option.paths[i].checkEvent)
                {
                    if (PlayerPrefs.GetInt(option.paths[i].eventToCheck.ToString()) == 1)
                    {
                        ExecutePath(i);
                        return;
                    }
                } 
                // Default path, if no restrictions are present
                else if (!option.paths[i].checkEvent && !option.paths[i].checkAttitude)
                {
                    ExecutePath(i);
                    return;
                }
            }

            Debug.LogError("No valid (default) pathway detected");
        }



        DialogueUIManager.instance.SetUpForOption();

        // Reset remnants of disabled buttons form previous option
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
            button.GetComponent<Image>().color = normalButtonColor;

        }

        for (int i = 0; i < currentOption.paths.Length; i++)
        {
            Path currentPath = currentOption.paths[i];
            bool enabled = false;

            // Determine if choice will be choosable, or greyed out. Currently, choices can be enabled / disabled by prerequisite events and character attitudes
            if (currentPath.checkAttitude)
            {

                string attitudeToCheck = currentPath.characterToCheck.ToString() + "Attitude";

                if (currentPath.comparison.ToString() == "greaterThanOrEqual")
                {
                    if (PlayerPrefs.GetInt(attitudeToCheck) >= currentPath.attitudeValueToCompare)
                        enabled = true;
                } 
                else if (currentPath.comparison.ToString() == "lessThanOrEqual")
                {
                    if ((PlayerPrefs.GetInt(attitudeToCheck) <= currentPath.attitudeValueToCompare))
                        enabled = true;
                }
                else
                {
                    Debug.LogError("Invalid comparator: " + currentPath.comparison.ToString());
                }

            } 
            else if (currentPath.checkEvent)
            {
                // If player prefs had bools this would look a bit simpler. As is, 1 is on, 0 is off
                if (PlayerPrefs.GetInt(currentPath.eventToCheck.ToString()) == 1)
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
            buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentOption.paths[i].choiceText;
        }
    }

    public void ExecutePath(int index)
    {
        Path choiceToExecute = currentOption.paths[index];

        if (choiceToExecute.changesAttitude)
        {
            // Attitude To Change is a charName enumerator
            string charToChange = choiceToExecute.attitudeToChange.ToString();
            PlayerPrefs.SetInt(charToChange + "Attitude", PlayerPrefs.GetInt(charToChange + "Attitude") + choiceToExecute.amountToAlter);
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
