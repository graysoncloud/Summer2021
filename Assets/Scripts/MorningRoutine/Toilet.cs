using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    public AudioClip soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        MorningRoutineManager.Instance.audioManager.LoadSound(soundEffect);
        MorningRoutineManager.Instance.audioManager.PlaySoundForce();
    }
}
