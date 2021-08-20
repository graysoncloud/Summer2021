using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip[] audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySoundEffect(int indexToPlay)
    {
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
        audioSource.clip = audioClips[indexToPlay];
        audioSource.PlayOneShot(audioSource.clip);
    }

    // Debugging
    //public IEnumerator PlayStuff()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        PlaySoundEffect(0);

    //    }
    //}

}
