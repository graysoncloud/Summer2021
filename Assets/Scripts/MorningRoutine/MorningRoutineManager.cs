using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MorningRoutineManager : Singleton<MorningRoutineManager>
{
    public Button backButton;
    public TextMeshProUGUI dayCounterTxt;
    public GameObject MRUI;

    public Minigame currentMinigame = null;

    public FlowerMinigame flowerMinigame;
    public ShavingMinigame shavingMinigame;
    public BedMinigame bedMinigame;
    public MedicationMinigame medicationMinigame;
    public TVInteractable tvInteractable;
    // this is the global variable to check if you took your medicine today
    public bool takenMedicationToday = false;
    public bool enteredLRToday = false;

    public SceneChange mrToOffice;
    bool isTVActive = false;

    public MRAudioManager audioManager;

    public Camera mainCamera;
    public Camera migraineCamera;

    public GameObject dayTextContainer;


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
        tvInteractable = FindObjectOfType<TVInteractable>();
        audioManager = FindObjectOfType<MRAudioManager>();

        gameDay = PlayerPrefs.GetInt("CurrentDayIndex") + 1;
    }

    void LoadInteractables() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            //MRUI.SetActive(false);
        } else {
            if(MRUI.activeInHierarchy == false) {
                 //MRUI.SetActive(true);
            }
        }

        if(dayCounterTxt != null) {
            dayCounterTxt.text = "DAY " + gameDay;
        }

    }

    public void SetMinigame(Minigame m) {
        this.currentMinigame = m;
        //backButton.gameObject.SetActive(true);
    }

    public void StopMinigame() {
        if(this.currentMinigame != null) {
            this.currentMinigame.StopGame();
            this.currentMinigame = null;
        }
        if(isTVActive) {
            tvInteractable.StopTVEvent();
        }
        if(medicationMinigame.isGameActive) {
            medicationMinigame.StopMedGame();
        }
        //backButton.gameObject.SetActive(false);
    }

    public void StartNewDay() {
        gameDay++;

        takenMedicationToday = false;
        enteredLRToday = false;

        flowerMinigame.IncrementDay();
        shavingMinigame.IncrementDay();
        bedMinigame.IncrementDay();
        medicationMinigame.IncrementDay();
        tvInteractable.Reset();

        //SetMinigame(bedMinigame);

        
    }

    public void SetTVActive(bool a) {
        isTVActive = a;
    }

    void OnEnable() {
        StartCoroutine("FadeDayText");
    }

    IEnumerator FadeDayText() {
        dayTextContainer.SetActive(true);
        for(float i = 0f; i <= 1f; i += 0.01f) {
            dayTextContainer.GetComponent<CanvasRenderer>().SetAlpha(i);
            dayCounterTxt.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        dayTextContainer.GetComponent<CanvasRenderer>().SetAlpha(1);
        dayCounterTxt.alpha = 1;
        yield return new WaitForSeconds(5f);

        for(float i = 1f; i >= 0f; i -= 0.01f) {
            dayTextContainer.GetComponent<CanvasRenderer>().SetAlpha(i);
            dayCounterTxt.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        dayTextContainer.GetComponent<CanvasRenderer>().SetAlpha(0);
        dayCounterTxt.alpha = 0;
        dayTextContainer.SetActive(true);
    }
}