using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OSOverlay : MonoBehaviour
{
    public static OSOverlay instance;

    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private TextMeshProUGUI continueText;

    private bool waitingForClick;

    [SerializeField]
    private SceneChange osToDrugGame;

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
        StartOSDisplay();
        waitingForClick = false;
        continueText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (waitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                waitingForClick = false;
                SceneChangeManager.instance.StartSceneChange(osToDrugGame);
            }
        }
    }

    public void StartOSDisplay()
    {
        progressBar.value = 0;
        gameObject.SetActive(true);
        continueText.gameObject.SetActive(false);

        GameManager.instance.sequenceActive = true;

        if (GameManager.instance.currentDayIndex == 0)
        {
            StartCoroutine("LoadOSFirstDay");
        } else
        {
            StartCoroutine("LoadOSNormal");
        }
    }

    IEnumerator LoadOSFirstDay ()
    {
        float rate = .1f;

        while (progressBar.value < 12)
        {
            progressBar.value += rate * Time.deltaTime * 60;
            rate *= 1.0025f;
            yield return new WaitForEndOfFrame();
        }

        while (progressBar.value < 25)
        {
            progressBar.value += rate * Time.deltaTime * 60;
            rate *= .9995f;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.3f);

        rate = .3f;

        while (progressBar.value < 65)
        {
            progressBar.value += rate * Time.deltaTime * 60;
            rate *= 1.0025f;
            yield return new WaitForEndOfFrame();
        }

        while (progressBar.value < 100)
        {
            progressBar.value += rate * Time.deltaTime * 60;
            yield return new WaitForEndOfFrame();
        }

        waitingForClick = true;

        yield return new WaitForSeconds(.4f);

        continueText.gameObject.SetActive(true);
    }

    IEnumerator LoadOSNormal ()
    {
        float rate = .015f;

        while (progressBar.value < 50)
        {
            progressBar.value += rate * Time.deltaTime * 60;
            rate *= 1.003f;
            yield return new WaitForEndOfFrame();
        }

        while (progressBar.value < 100)
        {
            progressBar.value += rate * Time.deltaTime * 60;
            yield return new WaitForEndOfFrame();
        }

        waitingForClick = true;

        yield return new WaitForSeconds(.8f);

        continueText.gameObject.SetActive(true);


    }

}
