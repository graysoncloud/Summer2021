using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public string minigameName;
    public GameObject focusCamera;
    public GameObject mainCamera;
    public BoxCollider2D minigameClickArea;

    public bool isGameActive = false;

    public Interactable[] interactableList;

    public void Start()
    {
        //focusCamera.SetActive(false);
        minigameClickArea = GetComponent<BoxCollider2D>();

        interactableList = GetComponentsInChildren<Interactable>();

        foreach(Interactable i in interactableList) {
            i.SetInteractableActive(false);
            i.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void OnMouseDown() {
        
        if(!isGameActive && !ConversationManager.instance.inConversation) {
            //Debug.Log("Minigame " + minigameName + " clicked");
            isGameActive = true;
            BeginGame();
        }
        
    }

    

    public void BeginGame() {
        if(isGameActive) {
            //Debug.Log(minigameName + " game started");
            minigameClickArea.enabled = false;

            foreach(Interactable i in interactableList) {
                i.SetInteractableActive(true);
                i.GetComponent<BoxCollider2D>().enabled = true;
            }

            focusCamera.SetActive(true);
            mainCamera.SetActive(false);   
            
            MorningRoutineManager.Instance.SetMinigame(this);
        }
    }

    public void StopGame() {
        if(isGameActive) {
            //Debug.Log(minigameName + " game ended");
            isGameActive = false;
            minigameClickArea.enabled = true;
        }

        foreach(Interactable i in interactableList) {
            i.SetInteractableActive(false);
            i.GetComponent<BoxCollider2D>().enabled = false;
        }

        mainCamera.SetActive(true);
        focusCamera.SetActive(false);
    }
}
