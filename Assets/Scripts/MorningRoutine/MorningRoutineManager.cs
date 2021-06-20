using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MorningRoutineManager : Singleton<MorningRoutineManager>
{
    public Button backButton;
    public TextMeshProUGUI dayCounterTxt;

    public Minigame currentMinigame = null;

    public FlowerMinigame flowerMinigame;

    // game data
    // the day
    public int gameDay = 1;

    // Start is called before the first frame update
    void Start()
    {
        flowerMinigame = FindObjectOfType<FlowerMinigame>();
    }

    void LoadInteractables() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dayCounterTxt != null) {
            dayCounterTxt.text = "Day: " + gameDay;
        }
    }

    public void SetMinigame(Minigame m) {
        this.currentMinigame = m;
        backButton.gameObject.SetActive(true);
    }

    public void StopMinigame() {
        this.currentMinigame.StopGame();
        this.currentMinigame = null;
        backButton.gameObject.SetActive(false);
    }

    public void StartNewDay() {
        gameDay++;

        flowerMinigame.IncrementDay();
    }
}
