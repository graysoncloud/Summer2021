using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFadeManager : MonoBehaviour
{
    public static CharacterFadeManager instance = null;

    // BIG NOTE- must match the order of the characterName enum
    public Character[] characterPrefabs;
    public int numActiveFades;

    public Dictionary<string, Character> currentChars;

    public GameObject ElizabethLocation;

    private CharacterFade currentFadeObject;

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
        numActiveFades = 0;

        // Might need to add default barney to this at the beginning
        currentChars = new Dictionary<string, Character>();
    }

    public void StartCharacterFade(CharacterFade fadeObject)
    {
        GameManager.instance.sequenceActive = true;

        DialogueUIManager.instance.SetUpForCharacterFade();

        currentFadeObject = fadeObject;

        foreach (CharacterFade.Fade fadeEvent in fadeObject.characterFades)
        {
            StartCoroutine(ExecuteCharacterFade(fadeEvent));
        }
    }

    public void StartInstantFade(CharacterFade fadeObject)
    {
        currentFadeObject = fadeObject;

        foreach (CharacterFade.Fade fadeEvent in fadeObject.characterFades)
        {
            if (fadeEvent.fadeIn)
            {                
                Character charToFade = Instantiate(characterPrefabs[(int)fadeEvent.characterToFade - 1], SceneChangeManager.instance.currentScene.transform);
                if (charToFade.name.Contains("Elizabeth"))
                {
                    charToFade.transform.parent = ElizabethLocation.transform;
                }

                Debug.Log(charToFade.transform.position.x);
                charToFade.transform.localPosition = charToFade.startLocation;
                Debug.Log(charToFade.transform.position.x);

                currentChars.Add(fadeEvent.characterToFade.ToString(), charToFade);
            }

            else
            {

                Character charToFade = currentChars[fadeEvent.characterToFade.ToString()];

                currentChars.Remove(fadeEvent.characterToFade.ToString());
                GameObject.Destroy(charToFade.gameObject);

            }
        }

            GameManager.instance.StartSequence(currentFadeObject.nextEvent);
    }


    public IEnumerator ExecuteCharacterFade(CharacterFade.Fade fadeEvent)
    {
        Debug.Log("Executing");

        numActiveFades++;

        Color fadeIncrement = new Color(.01f, .01f, .01f, .01f);

        // Fade the character in
        if (fadeEvent.fadeIn)
        {
            // Relies on prefab having an alpha of 0 and being set to black (0, 0, 0, 0);
            Character charToFade = Instantiate(characterPrefabs[(int)fadeEvent.characterToFade - 1], SceneChangeManager.instance.currentScene.transform);
            charToFade.transform.position = charToFade.startLocation;

            // Play SFX
            foreach (CharacterFadeSFX sfx in fadeEvent.SFX)
            {
                CharacterFadeSFXManager.instance.PlaySFX(sfx);
            }

            SpriteRenderer spriteRenderer = charToFade.GetComponent<SpriteRenderer>();

            currentChars.Add(fadeEvent.characterToFade.ToString(), charToFade);

            float fadeRate = .01f;

            // Move and fade in
            while (charToFade.GetComponent<SpriteRenderer>().color.a < 1)
            {
                charToFade.transform.position -= new Vector3(.02f, 0f, 0f);
                spriteRenderer.color += fadeIncrement;
                yield return new WaitForSeconds(fadeRate);
            }

        }
        // Fade the character out
        else
        {
            // Character prefabs MUST be named exactly how they appear in the CharacterNames enum
            Character charToFade = currentChars[fadeEvent.characterToFade.ToString()];
            SpriteRenderer spriteRenderer = charToFade.GetComponent<SpriteRenderer>();

            float fadeRate = .01f;

            while (charToFade.GetComponent<SpriteRenderer>().color.a > 0)
            {
                charToFade.transform.position += new Vector3(.02f, 0f, 0f);
                spriteRenderer.color += (fadeIncrement * -1);
                yield return new WaitForSeconds(fadeRate);
            }

            currentChars.Remove(fadeEvent.characterToFade.ToString());
            GameObject.Destroy(charToFade.gameObject);

        }

        numActiveFades--;

        if (numActiveFades == 0)
        {
            GameManager.instance.StartSequence(currentFadeObject.nextEvent);
        }

    }



}
