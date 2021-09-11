using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject ContractTutorial1;
    public GameObject ContractTutorial2;
    public GameObject ContractTutorial3;
    public GameObject ContractTutorial4;
    public GameObject ContractTutorial5;
    public GameObject DrugGameTutorial1;
    public GameObject DrugGameTutorial2;
    public GameObject DrugGameTutorial3;
    public GameObject DrugGameTutorial4;

    public GameObject DrugGameTutorial5;

    public GameObject DrugGameTutorial6;

    public GameObject DrugGameTutorial7;
    public GameObject DrugGameTutorial8;
    public GameObject DrugGameTutorial9;
    public GameObject DrugGameTutorial10;
    public GameObject DrugGameTutorial11;
    public GameObject DrugGameTutorial12;
    public GameObject RecapTutorial1;
    public GameObject RecapTutorial2;
    public GameObject RecapTutorial3;
    public GameObject RecapTutorial4;
    public GameObject RecapTutorial5;

    // Add more tutorial scenes

    public GameObject activeTutorial;

    [SerializeField]
    private bool readyToStopTutorial;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        readyToStopTutorial = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && readyToStopTutorial) {
            if (activeTutorial != null)
                activeTutorial.SetActive(false);

            activeTutorial = null;
        }
    }

    public void ActivateTutorial(GameObject toActivate)
    {
        // Checks if the tutorial already occured
        if (PlayerPrefs.GetInt(toActivate + "Triggered") == 1)
            return;

        StartCoroutine(DelayedExecute(toActivate));

    }

    public void ActivateReplayableTutorial(GameObject toActivate)
    {
        StartCoroutine(DelayedExecute(toActivate));  
    }

    IEnumerator DelayedExecute(GameObject toActivate)
    {
        readyToStopTutorial = false;
        activeTutorial = toActivate;

        yield return new WaitForSeconds(.25f);

        toActivate.SetActive(true);
        PlayerPrefs.SetInt(toActivate + "Triggered", 1);
        readyToStopTutorial = true;
    }

    private IEnumerator ClickDelay()
    {
        readyToStopTutorial = false;
        yield return new WaitForSeconds(.4f);
        readyToStopTutorial = true;
    }



}
