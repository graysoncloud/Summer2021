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

    public void NextDay()
    {
        currentDayIndex++;
        currentDay = days[currentDayIndex];
    }

    public void StartSequence(GameObject toExecute)
    {
        if (toExecute.GetComponent<Conversation>() != null)
        {
            ConversationManager.instance.StartConversation(toExecute.GetComponent<Conversation>());
        }

        else if (toExecute.GetComponent<Option>() != null)
        {
            OptionManager.instance.PresentOption(toExecute.GetComponent<Option>());
        }

        else if (toExecute.GetComponent<SceneChange>() != null)
        {
            SceneChangeManager.instance.StartSceneChange(toExecute.GetComponent<SceneChange>());
        }

        else if (toExecute.GetComponent<AnimationClip>() != null)
        {
            AnimationManager.instance.StartAnimationMoment(toExecute.GetComponent<AnimationMoment>());
        }

    }

}
