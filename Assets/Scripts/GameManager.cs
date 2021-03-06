using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Day[] days;
    public int currentDayIndex;
    public Day currentDay;

    public bool sequenceActive;
    public bool optionsMenuActive;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        // Should be an if statement to allow for save loading (ideally would just specify the day you're on
        currentDayIndex = 0;
        sequenceActive = false;

        currentDay = days[currentDayIndex];
        currentDay.ShuffleContracts();

        PlayerPrefs.DeleteAll();

        /*
         * NOTE ABOUT SAVING: player pref automatically saves on quit, which should not be the case
         * Possible options going forward are:
         *   1. Make a set of variables in the player prefs that represent changes made that day, add their strings to a list (one list 
         *      for each type), then loop through the list and revert the changes (can be done on load so as to avoid weird force quit issues)
         *   2. ???
         */

        // Load save info
        PlayerPrefs.SetFloat("MusicVolume", .5f);

        PlayerPrefs.SetInt("Stress", 50);

        foreach (CharacterName character in Enum.GetValues(typeof(CharacterName)))
        {
            // Load save state here
            PlayerPrefs.SetInt(character.ToString() + "Attitude", 150);
        }

    }

    public Contract GetCurrentContract()
    {
        return currentDay.contracts[OfficeSceneManager.instance.currentContractIndex];
    }

    public void NextDay()
    {
        OfficeSceneManager.instance.currentContractIndex = 0;

        PlayerPrefs.SetInt("TookPills", 0);
        PlayerPrefs.SetInt("WatchedNews", 0);

        currentDayIndex++;
        currentDay = days[currentDayIndex];
        currentDay.ShuffleContracts();
    }

    public void StartSequence(ScriptableObject toExecute)
    {
        if (toExecute == null)
        {
            ConversationManager.instance.EndConversation();
            GameManager.instance.sequenceActive = false;
        }

        else if (toExecute.GetType().ToString() == "Conversation")
            ConversationManager.instance.StartConversation((Conversation)toExecute);

        else if (toExecute.GetType().ToString() == "Option")
            OptionManager.instance.StartOption((Option)toExecute);

        else if (toExecute.GetType().ToString() == "SceneChange")
            SceneChangeManager.instance.StartSceneChange((SceneChange)toExecute);

        else if (toExecute.GetType().ToString() == "AnimationMoment")
            AnimationManager.instance.StartAnimationMoment((AnimationMoment)toExecute);

        else if (toExecute.GetType().ToString() == "CharacterFade")
            CharacterFadeManager.instance.StartCharacterFade((CharacterFade)toExecute);

        else if (toExecute.GetType().ToString() == "MusicEvent")
            MusicManager.instance.StartMusicEvent((MusicEvent)toExecute);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public enum SaveableEvent {
        TookPill,
        WatchedNews,


    }

}
