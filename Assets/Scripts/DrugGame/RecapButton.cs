using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapButton : MonoBehaviour
{

    public GameObject helpMenu;

    // Update is called once per frame
    void OnMouseUp(){
        if (GameManager.instance.optionsMenuActive)
            return;
        if (helpMenu.activeSelf){
            DrugManager.instance.allrecapfinished = false;
            DrugManager.instance.numrecapfinished = 0;
            helpMenu.SetActive(false);
        }
    }
}
