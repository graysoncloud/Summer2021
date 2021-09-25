using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;

    public float defaultMusicVolume = .8f;
    public float defaultSFXVolume = .8f;

    // Must be in the exact order of the Song enum
    public AudioClip[] songs;
    public AudioClip[] mrSongs;
    public AudioClip[] workSongs;

    public AudioSource audioSource;

    private Coroutine fadeInCoroutine = null;
    public Coroutine fadeOutCoroutine = null;
    private Coroutine volumeChangeCoroutine = null;

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
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            bool firstPlay = true;

            if (!audioSource.isPlaying && SceneChangeManager.instance.currentScene.name != "OfficeScene")
            {
                if (firstPlay)
                {
                    yield return new WaitForSeconds(2f);
                    firstPlay = false;
                } else
                {
                    yield return new WaitForSeconds(12f);
                }

                // Double checks after delay, preventing lingering music
                if (!audioSource.isPlaying && SceneChangeManager.instance.currentScene.name != "RecapScene")
                {

                    int newIndex = -1;
                    while (newIndex == PlayerPrefs.GetInt("LastMusic1") || newIndex == PlayerPrefs.GetInt("LastMusic2") || newIndex == PlayerPrefs.GetInt("LastMusic3") || newIndex < 0)
                    {
                        int rInt = Random.Range(0, workSongs.Length - 1);
                        newIndex = rInt;

                    }


                    PlayerPrefs.SetInt("LastMusic3", PlayerPrefs.GetInt("LastMusic2"));
                    PlayerPrefs.SetInt("LastMusic2", PlayerPrefs.GetInt("LastMusic1"));
                    PlayerPrefs.SetInt("LastMusic1", newIndex);

                    audioSource.clip = workSongs[newIndex];

                    audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                    audioSource.PlayOneShot(audioSource.clip);
                }

            }

            yield return new WaitForEndOfFrame();

        }

        backgroundMusicPlayerInstance = null;

    }

    // Unlike other managers, this isn't a coroutine since it should happen instantly and not delay the game
    public void StartMusicEvent(MusicEvent toExecute)
    {

        if (toExecute.fadeOut)
        {
            if (fadeOutCoroutine == null)
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
        if (fadeOutCoroutine == null)
            fadeOutCoroutine = StartCoroutine("FadeOutMusic");
    }

    private IEnumerator FadeOutMusic()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }

        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");

        while (audioSource.volume > 0)
        {
            if (audioSource.volume > .1)
            {
                audioSource.volume -= (float)(.45 * Time.deltaTime * PlayerPrefs.GetFloat("MusicVolume"));
            }
            // Allows fade out to be more gradual
            else if (audioSource.volume < .13)
            {
                audioSource.volume -= (float)(.08 * Time.deltaTime * PlayerPrefs.GetFloat("MusicVolume"));
            }
            else if (audioSource.volume < (.02))
            {
                audioSource.volume -= (float)(.015 * Time.deltaTime * PlayerPrefs.GetFloat("MusicVolume"));
            }

            yield return new WaitForEndOfFrame();

        }

        audioSource.Stop();
        fadeOutCoroutine = null;

    }

    public void AttemptMusicVolumeChange()
    {
        if (volumeChangeCoroutine == null)
            volumeChangeCoroutine = StartCoroutine("ChangeVolume");
    }

    IEnumerator ChangeVolume()
    {
        while (fadeOutCoroutine != null)
        {
            yield return new WaitForEndOfFrame();
        }

        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        volumeChangeCoroutine = null;
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
