using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MorningRoutineManager : Singleton<MorningRoutineManager>
{
    public Button backButton;
    public GameObject roomButtons;
    public TextMeshProUGUI dayCounterTxt;
    public GameObject MRUI;

    public Minigame currentMinigame = null;

    public FlowerMinigame flowerMinigame;
    public ShavingMinigame shavingMinigame;
    public BedMinigame bedMinigame;
    public MedicationMinigame medicationMinigame;

    // this is the global variable to check if you took your medicine today
    public bool takenMedicationToday = false;

    public SceneChange mrToOffice;

    // game data
    // the day
    public int gameDay = 1;

    // Start is called before the first frame update
    void Start()
    {
        flowerMinigame = FindObjectOfType<FlowerMinigame>();
        shavingMinigame = FindObjectOfType<ShavingMinigame>();
        bedMinigame = FindObjectOfType<BedMinigame>();
        medicationMinigame = FindObjectOfType<MedicationMinigame>();
    }

    void LoadInteractables() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            MRUI.SetActive(false);
        } else {
            if(MRUI.activeInHierarchy == false) {
                 MRUI.SetActive(true);
            }
        }

        if(dayCounterTxt != null) {
            dayCounterTxt.text = "Day: " + gameDay;
        }
    }

    public void SetMinigame(Minigame m) {
        this.currentMinigame = m;
        backButton.gameObject.SetActive(true);
        roomButtons.SetActive(false);
    }

    public void StopMinigame() {
        this.currentMinigame.StopGame();
        this.currentMinigame = null;
        backButton.gameObject.SetActive(false);
        roomButtons.SetActive(true);
    }

    public void StartNewDay() {
        gameDay++;

        takenMedicationToday = false;

        flowerMinigame.IncrementDay();
        shavingMinigame.IncrementDay();
        bedMinigame.IncrementDay();
        medicationMinigame.IncrementDay();
    }
}