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
        if (Input.GetMouseButtonDown(0)) {
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

        StartCoroutine("ClickDelay");

        activeTutorial = toActivate;
        toActivate.SetActive(true);
        PlayerPrefs.SetInt(toActivate + "Triggered", 1);

    }

    private IEnumerator ClickDelay()
    {
        readyToStopTutorial = false;
        yield return new WaitForSeconds(.4f);
        readyToStopTutorial = true;
    }



}
