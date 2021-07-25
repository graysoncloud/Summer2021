using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;
    
    // Must be in the exact order of the Song enum
    public AudioClip[] songs;

    public AudioSource audioSource;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    // Unlike other managers, this isn't a coroutine since it should happen instantly and not delay the game
    public void StartMusicEvent(MusicEvent toExecute)
    {
        // Makes sure nothing gets messed up if fade in / out happens at the same time
        StopAllCoroutines();

        if (toExecute.fadeOut)
        {
            StartCoroutine("FadeOutMusic");
            GameManager.instance.StartSequence(toExecute.nextEvent);
            return;
        }

        audioSource.clip = songs[(int)toExecute.toPlay];
        StartCoroutine("FadeInMusic");
        GameManager.instance.StartSequence(toExecute.nextEvent);

    }

    public void StartFadeOut()
    {
        StartCoroutine("FadeOutMusic");
    }

    private IEnumerator FadeInMusic()
    {
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < PlayerPrefs.GetFloat("MusicVolume"))
        {
            audioSource.volume += (float)(.001 * PlayerPrefs.GetFloat("MusicVolume"));
            yield return new WaitForSeconds(.005f);
        }


    }

    private IEnumerator FadeOutMusic()
    {

        while (audioSource.volume > 0)
        {
            audioSource.volume -= (float)(.001 * PlayerPrefs.GetFloat("MusicVolume"));
            yield return new WaitForSeconds(.005f);

            // Allows fade out to be more gradual
            if (audioSource.volume < (.1 * PlayerPrefs.GetFloat("MusicVolume")))
                yield return new WaitForSeconds(.02f);
        }

        audioSource.Stop();

    }

}
