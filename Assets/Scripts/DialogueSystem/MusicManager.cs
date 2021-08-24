using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;

    public float defaultMusicVolume = .5f;
    public float defaultSFXVolume = .5f;

    // Must be in the exact order of the Song enum
    public AudioClip[] songs;
    public AudioClip[] mrSongs;
    public AudioClip[] workSongs;

    public AudioSource audioSource;

    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    private Coroutine volumeChangeCoroutine;

    public Coroutine backgroundMusicPlayerInstance;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void StartBackgroundPlayer()
    {
        if (backgroundMusicPlayerInstance != null)
            return;

        backgroundMusicPlayerInstance = StartCoroutine("BackgroundMusicPlayer");
    }

    public IEnumerator BackgroundMusicPlayer()
    {
        // Initial delay
        //yield return new WaitForSeconds(Random.value * 10);


        while (SceneChangeManager.instance.currentScene.name != "RecapScene")
        {
            if (!audioSource.isPlaying && SceneChangeManager.instance.currentScene.name != "OfficeScene")
            {
                yield return new WaitForSeconds((Random.value * 5) + 5);

                // Double checks after delay, preventing lingering music
                if (!audioSource.isPlaying && SceneChangeManager.instance.currentScene.name != "RecapScene")
                {
                    int rInt = Random.Range(0, workSongs.Length);
                    audioSource.clip = workSongs[rInt];

                    audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                    audioSource.PlayOneShot(audioSource.clip);
                }

            }

            yield return new WaitForSeconds(1f);

        }

    }

    // Unlike other managers, this isn't a coroutine since it should happen instantly and not delay the game
    public void StartMusicEvent(MusicEvent toExecute)
    {

        if (toExecute.fadeOut)
        {
            fadeOutCoroutine = StartCoroutine("FadeOutMusic");
            GameManager.instance.StartSequence(toExecute.nextEvent);
            return;
        } 
        else
        {
            audioSource.clip = songs[(int)toExecute.toPlay];
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.PlayOneShot(audioSource.clip); GameManager.instance.StartSequence(toExecute.nextEvent);
        }
    }

    public void StartMRMusic()
    {
        StartCoroutine("ExecuteMRMusic");
    }

    IEnumerator ExecuteMRMusic()
    {
        yield return new WaitForSeconds(.5f);

        int index = Random.Range(0, 2);

        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        audioSource.clip = mrSongs[index];
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void StartFadeOut()
    {
        fadeOutCoroutine = StartCoroutine("FadeOutMusic");
    }

    private IEnumerator FadeOutMusic()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            Debug.Log("Fade in ended");
        }

        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");

        Debug.Log(audioSource.volume);

        while (audioSource.volume > 0)
        {

            audioSource.volume -= (float)(.004 * PlayerPrefs.GetFloat("MusicVolume"));
            yield return new WaitForSeconds(.005f);

            // Allows fade out to be more gradual
            if (audioSource.volume < (.1 * PlayerPrefs.GetFloat("MusicVolume")))
                yield return new WaitForSeconds(.022f);

            if (audioSource.volume < (.02 * PlayerPrefs.GetFloat("MusicVolume")))
                yield return new WaitForSeconds(.04f);
        }

        audioSource.Stop();

    }

    public void AttemptMusicVolumeChange()
    {
        if (volumeChangeCoroutine == null)
            volumeChangeCoroutine = StartCoroutine("ChangeVolume");
    }

    IEnumerator ChangeVolume()
    {
        while (fadeOutCoroutine != null)
            yield return new WaitForEndOfFrame();

        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    }


    //private IEnumerator FadeInMusic()
    //{
    //    if (fadeOutCoroutine != null)
    //    {
    //        StopCoroutine(fadeOutCoroutine);
    //    }

    //    audioSource.volume = 0f;

    //    audioSource.volume = 0;
    //    audioSource.Play();

    //    while (audioSource.volume < PlayerPrefs.GetFloat("MusicVolume"))
    //    {
    //        audioSource.volume += (float)(.001 * PlayerPrefs.GetFloat("MusicVolume"));
    //        yield return new WaitForSeconds(.005f);
    //    }


    //}

}
