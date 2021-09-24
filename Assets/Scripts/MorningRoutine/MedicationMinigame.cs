using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicationMinigame : Minigame
{
    public LidInteractable lidInteractable;
    public PillInteractable pillInteractable;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        lidInteractable = GetComponentInChildren<LidInteractable>();
        pillInteractable = GetComponentInChildren<PillInteractable>();
        lidInteractable.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    new void OnMouseDown() { 
        //Debug.Log("Minigame " + minigameName + " clicked");
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive || MorningRoutineManager.Instance.currentMinigame != null || SceneChangeManager.instance.IsFading()) {
            return;
        }
        if(!isGameActive) {
            isGameActive = true;
            BeginGame();
        }
    }

    new void BeginGame() {
        base.BeginGame();
        //Debug.Log("medicine game start");
        lidInteractable.SetActive(true);
    }

    public void StopMedGame() {
        //Debug.Log("medicine game end");
        
        lidInteractable.CloseLid();
        lidInteractable.SetActive(false);
        lidInteractable.Reset();
        pillInteractable.Reset();
        base.StopGame();
    }

    public void IncrementDay() {
        this.StopGame();
        lidInteractable.Reset();
        pillInteractable.Reset();
        PlayerPrefs.SetInt("TookPill", 0);
    }


}