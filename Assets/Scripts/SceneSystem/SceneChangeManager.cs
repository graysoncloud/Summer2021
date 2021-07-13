using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance = null;

    // Current scene could be referenced by other scripts to make sure they don't run while their parent is off
    // Current scene is a parent game object of a given scene

    public GameObject currentScene;
    private List<Character> activeCharacters;

    [SerializeField]
    Image fadeOutCover;

    public GameObject characterPool;
    // Not used
    public SceneChange startingScene;

    private float fadeOutRate = .01f;
    private float fadeInRate = .01f;
    private float midFadeDelay = .7f;

    public GameObject[] scenes;

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
        activeCharacters = new List<Character>();
        StartSceneChange(startingScene);
        fadeOutCover.gameObject.SetActive(false);
    }

    public void StartSceneChange(SceneChange sceneChange) {
        DialogueUIManager.instance.SetUpForSceneChange();
        StartCoroutine(ExecuteSceneChange(sceneChange));
    }

    private IEnumerator ExecuteSceneChange(SceneChange sceneChange)
    {
        // Old scene name lets us track which scene we're transitioning from, allowing us to trigger certain events
        string oldSceneName;
        if (currentScene != null)
            oldSceneName = currentScene.gameObject.name;
        else
            oldSceneName = null;

        yield return new WaitForSeconds(sceneChange.predelay);

        if (sceneChange.transitionStyle.ToString() == "fade")
        {
            fadeOutCover.color = new Color(0f, 0f, 0f, 0f);
            fadeOutCover.gameObject.SetActive(true);
            while (fadeOutCover.color.a < 1)
            {
                fadeOutCover.color += new Color(0f, 0f, 0f, .01f);
                yield return new WaitForSeconds(fadeOutRate);
            }
        }

        if (currentScene != null)
            currentScene.SetActive(false);

        // Remove old characters
        if (activeCharacters != null) {
            foreach (Character character in activeCharacters)
            {
                character.transform.parent = characterPool.transform;
                character.gameObject.SetActive(false);
                // Something to reset the characters animation / anything else?
            }
        }
        activeCharacters = new List<Character>();

        // Turn on new scene and assign it to scene manager's current scene field
        GameObject newScene = null;

        // Strings must match whatevers in the newScene sceneName enumerator, and the scene array must be indexed properly
        switch(sceneChange.newScene.ToString())
        {
            case "MorningRoutineScene": newScene = scenes[0]; break;
            case "OfficeScene": newScene = scenes[1]; break;
            case "DrugGameScene": newScene = scenes[2]; break;
            case "RecapScene": newScene = scenes[3]; break;
            case "DreamScene": newScene = scenes[4]; break;
            default: Debug.LogError("Invalid sceneName: " + sceneChange.newScene.ToString()); break;
        }

        newScene.gameObject.SetActive(true);
        currentScene = newScene;

        // Add in new characters
        foreach (SceneChange.CharacterInstantiation characterInstantiation in sceneChange.characters)
        {
            Character charToInstantiate = characterPool.transform.Find(characterInstantiation.character.ToString()).GetComponent<Character>();

            charToInstantiate.transform.parent = newScene.transform;
            // Note this may change z value and result in weird rendering. 
            charToInstantiate.transform.position = characterInstantiation.location;
            charToInstantiate.gameObject.SetActive(true);
            charToInstantiate.GetComponent<Animator>().Play(characterInstantiation.animation.ToString());
            activeCharacters.Add(charToInstantiate);
        }

        yield return new WaitForSeconds(midFadeDelay);

        if (sceneChange.transitionStyle.ToString() == "fade")
        {
            while (fadeOutCover.color.a > 0)
            {
                fadeOutCover.color -= new Color(0f, 0f, 0f, .01f);
                yield return new WaitForSeconds(fadeInRate);
            }
            fadeOutCover.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(sceneChange.postdelay);

        if (sceneChange.nextEvent == null)
            ConversationManager.instance.EndConversation();
        else
            GameManager.instance.StartSequence(sceneChange.nextEvent);

        if(oldSceneName == "MorningRoutineScene" && currentScene.gameObject.name == "OfficeScene")
        {
            foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
            {
                if (sequence.trigger.ToString() == "arrivingAtWork")
                {
                    GameManager.instance.StartSequence(sequence.initialEvent);
                }
            }
        }

    }

}
