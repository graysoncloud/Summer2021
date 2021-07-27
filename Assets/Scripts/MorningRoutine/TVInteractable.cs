using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteractable : MonoBehaviour
{
    public GameObject focusCamera;
    public GameObject mainCamera;
    public BoxCollider2D clickArea;

    public bool isTVActive = false;

    // Start is called before the first frame update
    void Start()
    {
        clickArea = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown() {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            return;
        }
        if(!isTVActive) {
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
            MorningRoutineManager.Instance.SetMinigame(null);
            //do something to start the dialogue
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
}
