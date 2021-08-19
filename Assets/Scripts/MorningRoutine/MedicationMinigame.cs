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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    new void OnMouseDown() { 
        //Debug.Log("Minigame " + minigameName + " clicked");
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
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

    new void StopGame() {
        //Debug.Log("medicine game end");
        base.StopGame();
    }

    public void IncrementDay() {
        this.StopGame();
        lidInteractable.Reset();
        pillInteractable.Reset();
        PlayerPrefs.SetInt("TookPill", 0);
    }


}