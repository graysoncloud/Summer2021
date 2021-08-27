using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject helpMenu;
    void OnMouseUp(){
        if(helpMenu.activeSelf){
            helpMenu.SetActive(false);
        }
        else
        {
            helpMenu.SetActive(true);
        }
    }
}
