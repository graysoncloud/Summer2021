using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpButton : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject helpMenu;
    public GameObject textObj;
    private TextMeshProUGUI textTMP;

    void Start(){
        textTMP = textObj.GetComponent<TextMeshProUGUI>();
        textTMP.text = "Help";
    }
    void OnMouseUp(){
        if (GameManager.instance.optionsMenuActive)
            return;
        if (helpMenu.activeSelf){
            helpMenu.SetActive(false);
            textTMP.text = "Help";
        }
        else if(!helpMenu.activeSelf && DrugManager.instance.tutorialsfinished)
        {
            helpMenu.SetActive(true);
            textTMP.text = "Back";
        }
    }

    void Update(){
        if(helpMenu.activeSelf){
            textTMP.text = "Back";
        }
        else{
            textTMP.text = "Help";
        }
    }
}
