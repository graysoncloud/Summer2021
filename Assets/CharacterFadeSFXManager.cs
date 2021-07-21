using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFadeSFXManager : MonoBehaviour
{
    public static CharacterFadeSFXManager instance = null;

    // Needs to match ConversationSFX enum exactly
    public AudioClip[] SFX;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PlaySFX(CharacterFadeSFX toPlay)
    {
        GetComponent<AudioSource>().clip = SFX[(int)toPlay];
        GetComponent<AudioSource>().Play();
    }

}

public enum CharacterFadeSFX
{
    Footsteps
}


