using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance = null;

    // Current scene could be referenced by other scripts to make sure they don't run while their parent is off
    // Current scene is a parent game object of a given scene
    private GameObject currentScene;
    private List<Character> activeCharacters;

    [SerializeField]
    Image fadeOutCover;

    public GameObject characterPool;
    // Right now, the first scene must be specified and is immediately loaded
    public SceneChange startingScene;

    private float fadeOutRate = .03f;
    private float fadeInRate = .03f;
    private float midFadeDelay = .7f;

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
        BeginSceneChange(startingScene);
        fadeOutCover.gameObject.SetActive(false);
    }

    public void BeginSceneChange(SceneChange sceneChange) {
        DialogueUIManager.instance.SetUpForSceneChange();
        StartCoroutine(ExecuteSceneChange(sceneChange));
    }

    public IEnumerator ExecuteSceneChange(SceneChange sceneChange)
    {
        yield return new WaitForSeconds(sceneChange.predelay);

        if (sceneChange.transitionStyle.ToString() == "fade")
        {
            fadeOutCover.color = new Color(0f, 0f, 0f, 0f);
            fadeOutCover.gameObject.SetActive(true);
            // Note- all parent scene objects must have a background named "Background"
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
        GameObject newScene = GameObject.Find(sceneChange.newScene.ToString());
        newScene.gameObject.SetActive(true);
        currentScene = newScene;

        // Add in new characters
        foreach (SceneChange.CharacterInstantiation characterInstantiation in sceneChange.characters)
        {
            Character toInstantiate = characterPool.transform.Find(characterInstantiation.character.ToString()).GetComponent<Character>();

            toInstantiate.transform.parent = newScene.transform;
            // Note this may change z value and result in weird rendering. 
            toInstantiate.transform.position = characterInstantiation.location;
            toInstantiate.gameObject.SetActive(true);
            Debug.Log(activeCharacters == null);
            activeCharacters.Add(toInstantiate);
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

        if (sceneChange.nextEvent != null)
        {
            if (sceneChange.nextEvent.GetComponent<Conversation>() != null)
                ConversationManager.instance.StartConversation(sceneChange.nextEvent.GetComponent<Conversation>());
            else if (sceneChange.nextEvent.GetComponent<Option>() != null)
                OptionManager.instance.PresentOption(sceneChange.nextEvent.GetComponent<Option>());
            else if (sceneChange.nextEvent.GetComponent<SceneChange>() != null)
                BeginSceneChange(sceneChange.nextEvent.GetComponent<SceneChange>());
            else
                Debug.LogError("Invalid next event");
        }

    }

}
