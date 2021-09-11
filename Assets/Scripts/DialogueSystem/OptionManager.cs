using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance = null;
    public GameObject[] buttonBackgrounds;
    public UIButton[] buttonTexts;

    private Color normalBackgroundColor = new Color(1f, 1f, 1f, 1f);
    private Color shadedBackgroundColor = new Color(.75f, .75f, .75f, .7f);

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

        //Debug.Log(PlayerPrefs.GetInt("BarneyAttitude"));
        //Debug.Log(PlayerPrefs.GetFloat("Stress"));

        // This block of code allows instant forks to occur (no choosing necessary)
        if (option.automatic)
        {
            for (int i = 0; i < option.paths.Length; i++)
            {
                bool execute = true;
                if (option.paths[i].checkAttitude)
                {
                    string attitudeToCheck = option.paths[i].characterToCheck.ToString() + "Attitude";
                    if (option.paths[i].attitudeComparison.ToString() == "greaterThanOrEqual")
                    {
                        if (PlayerPrefs.GetInt(attitudeToCheck) >= option.paths[i].attitudeToCompare)
                            execute = (execute && true);
                        else
                            execute = (execute && false);
                    }
                    else if (option.paths[i].attitudeComparison.ToString() == "lessThanOrEqual")
                    {
                        if (PlayerPrefs.GetInt(attitudeToCheck) <= option.paths[i].attitudeToCompare)
                            execute = (execute && true);
                        else
                            execute = (execute && false);
                    }
                    else
                    {
                        Debug.LogError("Invalid comparator: " + option.paths[i].attitudeComparison.ToString());
                    }
                }
                if (option.paths[i].checkEvent)
                {
                    //Debug.Log(PlayerPrefs.GetInt(option.paths[i].eventToCheck.ToString()) + option.paths[i].eventToCheck.ToString());
                    if (PlayerPrefs.GetInt(option.paths[i].eventToCheck.ToString()) == 1)
                        execute = (execute && true);
                    else
                        execute = (execute && false);

                } 
                if (option.paths[i].checkStress)
                {
                    if (option.paths[i].stressComparison.ToString() == "greaterThanOrEqual")
                    {
                        if (PlayerPrefs.GetInt("Stress") >= option.paths[i].stressCheckAmount)
                            execute = (execute && true);
                        else
                            execute = (execute && false);
                    }
                    else if (option.paths[i].stressComparison.ToString() == "lessThanOrEqual")
                    {
                        if (PlayerPrefs.GetInt("Stress") <= option.paths[i].stressCheckAmount)
                            execute = (execute && true);
                        else
                            execute = (execute && false);
                    }
                    else
                    {
                        Debug.LogError("Invalid comparator: " + option.paths[i].attitudeComparison.ToString());
                    }
                }
                if (option.paths[i].checkNumAltContractsFinished)
                {
                    if (PlayerPrefs.GetInt("OptionalCompleted") > option.paths[i].minimumRequired)
                        execute = (execute && true);
                    else
                        execute = (execute && false);
                }

                // Default path, if no restrictions are present
                else if (!option.paths[i].checkEvent && !option.paths[i].checkAttitude && !option.paths[i].checkStress)
                {
                    execute = true;
                }

                if (execute)
                {
                    ExecutePath(i);
                    return;
                }
            }

            Debug.LogError("No valid (default) pathway detected");
        }
        // End of auto-fork code

        DialogueUIManager.instance.SetUpForOption();

        // Reset remnants of disabled buttons form previous option
        for (int i = 0; i < buttonBackgrounds.Length; i++)
        {
            buttonTexts[i].Enable();
            buttonBackgrounds[i].GetComponent<Image>().color = normalBackgroundColor;
        }

        for (int i = 0; i < currentOption.paths.Length; i++)
        {
            Path currentPath = currentOption.paths[i];
            bool enabled = true;

            // Determine if choice will be choosable, or greyed out. Currently, choices can be enabled / disabled by prerequisite events and character attitudes
            if (currentPath.checkAttitude)
            {
                string attitudeToCheck = currentPath.characterToCheck.ToString() + "Attitude";

                if (currentPath.attitudeComparison.ToString() == "greaterThanOrEqual")
                {
                    if (PlayerPrefs.GetInt(attitudeToCheck) >= currentPath.attitudeToCompare)
                        enabled = (enabled && true);
                    else
                        enabled = (enabled && false);
                } 
                else if (currentPath.attitudeComparison.ToString() == "lessThanOrEqual")
                {
                    if (PlayerPrefs.GetInt(attitudeToCheck) <= currentPath.attitudeToCompare)
                        enabled = (enabled && true);
                    else
                        enabled = (enabled && false);
                }
                else
                {
                    Debug.LogError("Invalid comparator: " + currentPath.attitudeComparison.ToString());
                }
            } 
            if (currentPath.checkEvent)
            {
                Debug.Log("checking " + currentPath.eventToCheck.ToString());
                Debug.Log("value " + PlayerPrefs.GetInt(currentPath.eventToCheck.ToString()));

                // If player prefs had bools this would look a bit simpler. As is, 1 is on, 0 is off
                if (PlayerPrefs.GetInt(currentPath.eventToCheck.ToString()) == 1)
                    enabled = (enabled && true);
                else
                    enabled = (enabled && false);
            }
            if (option.paths[i].checkStress)
            {
                if (option.paths[i].stressComparison.ToString() == "greaterThanOrEqual")
                {
                    if (PlayerPrefs.GetInt("Stress") >= option.paths[i].stressCheckAmount)
                        enabled = (enabled && true);
                    else
                        enabled = (enabled && false);
                }
                else if (option.paths[i].stressComparison.ToString() == "lessThanOrEqual")
                {
                    if (PlayerPrefs.GetInt("Stress") <= option.paths[i].stressCheckAmount)
                        enabled = (enabled && true);
                    else
                        enabled = (enabled && false);
                }
                else
                {
                    Debug.LogError("Invalid comparator: " + option.paths[i].attitudeComparison.ToString());
                }
            }
            if (option.paths[i].checkNumAltContractsFinished)
            {
                if (PlayerPrefs.GetInt("OptionalCompleted") > option.paths[i].minimumRequired)
                    enabled = (enabled && true);
                else
                    enabled = (enabled && false);
            }

            buttonBackgrounds[i].gameObject.SetActive(true);

            if (!enabled)
            {
                buttonTexts[i].Disable();
                // Change sprite ideally, not just color. But maybe just color would work
                buttonBackgrounds[i].gameObject.GetComponent<Image>().color = shadedBackgroundColor;
            }


            // The only children of these buttons right now is their text. If this changes, make sure the text is the first child
            buttonBackgrounds[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentOption.paths[i].choiceText;
        }
    }

    public void ExecutePath(int index)
    {
        // Turn off lingering text
        DialogueUIManager.instance.dialogueTextObject.text = "";
        DialogueUIManager.instance.characterTextObject.text = "";

        Path choiceToExecute = currentOption.paths[index];

        if (choiceToExecute.changesAttitude)
        {
            // Attitude To Change is a charName enumerator
            string charToChange = choiceToExecute.attitudeToChange.ToString();

            PlayerPrefs.SetInt(charToChange + "Attitude", PlayerPrefs.GetInt(charToChange + "Attitude") + choiceToExecute.amountToAlterAttitude);

            if (PlayerPrefs.GetInt(charToChange + "Attitude") > 3)
                PlayerPrefs.SetInt(charToChange + "Attitude", 3);
            else if (PlayerPrefs.GetInt(charToChange + "Attitude") < -3)
                PlayerPrefs.SetInt(charToChange + "Attitude", -3);


        }

        if (choiceToExecute.logsEvent)
        {
            PlayerPrefs.SetInt(choiceToExecute.eventToLog.ToString(), 1);
        }

        if (choiceToExecute.changesStress) 
        {
            PlayerPrefs.SetInt("Stress", PlayerPrefs.GetInt("Stress") + choiceToExecute.stressChangeAmount);
        }


        foreach (GameObject button in buttonBackgrounds)
            button.SetActive(false);

        // Execute Consequences

        // Note that options must result in conversations right now. It should be possible to run an empty conversation if need be
        ConversationManager.instance.StartConversation(currentOption.possibleConversations[index]);
        // This should probably be in ConversationManager, or renamed
    }

}
