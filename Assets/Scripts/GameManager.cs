using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Day[] days;
    public int currentDayIndex;
    public Day currentDay;

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

        currentDay = days[currentDayIndex];
    }

    public Contract GetCurrentContract()
    {
        return currentDay.contracts[OfficeSceneManager.instance.currentContractIndex];
    }

    public void NextDay()
    {
        OfficeSceneManager.instance.currentContractIndex = 0;

        currentDayIndex++;
        currentDay = days[currentDayIndex];
    }

    public void StartSequence(ScriptableObject toExecute)
    {
        if (toExecute.GetType().ToString() == "Conversation")
        {
            ConversationManager.instance.StartConversation((Conversation)toExecute);
        }

        else if (toExecute.GetType().ToString() == "Option")
        {
            OptionManager.instance.PresentOption((Option)toExecute);
        }

        else if (toExecute.GetType().ToString() == "SceneChange")
        {
            SceneChangeManager.instance.StartSceneChange((SceneChange)toExecute);
        }

        else if (toExecute.GetType().ToString() == "AnimationMoment")
        {
            AnimationManager.instance.StartAnimationMoment((AnimationMoment)toExecute);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
