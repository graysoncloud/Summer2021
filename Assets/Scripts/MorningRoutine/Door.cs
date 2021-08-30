using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;

    public Room destination;
    public bool workDoor;

    public AudioClip hoverSoundEffect;
    public AudioClip clickSoundEffect;

    bool soundEffectPlayed = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver() {
        if (GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive || SceneChangeManager.instance.IsFading())
        {
            return;
        }

        spriteRenderer.sprite = openSprite;
        if(!soundEffectPlayed) {
            soundEffectPlayed = true;
            MorningRoutineManager.Instance.audioManager.LoadSound(hoverSoundEffect);
            MorningRoutineManager.Instance.audioManager.PlaySoundForce();
        }
        
    }

    void OnMouseDown() {
        if (GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive || SceneChangeManager.instance.IsFading())
        {
            return;
        }

        if(!workDoor) {
            FindObjectOfType<PlayerController>().SetRoom(destination);
            MorningRoutineManager.Instance.audioManager.LoadSound(clickSoundEffect);
            MorningRoutineManager.Instance.audioManager.PlaySoundForce();
        } else {
            GoToWork();
            MorningRoutineManager.Instance.audioManager.LoadSound(clickSoundEffect);
            MorningRoutineManager.Instance.audioManager.PlaySoundForce();
        }
        
    }

    void OnMouseExit() {
        if (GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive)
        {
            return;
        }
        
        spriteRenderer.sprite = closedSprite;
        MorningRoutineManager.Instance.audioManager.LoadSound(clickSoundEffect);
        MorningRoutineManager.Instance.audioManager.PlaySoundForce();
        soundEffectPlayed = false;
    }

    void GoToWork() {
        foreach (Day.Sequence dialogueEvent in GameManager.instance.currentDay.sequences)
        {
            if (dialogueEvent.trigger.ToString() == "leavingHome")
            {
                GameManager.instance.StartSequence(dialogueEvent.initialEvent);
                return;
            }

        }

        SceneChangeManager.instance.StartSceneChange(MorningRoutineManager.Instance.mrToOffice);
    }
}
