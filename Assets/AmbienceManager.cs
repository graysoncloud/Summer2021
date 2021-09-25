using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager instance;

    public AudioSource audioSource;

    public AudioClip[] ambienceArray;

    private Coroutine volumeChangeCoroutine;
    private Coroutine fadeOutCoroutine;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void PlayAmbience(int index)
    {
        audioSource.Stop();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        audioSource.PlayOneShot(ambienceArray[index]);
    }

    public void StartFadeOut()
    {
        if (fadeOutCoroutine == null)
            fadeOutCoroutine = StartCoroutine("FadeOutAmbience");
    }


    private IEnumerator FadeOutAmbience()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");

        while (audioSource.volume > 0)
        {

            audioSource.volume -= (float)(.004 * PlayerPrefs.GetFloat("SFXVolume"));
            yield return new WaitForSeconds(.005f);

            // Allows fade out to be more gradual
            if (audioSource.volume < (.1 * PlayerPrefs.GetFloat("SFXVolume")))
                yield return new WaitForSeconds(.022f);

            if (audioSource.volume < (.02 * PlayerPrefs.GetFloat("SFXVolume")))
                yield return new WaitForSeconds(.04f);
        }

        audioSource.Stop();
        fadeOutCoroutine = null;

    }

    public void AttemptSFXVolumeChange()
    {
        if (volumeChangeCoroutine == null)
            volumeChangeCoroutine = StartCoroutine("ChangeVolume");
    }

    IEnumerator ChangeVolume()
    {
        while (fadeOutCoroutine != null)
            yield return new WaitForEndOfFrame();

        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
    }


}

public enum Ambience
{
    OfficeAmbience,
    RainAmbience
}

