using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteractable : MonoBehaviour
{
    public GameObject focusCamera;
    public GameObject mainCamera;
    public BoxCollider2D clickArea;

    public bool isTVActive = false;

    public bool TVEventPlayed = false;
    public TVReport tVReport;

    // Start is called before the first frame update
    void Start()
    {
        clickArea = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(TVEventPlayed) {
            if(!GameManager.instance.sequenceActive && isTVActive) {
                StopTVEvent();
            }
        }
        if(GameManager.instance.currentDay.newsEvent != null && !TVEventPlayed) {
            GetComponent<GlowEffect>().hoverGlow = true;
        }
        else {
            GetComponent<GlowEffect>().hoverGlow = false;
        }
    }

    public void OnMouseDown() {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            return;
        }
        if(!isTVActive && !TVEventPlayed) {
            isTVActive = true;
            BeginTVEvent();
            tVReport.StartNews();
        }
    }

    public void BeginTVEvent() {
        if(isTVActive) {
            clickArea.enabled = false;

            focusCamera.SetActive(true);
            mainCamera.SetActive(false);

            PlayerPrefs.SetInt("WatchedNews", 1);

            MorningRoutineManager.Instance.SetTVActive(true);
            //MorningRoutineManager.Instance.SetMinigame(null);
            //do something to start the dialogue

            if(!TVEventPlayed) {
                if(GameManager.instance.currentDay.newsEvent != null) {
                    TVEventPlayed = true;
                    GameManager.instance.StartSequence(GameManager.instance.currentDay.newsEvent);
                }
            }
        }
    }

    IEnumerator PlayTVEvent()
    {
        yield return new WaitForSeconds(.5f);
    }

    public void StopTVEvent() {
        if(isTVActive) {
            isTVActive = false;

            clickArea.enabled = true;

            mainCamera.SetActive(true);
            focusCamera.SetActive(false);
            MorningRoutineManager.Instance.SetTVActive(false);
            tVReport.StopNews();
        }
    }

    public void Reset() {
        TVEventPlayed = false;
    }
}
