using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRAudioManager : MonoBehaviour
{
    public AudioClip currentSound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSound(AudioClip a) {
        currentSound = a;
    }

    public void PlaySound() {
        if(currentSound.loadState == AudioDataLoadState.Loaded && !audioSource.isPlaying) {
            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
            audioSource.PlayOneShot(currentSound);
        }
    }

    public void PlaySoundForce() {
        if(currentSound.loadState == AudioDataLoadState.Loaded) {
            audioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
            audioSource.PlayOneShot(currentSound);
        }
    }
}
