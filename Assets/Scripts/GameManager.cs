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

    public EscapeMenu escapeMenu;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        // De-comment on build
        //ResetPlayerPrefsToSave();  

        // Delete:
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {

        // Should be an if statement to allow for save loading (ideally would just specify the day you're on
        currentDayIndex = PlayerPrefs.GetInt("CurrentDayIndex");
        sequenceActive = false;

        currentDay = days[currentDayIndex];
        currentDay.ShuffleContracts();

        /*
         * NOTE ABOUT SAVING: player pref automatically saves on quit, which should not be the case
         * Possible options going forward are:
         *   1. Make a set of variables in the player prefs that represent changes made that day, add their strings to a list (one list 
         *      for each type), then loop through the list and revert the changes (can be done on load so as to avoid weird force quit issues)
         *   2. ???
         */

        // Load save info
        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));

        if (PlayerPrefs.GetInt("ActiveGame") != 1)
        {
            PlayerPrefs.SetFloat("MusicVolume", MusicManager.instance.defaultMusicVolume);
            PlayerPrefs.SetFloat("SFXVolume", MusicManager.instance.defaultSFXVolume);
        }

        MusicManager.instance.audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        AmbienceManager.instance.audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");

        SceneChangeManager.instance.StartSceneChange(SceneChangeManager.instance.startingScene);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Escape menu won't trigger while looking at title scene or if a dialogue sequence is active
            if (SceneChangeManager.instance.currentScene.name != "TitleScene" && !sequenceActive && !escapeMenu.gameObject.activeSelf && TutorialManager.instance.activeTutorial == null)
            {
                optionsMenuActive = true;
                escapeMenu.OpenMainMenu();
                escapeMenu.gameObject.SetActive(true);
            }

        }

    }

    public void StartNewGame()
    {
        TitleSceneManager.instance.ToggleButtonInteractability(false);

        float musicVol = PlayerPrefs.GetFloat("MusicVolume");
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume");

        PlayerPrefs.DeleteAll();

        // Saves settings from game to game
        PlayerPrefs.SetFloat("MusicVolume", musicVol);
        PlayerPrefs.SetFloat("SFXVolume", sfxVol);

        PlayerPrefs.SetInt("Stress", 50);

        PlayerPrefs.SetFloat("TotalMoney", 0);

        PlayerPrefs.SetInt("OptionalCompleted", 0);

        PlayerPrefs.SetInt("LastMusic1", 2);
        PlayerPrefs.SetInt("LastMusic2", 3);
        PlayerPrefs.SetInt("LastMusic3", 4);

        PlayerPrefs.SetInt("CurrentDayIndex", 0);
        currentDayIndex = 0;

        foreach (CharacterName character in System.Enum.GetValues(typeof(CharacterName)))
        {
            // Load save state here
            PlayerPrefs.SetInt(character.ToString() + "Attitude", 3);
        }

        PlayerPrefs.SetInt("DrugID", 100);

        PlayerPrefs.SetInt("ActiveGame", 1);

        UpdatePlayerPrefs();

        SceneChangeManager.instance.StartSceneChange(TitleSceneManager.instance.titleToMR);

        MusicManager.instance.StartFadeOut();

    }

    public void ResumeGame()
    {
        TitleSceneManager.instance.ToggleButtonInteractability(false);
        SceneChangeManager.instance.StartSceneChange(TitleSceneManager.instance.titleToMR);

        MusicManager.instance.StartFadeOut();

        // Load save information
    }

    public Contract GetCurrentContract()
    {
        return currentDay.contracts[OfficeSceneManager.instance.currentContractIndex];
    }

    public Day GetCurrentDay()
    {
        return currentDay;
    }

    public void NextDay()
    {
        OfficeSceneManager.instance.currentContractIndex = 0;

        UpdatePlayerPrefs();

        ElizabethHitbox.instance.timesClickedToday = 0;

        DrugManager.instance.ResetTimeElapsed();
        OfficeSceneManager.instance.contractsSolved = 0;

        OfficeSceneManager.instance.openedComputerToday = false;
        DrugManager.instance.tutorialsfinished = false;
        DrugManager.instance.numtutorialsfinished = 0;

        currentDayIndex++;
        PlayerPrefs.SetInt("CurrentDayIndex", PlayerPrefs.GetInt("CurrentDayIndex") + 1);
        currentDay = days[currentDayIndex];
        currentDay.ShuffleContracts();

        PlayerPrefs.Save();

        MorningRoutineManager.Instance.StartNewDay();
    }

    public void ResetPlayerPrefsToSave()
    {

        foreach (SaveableEvent se in Enum.GetValues(typeof(SaveableEvent)))
        {
            string s = se.ToString();

            if (PlayerPrefs.HasKey(s + "ResetValue"))
            {
                PlayerPrefs.SetInt(s, PlayerPrefs.GetInt(s + "ResetValue"));
                PlayerPrefs.DeleteKey(s + "ResetValue");
            }
        }

        foreach (CharacterName cn in Enum.GetValues(typeof(CharacterName)))
        {
            string s = cn.ToString();


            if (PlayerPrefs.HasKey(s + "AttitudeResetValue"))
            {
                PlayerPrefs.SetInt(s + "Attitude", PlayerPrefs.GetInt(s + "AttitudeResetValue"));
                PlayerPrefs.DeleteKey(s + "AttitudeResetValue");
            }
        }

        if (PlayerPrefs.HasKey("StressResetValue"))
        {
            PlayerPrefs.SetInt("Stress", PlayerPrefs.GetInt("StressResetValue"));
            PlayerPrefs.DeleteKey("StressResetValue");
        }

        if (PlayerPrefs.HasKey("TotalMoneyResetValue"))
        {
            PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoneyResetValue"));
            PlayerPrefs.DeleteKey("TotalMoneyResetValue");
        }

        if (PlayerPrefs.HasKey("OptionalCompletedResetValue"))
        {
            PlayerPrefs.SetInt("OptionalCompleted", PlayerPrefs.GetInt("OptionalCompletedResetValue"));
            PlayerPrefs.DeleteKey("OptionalCompletedResetValue");
        }

        if (PlayerPrefs.HasKey("DrugIDResetValue"))
        {
            PlayerPrefs.SetInt("DrugID", PlayerPrefs.GetInt("DrugIDResetValue"));
            PlayerPrefs.DeleteKey("DrugIDResetValue");
        }

        PlayerPrefs.Save();

    }

    private void UpdatePlayerPrefs()
    {
        // Day-to-day resetters
        PlayerPrefs.SetInt("TookPill", 0);
        PlayerPrefs.SetInt("WatchedNews", 0);
        PlayerPrefs.SetInt("IsLate", 0);
        PlayerPrefs.SetInt("WateredPlants", 0);

        // In short this bit of code checks if any variables were changed but not properly saved (i.e. if the player quit mid-day). It goes back and resets them to their original
        //   values, which should be saved whenever PlayerPrefs is altered
        foreach (SaveableEvent se in Enum.GetValues(typeof(SaveableEvent)))
        {
            string s = se.ToString();
            if (PlayerPrefs.HasKey(s))
                PlayerPrefs.SetInt(s + "ResetValue", PlayerPrefs.GetInt(s));
            else
                PlayerPrefs.SetInt(s + "ResetValue", 0);
        }

        foreach (CharacterName cn in Enum.GetValues(typeof(CharacterName)))
        {
            string s = cn.ToString();

            PlayerPrefs.SetInt(s + "AttitudeResetValue", PlayerPrefs.GetInt(s + "Attitude"));
        }

        PlayerPrefs.SetInt("StressResetValue", PlayerPrefs.GetInt("Stress"));
        PlayerPrefs.SetInt("TotalMoneyResetValue", PlayerPrefs.GetInt("TotalMoney"));
        PlayerPrefs.SetInt("OptionalCompletedResetValue", PlayerPrefs.GetInt("OptionalCompleted"));
        PlayerPrefs.SetInt("DrugIDResetValue", PlayerPrefs.GetInt("DrugID"));

        PlayerPrefs.Save();
    }

    public void StartSequence(ScriptableObject toExecute)
    {
        if (toExecute == null)
        {
            ConversationManager.instance.EndConversation();

            if (SceneChangeManager.instance.currentScene.name == "OfficeScene" && CharacterFadeManager.instance.currentChars.ContainsKey("Barney"))
            {
                CharacterFadeManager.instance.currentChars["Barney"].GetComponent<Animator>().Play("BarneyWorkingNormal");
            }

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

        else if (toExecute.GetType().ToString() == "TutorialEvent")
        {
            if (SceneChangeManager.instance.currentScene.name == "OfficeScene" && CharacterFadeManager.instance.currentChars.ContainsKey("Barney"))
                CharacterFadeManager.instance.currentChars["Barney"].GetComponent<Animator>().Play("BarneyWorkingNormal");

            TutorialManager.instance.ActivateTutorial(TutorialManager.instance.ContractTutorial1);
            sequenceActive = false;
        } 

        else if (toExecute.GetType().ToString() == "MigraineEvent") {
            Camera.main.GetComponent<MigraineController>().StartMigraine((MigraineEvent)toExecute);
        }

        else if (toExecute.GetType().ToString() == "DreamEvent")
        {
            DreamEventManager.instance.StartDreamEvent((DreamEvent)toExecute);
        }

        else if (toExecute.GetType().ToString() == "WaitEvent")
        {
            WaitManager.instance.StartWaitEvent((WaitEvent)toExecute);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }



    public enum SaveableEvent {
        // Ignore "not" events
        TookPill,
        WatchedNews,
        WateredPlants,
        NotWateredPlants,
        FinishedOnTime,
        FinishedLate,
        NotTookPill,
        HelpedRobbie,
        IsLate,
        SpecialContract1,
        SpecialContract2,
        AcceptedRobbiePills,
        RobbieGaveMomPills,
        TookPillFirstTime,
        BigReveal
    }

}
