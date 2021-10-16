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
    //private List<Character> activeCharacters;

    [SerializeField]
    Image fadeOutCover;

    //public GameObject characterPool;
    // Not used
    public SceneChange startingScene;

    private float fadeOutRate = .01f;
    private float fadeInRate = .01f;
    private float midFadeDelay = .7f;

    public GameObject[] scenes;

    public CharacterFade instantBarneyFadeIn;
    public CharacterFade instantBarneyFadeOut;
    public CharacterFade instantElizabethFadeIn;
    public CharacterFade instantElizabethFadeOut;

    public GameObject bedroom;


    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
    }

    private void Start()
    {
        //activeCharacters = new List<Character>();
        fadeOutCover.gameObject.SetActive(false);
    }

    public void StartSceneChange(SceneChange sceneChange) 
    {
        GameManager.instance.sequenceActive = true;

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

        // Default fade outs:
        if (oldSceneName == "MorningRoutineScene" && sceneChange.newScene.ToString() == "OfficeScene")
        {
            MusicManager.instance.StartFadeOut();
        }
        else if (oldSceneName == "OfficeScene" && sceneChange.newScene.ToString() == "RecapScene")
        {
            MusicManager.instance.StartFadeOut();
        }

        if (oldSceneName == "RecapScene")
        {
            AmbienceManager.instance.StartFadeOut();
        }

        yield return new WaitForSeconds(sceneChange.preDelay);

        if (sceneChange.transitionStyle.ToString() == "fade" || sceneChange.transitionStyle.ToString() == "longFade")
        {
            fadeOutCover.color = new Color(0f, 0f, 0f, 0f);
            fadeOutCover.gameObject.SetActive(true);
            while (fadeOutCover.color.a < 1)
            {
                fadeOutCover.color += new Color(0f, 0f, 0f, .01f) * Time.deltaTime * 60;
                yield return new WaitForEndOfFrame();

                // Doubles fade length for long fade
                if (sceneChange.transitionStyle.ToString() == "longFade")
                    yield return new WaitForEndOfFrame();
            }

            MusicManager.instance.audioSource.Stop();

            if (MusicManager.instance.fadeOutCoroutine != null)
            {
                StopCoroutine(MusicManager.instance.fadeOutCoroutine);
                MusicManager.instance.fadeOutCoroutine = null;
            }
        }


        if (currentScene != null)
        {
            if (currentScene.name == "OfficeScene" && sceneChange.newScene.ToString() == "RecapScene")
            {
                CharacterFadeManager.instance.StartInstantFade(instantBarneyFadeOut);
            }

            if (currentScene.name == "MorningRoutineScene")
            {
                CharacterFadeManager.instance.StartInstantFade(instantElizabethFadeOut);
            }
        }

        GameObject oldScene = currentScene;

        yield return new WaitForSeconds(sceneChange.midDelay);

        /*
         * Depricated: characters instantiated in their own scriptable object
         */
        //// Remove old characters
        //if (activeCharacters != null) {
        //    foreach (Character character in activeCharacters)
        //    {
        //        character.transform.parent = characterPool.transform;
        //        character.gameObject.SetActive(false);
        //        // Something to reset the characters animation / anything else?
        //    }
        //}
        //activeCharacters = new List<Character>();

        // Turn on new scene and assign it to scene manager's current scene field
        GameObject newScene = null;

        // Strings must match whatevers in the newScene sceneName enumerator, and the scene array must be indexed properly
        switch (sceneChange.newScene.ToString())
        {
            // Insert things like MRManager.ResetScene() below
            case "MorningRoutineScene": newScene = scenes[0]; currentScene = newScene; CharacterFadeManager.instance.StartInstantFade(instantElizabethFadeIn);
                newScene.gameObject.SetActive(true); MusicManager.instance.StartMRMusic(); GameObject.FindObjectOfType<PlayerController>().SetRoom(bedroom.GetComponent<Room>());
                DreamEventManager.instance.ResetDreamScene(); break;
            case "OfficeScene": newScene = scenes[1]; currentScene = newScene; newScene.gameObject.SetActive(true); AmbienceManager.instance.PlayAmbience(0);
                if (oldScene.name == "MorningRoutineScene" && !CharacterFadeManager.instance.currentChars.ContainsKey("Barney")) CharacterFadeManager.instance.StartInstantFade(instantBarneyFadeIn); 
                break;
            case "DrugGameScene": newScene = scenes[2]; currentScene = newScene; newScene.gameObject.SetActive(true); VolatilityBar.instance.Refresh(); AmbienceManager.instance.StartFadeOut(); break;
            case "RecapScene": newScene = scenes[3]; currentScene = newScene; newScene.gameObject.SetActive(true); RecapSceneManager.instance.DisplayContracts();
                AmbienceManager.instance.StartFadeOut(); AmbienceManager.instance.PlayAmbience(1);  break;
            case "DreamScene": newScene = scenes[4]; currentScene = newScene; newScene.gameObject.SetActive(true); DreamPropController.instance.BeginPropCoroutine(); break;
            case "TitleScene": newScene = scenes[5]; currentScene = newScene; newScene.gameObject.SetActive(true); newScene.GetComponent<TitleSceneManager>().PrepareScene();
                MusicManager.instance.StartMusicEvent(TitleSceneManager.instance.defaultTitleMusicEvent); break;
            case "OSOverlay": newScene = scenes[6]; currentScene = newScene; newScene.gameObject.SetActive(true); OSOverlay.instance.StartOSDisplay();
                AmbienceManager.instance.StartFadeOut(); break;
            case "OpeningScene": newScene = scenes[7]; currentScene = newScene; newScene.gameObject.SetActive(true); break;
            case "CreditsScene": newScene = scenes[8]; currentScene = newScene; newScene.gameObject.SetActive(true); break;
            case "OverBlackScene": newScene = scenes[9]; currentScene = newScene; newScene.gameObject.SetActive(true); break;
            default: Debug.LogError("Invalid sceneName: " + sceneChange.newScene.ToString()); break;

                //if (MusicManager.instance.backgroundMusicPlayerInstance != null) MusicManager.instance.StopCoroutine(MusicManager.instance.backgroundMusicPlayerInstance);
                //MusicManager.instance.backgroundMusicPlayerInstance = null;
        }

        if (oldScene != null)
            oldScene.SetActive(false);

        if (sceneChange.increaseDay)
            GameManager.instance.NextDay();

        // Shouldn't be neccessary but it fixes a bug
        GameManager.instance.sequenceActive = true;

        /*
         * Depricated, characters now added in by there own scriptable object
         */
        // Add in new characters
        //foreach (SceneChange.CharacterInstantiation characterInstantiation in sceneChange.characters)
        //{
        //    Character charToInstantiate = characterPool.transform.Find(characterInstantiation.character.ToString()).GetComponent<Character>();

        //    charToInstantiate.transform.parent = newScene.transform;
        //    // Note this may change z value and result in weird rendering. 
        //    charToInstantiate.transform.position = characterInstantiation.location;
        //    charToInstantiate.gameObject.SetActive(true);
        //    charToInstantiate.GetComponent<Animator>().Play(characterInstantiation.animation.ToString());
        //    activeCharacters.Add(charToInstantiate);
        //}

        yield return new WaitForSeconds(midFadeDelay);

        if (sceneChange.transitionStyle.ToString() == "fade" || sceneChange.transitionStyle.ToString() == "longFade")
        {
            while (fadeOutCover.color.a > 0)
            {
                fadeOutCover.color -= new Color(0f, 0f, 0f, .01f) * Time.deltaTime * 60;
                yield return new WaitForEndOfFrame();

                // Doubles fade length for long fade
                //if (sceneChange.transitionStyle.ToString() == "longFade")
                //    yield return new WaitForEndOfFrame();
            }
            fadeOutCover.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(sceneChange.postDelay);
        yield return new WaitForSeconds(sceneChange.postDelay);

        // Look for any triggered sequences
        bool triggeredSequence = false;
        if(oldSceneName == "MorningRoutineScene" && currentScene.gameObject.name == "OfficeScene")
        {
            foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
            {
                if (sequence.trigger.ToString() == "arrivingAtWork")
                {
                    triggeredSequence = true;
                    GameManager.instance.StartSequence(sequence.initialEvent);
                }
            }
        }

        if (!triggeredSequence)
            GameManager.instance.StartSequence(sceneChange.nextEvent);


    }


    public bool IsFading() {
        return fadeOutCover.IsActive();
    }
}
