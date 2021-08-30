using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameExit : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if(MorningRoutineManager.Instance.currentMinigame != null) {
            MorningRoutineManager.Instance.StopMinigame();
        }
    }
}
