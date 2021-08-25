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
    }

    public void OnMouseDown() {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            return;
        }
        if(!isTVActive && !TVEventPlayed) {
            isTVActive = true;
            BeginTVEvent();
        }
    }

    public void BeginTVEvent() {
        if(isTVActive) {
            clickArea.enabled = false;

            focusCamera.SetActive(true);
            mainCamera.SetActive(false);

            MorningRoutineManager.Instance.SetTVActive(true);
            //MorningRoutineManager.Instance.SetMinigame(null);
            //do something to start the dialogue

            if(!TVEventPlayed) {
                if(GameManager.instance.currentDay.newsEvent != null) {
                    GameManager.instance.StartSequence(GameManager.instance.currentDay.newsEvent);
                    TVEventPlayed = true;
                }
            }
        }
    }

    public void StopTVEvent() {
        if(isTVActive) {
            isTVActive = false;

            clickArea.enabled = true;

            mainCamera.SetActive(true);
            focusCamera.SetActive(false);
            MorningRoutineManager.Instance.SetTVActive(false);
        }
    }

    public void Reset() {
        TVEventPlayed = false;
    }
}
